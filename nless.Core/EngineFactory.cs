namespace nless.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using configuration;

    public class EngineFactory
    {
        public ILessEngine GetEngine(DotlessConfiguration configuration)
        {
            ILessEngine engine = new LessEngine();
            if (configuration.MinifyOutput)
                engine = new MinifierDecorator(engine);
            CacheConfig cacheConfig = configuration.CacheConfiguration;
            if (cacheConfig.CacheEnabled)
            {
                if (cacheConfig.CacheStragy == "file")
                    engine = new FileWatcherCacheDecorator(engine);
                if (cacheConfig.CacheStragy == "expiration")
                    engine = new TimeExpirationCacheDecorator(engine, cacheConfig.CacheExpiration);
            }
            return engine;
        }
    }

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
            var watcher = new FileSystemWatcher(filename);
            watcher.Changed += (sender, e) =>
                                   {
                                       cache.Remove(filename);
                                       watchers.Remove((FileSystemWatcher)sender);
                                   };
            watchers.Add(watcher);
            return css;
        }
    }

    public class TimeExpirationCacheDecorator : ILessEngine
    {
        private readonly ILessEngine lessEngine;
        private readonly long expiration;
        private readonly IDictionary<string, CacheItem> cache = new Dictionary<string, CacheItem>();
        public TimeExpirationCacheDecorator(ILessEngine lessEngine, long expiration)
        {
            this.lessEngine = lessEngine;
            this.expiration = expiration;
        }

        public string TransformToCss(string filename)
        {
            if (cache.ContainsKey(filename))
            {
                var cacheItem = cache[filename];
                if (cacheItem.TimeStamp.AddSeconds(expiration) < DateTime.UtcNow)
                {
                    return cacheItem.Content;
                }
                cache.Remove(filename);
            }
            string css = lessEngine.TransformToCss(filename);
            cache.Add(filename, new CacheItem {Content = css, TimeStamp = DateTime.UtcNow});
            return css;
        }

        private class CacheItem
        {
            public string Content { get; set; }
            public DateTime TimeStamp { get; set; }
        }
    }
}