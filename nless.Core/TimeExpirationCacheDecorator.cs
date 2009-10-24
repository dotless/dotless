namespace nless.Core
{
    using System;
    using System.Collections.Generic;

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