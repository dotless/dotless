namespace dotless.Core.Abstractions
{
    using System.Web;

    public class Http : IHttp
    {
        private HttpContextBase _context;

        public Http()
        {
            if (HttpContext.Current != null)
                _context = new HttpContextWrapper(HttpContext.Current);
        }

        public Http(HttpContextBase context)
        {
            _context = context;
        }

        public HttpContextBase Context
        {
            get { return _context; }
        }
    }
}