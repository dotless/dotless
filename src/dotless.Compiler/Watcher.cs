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
    using System.IO;
    using System.Threading;

    public class Watcher
    {
        private readonly Action compilationDelegate;
        public Watcher(string inputFilePath, Action compilationDelegate)
        {
            this.compilationDelegate = compilationDelegate;

            SetupWatcher(inputFilePath);

            Console.WriteLine("Watching " + inputFilePath + " for changes");
        }

        private void SetupWatcher(string inputFilePath)
        {
            var fileInfo = new FileInfo(inputFilePath);
            if (fileInfo.Directory == null)
                throw new IOException("File Path has no directory to watch");

            string directoryFullName = fileInfo.Directory.FullName;
            var fsWatcher = new FileSystemWatcher(directoryFullName, fileInfo.Name);
            fsWatcher.Changed += FsWatcherChanged;
            fsWatcher.EnableRaisingEvents = true;
        }
        
        void FsWatcherChanged(object sender, FileSystemEventArgs e)
        {
            if (IsDuplicateEvent()) return;

            bool completed = false;
            Console.WriteLine("Found change in file. Recompiling...");
            while(!completed){
                try
                {
                    compilationDelegate();
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