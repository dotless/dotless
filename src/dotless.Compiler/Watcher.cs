namespace dotless.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;

    public class Watcher
    {
        private Func<IEnumerable<string>> CompilationDelegate { get; set; }
        private Dictionary<string, FileSystemWatcher> FileSystemWatchers { get; set; }

        public Watcher(IEnumerable<string> files, Func<IEnumerable<string>> compilationDelegate)
        {
            CompilationDelegate = compilationDelegate;
            FileSystemWatchers = new Dictionary<string, FileSystemWatcher>();

            SetupWatchers(files);
        }

        private void SetupWatchers(IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                if (fileInfo.Directory == null)
                    throw new IOException("File Path has no directory to watch");

                if(FileSystemWatchers.ContainsKey(file))
                    continue;

                Console.WriteLine("Started watching '{0}' for changes", file);

                var directoryFullName = fileInfo.Directory.FullName;
                var fsWatcher = new FileSystemWatcher(directoryFullName, fileInfo.Name);
                fsWatcher.Changed += FsWatcherChanged;
                fsWatcher.EnableRaisingEvents = true;

                FileSystemWatchers[file] = fsWatcher;
            }

            var missing = FileSystemWatchers.Keys.Where(f => !files.Contains(f)).ToList();

            foreach (var file in missing)
            {
                var fsWatcher = FileSystemWatchers[file];

                fsWatcher.Changed -= FsWatcherChanged;

                fsWatcher.Dispose();

                FileSystemWatchers.Remove(file);

                Console.WriteLine("Stopped watching '{0}'", file);
            }
        }
        
        void FsWatcherChanged(object sender, FileSystemEventArgs e)
        {
            if (IsDuplicateEvent()) return;

            var fsWatcher = (FileSystemWatcher) sender;
            var file = FileSystemWatchers.First(d => d.Value == fsWatcher).Key;

            var completed = false;
            Console.WriteLine("Found change in '{0}'. Recompiling...", file);
            while(!completed)
            {
                try
                {
                    var files = CompilationDelegate();
                    SetupWatchers(files);
                    completed = true;
                }
                catch(IOException)
                {
                    Thread.Sleep(100);
                    Console.WriteLine("[Waiting]");
                    Console.WriteLine("File still locked, waiting 100ms");
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
    }
}