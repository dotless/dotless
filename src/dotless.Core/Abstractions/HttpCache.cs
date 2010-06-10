namespace dotless.Core.Abstractions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Caching;

    public class HttpCache : ICache
    {
        private readonly HttpContextBase _context;
        public IPathResolver PathResolver { get; set; }

        public HttpCache(HttpContextBase context, IPathResolver pathResolver)
        {
            _context = context;
            PathResolver = pathResolver;
        }

        public void Insert(string fileName, IEnumerable<string> imports, string css)
        {
            var fullPaths = imports.Concat(new[] { fileName }).Select(f => PathResolver.GetFullPath(f)).ToArray();

            var cache = GetCache();

            cache.Insert(fileName, css, new CacheDependency(fullPaths));
        }

        public bool Exists(string filename)
        {
            return Retrieve(filename) != null;
        }

        public string Retrieve(string filename)
        {
            return (string)GetCache()[filename];
        }

        private Cache GetCache()
        {
            return _context.Cache;
        }
    }
}