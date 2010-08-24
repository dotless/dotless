namespace dotless.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;

    public delegate CompilationDelegate CompilationFactoryDelegate(string input);
    public delegate IEnumerable<string> CompilationDelegate();

    class Watcher : IDisposable
    {
        private static Dictionary<string, CompilationDelegate> CompilationDelegates = new Dictionary<string, CompilationDelegate>();
        private static Dictionary<string, CompilationFactoryDelegate> CreationDelegates = new Dictionary<string, CompilationFactoryDelegate>();
        private static List<FileSystemWatcher> FileSystemWatchers = new List<FileSystemWatcher>();
        public bool Watch { get; set; }

        private object _eventLock = new object();

        public void SetupDirectoryWatcher(string directoryPath, string pattern, CompilationFactoryDelegate del)
        {
            var fsWatcher = new FileSystemWatcher(directoryPath, pattern);
            fsWatcher.Created += FileCreatedHandler;
            fsWatcher.EnableRaisingEvents = true;
            Console.WriteLine("Started watching '{0}' for changes", directoryPath + "\\" + pattern);
            CreationDelegates.Add(directoryPath, del);
            FileSystemWatchers.Add(fsWatcher);
        }

        public void SetupWatchers(IEnumerable<string> files, CompilationDelegate del)
        {
            foreach (var path in files)
            {
                if (!CompilationDelegates.ContainsKey(path))
                {
                    var fsWatcher = new FileSystemWatcher(Path.GetDirectoryName(path), Path.GetFileName(path));
                    fsWatcher.Changed += FileChangedHandler;
                    fsWatcher.Deleted += FileDeletedHandler;
                    fsWatcher.EnableRaisingEvents = true;
                    Console.WriteLine("Started watching '{0}' for changes", path);
                    CompilationDelegates.Add(path, del);
                    FileSystemWatchers.Add(fsWatcher);
                }
                else
                {
                    lock (_eventLock)
                    {
                        bool add = true;
                        foreach (var d in CompilationDelegates[path].GetInvocationList())
                        {
                            if (d.Target.Equals(del.Target)) add = false;
                        }
                        if (add) CompilationDelegates[path] += del;
                    }
                }
            }
        }

        void FileCreatedHandler(object sender, FileSystemEventArgs e)
        {
            var directoryPath = Path.GetDirectoryName(e.FullPath);
            if (CreationDelegates.ContainsKey(directoryPath))
            {
                var compilationDelegate = CreationDelegates[directoryPath](e.FullPath);
                Console.WriteLine("[Compile]");
                var files = compilationDelegate();
                SetupWatchers(files, compilationDelegate);
            }
        }

        void FileDeletedHandler(object sender, FileSystemEventArgs e)
        {
            var fsWatcher = sender as FileSystemWatcher;
            fsWatcher.EnableRaisingEvents = false;
            Console.WriteLine("Stopped watching '{0}'", e.FullPath);
            FileSystemWatchers.Remove(fsWatcher);
            fsWatcher.Changed -= null;
            fsWatcher.Dispose();

            var path = e.FullPath;
            if (CompilationDelegates.ContainsKey(path))
            {
                lock (_eventLock)
                {
                    var del = CompilationDelegates[path];
                    CompilationDelegates.Remove(path);
                    List<string> toberemoved = new List<string>();
                    foreach (var key in CompilationDelegates.Keys)
                        foreach (var d in CompilationDelegates[key].GetInvocationList())
                            if (d.Target.Equals(del.Target))
                                toberemoved.Add(key);
                    foreach (var key in toberemoved) 
                        CompilationDelegates[key] -= del;
                }
            }
        }

        void FileChangedHandler(object sender, FileSystemEventArgs e)
        {
            if (IsDuplicateEvent()) return;
            var compilationDelegate = CompilationDelegates[e.FullPath];
            var completed = false;
            Console.WriteLine("[Change in {0}]", e.Name);
            Console.WriteLine("[Recompile]");
            while (!completed)
            {
                try
                {
                    var files = compilationDelegate();
                    SetupWatchers(files, compilationDelegate);
                    completed = true;
                }
                catch (IOException)
                {
                    Console.WriteLine("[Waiting(File locked)]");
                    Thread.Sleep(300);
                }
            }
        }

        private static DateTime lastEventOccured;

        private bool IsDuplicateEvent()
        {
            DateTime fileTimeUtc = DateTime.Now;
            var timeSpan = fileTimeUtc.Subtract(lastEventOccured);

            if (timeSpan.TotalMilliseconds < 500)
            {
                return true;
            }
            lastEventOccured = fileTimeUtc;
            return false;
        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            foreach (var fsWatcher in FileSystemWatchers)
            {
                fsWatcher.EnableRaisingEvents = false;
                fsWatcher.Changed -= null;
                fsWatcher.Dispose();
                Console.WriteLine("Stopped watching '{0}'", fsWatcher.Filter);
            }
            FileSystemWatchers.Clear();
            CompilationDelegates.Clear();
            CreationDelegates.Clear();
        }

        #endregion
    }
}
