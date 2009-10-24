namespace nless.Core
{
    using System.Collections.Generic;
    using System.IO;

    public class FileWatcherCacheDecorator : ILessEngine
    {
        private readonly ILessEngine engine;

        private IDictionary<string, string> cache = new Dictionary<string, string>();
        private IList<FileSystemWatcher> watchers = new List<FileSystemWatcher>();

        public FileWatcherCacheDecorator(ILessEngine engine)
        {
            this.engine = engine;
        }

        public string TransformToCss(string filename)
        {
            if (cache.ContainsKey(filename))
            {
                return cache[filename];
            }
            string css = engine.TransformToCss(filename);
            cache.Add(filename, css);
            var info = new FileInfo(filename);
            string dir = info.Directory.FullName;
            var watcher = new FileSystemWatcher(dir, info.Name);
            watcher.Changed += (sender, e) =>
                                   {
                                       cache.Remove(filename);
                                       watchers.Remove((FileSystemWatcher)sender);
                                   };
            watchers.Add(watcher);
            return css;
        }
    }
}