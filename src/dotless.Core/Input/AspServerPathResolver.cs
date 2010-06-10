namespace dotless.Core
{
    using Abstractions;

    class AspServerPathResolver : IPathResolver
    {
        private readonly IHttp _http;

        public AspServerPathResolver(IHttp http)
        {
            _http = http;
        }

        public string GetFullPath(string path)
        {
            return _http.Context.Server.MapPath(path);
        }
    }
}