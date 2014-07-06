using dotless.Core.Abstractions;

namespace dotless.Core.Input
{
    public class AspServerPathResolver : IPathResolver
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