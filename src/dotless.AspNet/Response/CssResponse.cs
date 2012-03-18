namespace dotless.Core.Response
{
    using Abstractions;

    public class CssResponse : IResponse
    {
        public readonly IHttp Http;

        public CssResponse(IHttp http)
        {
            Http = http;
        }

        public void WriteCss(string css)
        {
            var response = Http.Context.Response;
            response.ContentType = "text/css";
            response.Write(css);
        }
    }
}