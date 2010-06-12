/* Copyright 2009 dotless project, http://www.dotlesscss.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *     
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License. */

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