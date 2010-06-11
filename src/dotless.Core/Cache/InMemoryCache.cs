namespace dotless.Core.Cache
{
    using System.Collections.Generic;

    class InMemoryCache : ICache
    {
        private readonly Dictionary<string, string> _cache;

        public InMemoryCache()
        {
            _cache = new Dictionary<string, string>();
        }

        public void Insert(string fileName, IEnumerable<string> imports, string css)
        {
            _cache[fileName] = css;
        }

        public bool Exists(string filename)
        {
            return _cache.ContainsKey(filename);
        }

        public string Retrieve(string filename)
        {
            if (_cache.ContainsKey(filename))
                return _cache[filename];

            return "";
        }
    }
}