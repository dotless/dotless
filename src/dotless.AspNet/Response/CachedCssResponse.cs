namespace dotless.Core.Response
{
    using System;
    using System.Web;
    using Abstractions;

    public class CachedCssResponse : IResponse
    {
        public readonly IHttp Http;
        private const int CacheAgeMinutes = 10080; //7 days

        public CachedCssResponse(IHttp http)
        {
            Http = http;
        }

        public void WriteCss(string css)
        {
            var response = Http.Context.Response;

            response.Cache.SetCacheability(HttpCacheability.Public);
           
            response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(CacheAgeMinutes));
            response.Cache.SetETagFromFileDependencies();
            response.Cache.SetLastModifiedFromFileDependencies();

            //response.Cache.SetOmitVaryStar(true);
            response.Cache.SetVaryByCustom("Accept-Encoding");

            response.ContentType = "text/css";
            response.Write(css);

        }
    }
}