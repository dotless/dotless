using System.IO;

namespace dotless.Core.Cache
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Caching;
    using Abstractions;
    using Input;

    public class HttpCache : ICache
    {
        private readonly IHttp _http;
        private readonly IFileReader _reader;
        public IPathResolver PathResolver { get; set; }

        public HttpCache(IHttp http, IPathResolver pathResolver, IFileReader reader)
        {
            _http = http;
            PathResolver = pathResolver;
            _reader = reader;
        }

        public void Insert(string cacheKey, IEnumerable<string> fileDependancies, string css)
        {
            var cache = GetCache();

            if (_reader.UseCacheDependencies)
            {
                var fullPaths = fileDependancies.Select(f => PathResolver.GetFullPath(f)).Where(File.Exists).ToArray();

                _http.Context.Response.AddFileDependencies(fullPaths);

                cache.Insert(cacheKey, css, new CacheDependency(fullPaths));
            }
            else
            {
                cache.Insert(cacheKey, css);
            }
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
            return _http.Context.Cache;
        }
    }
}