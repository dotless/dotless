namespace dotless.Core.Abstractions
{
    using System.Web;

    public class Http : IHttp
    {
        private HttpContextWrapper _context;
        private HttpContext _c;

        public HttpContextBase Context
        {
            get
            {
                if (_context == null || _c != HttpContext.Current)
                {
                    _context = new HttpContextWrapper(HttpContext.Current);
                    _c = HttpContext.Current;
                }

                return _context;
            }
        }
    }
}