namespace dotless.Core.Abstractions
{
    using System.Web;

    public class CssResponse : IResponse
    {
        private readonly HttpContextBase _context;

        public CssResponse(HttpContextBase context)
        {
            _context = context;
        }

        public void WriteCss(string css)
        {
            var response = _context.Response;
            response.Cache.SetCacheability(HttpCacheability.Public);
            response.ContentType = "text/css";
            response.Write(css);
            response.End();
        }
    }
}