namespace dotless.Core
{
    using System.Web;

    class AspServerPathResolver : IPathResolver
    {
        private readonly HttpContextBase _context;

        public AspServerPathResolver(HttpContextBase context)
        {
            _context = context;
        }

        public string GetFullPath(string path)
        {
            return _context.Server.MapPath(path);
        }
    }
}