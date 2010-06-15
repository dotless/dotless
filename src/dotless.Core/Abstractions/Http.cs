namespace dotless.Core.Abstractions
{
    using System.Web;

    public class Http : IHttp
    {
        private HttpContextWrapper _context;

        public HttpContextBase Context
        {
            get
            {
                if (_context == null)
                    _context = new HttpContextWrapper(HttpContext.Current);

                return _context;
            }
        }
    }
}