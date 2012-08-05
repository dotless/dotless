namespace dotless.Core.Response
{
    using System;
    using System.Web;
    using Abstractions;

    public class CachedCssResponse : CssResponse
    {
        private const int CacheAgeMinutes = 10080; //7 days

        public CachedCssResponse(IHttp http, bool isCompressionHandledByResponse) : base(http, isCompressionHandledByResponse)
        {
        }

        public override void WriteCss(string css)
        {
            var response = Http.Context.Response;

            response.Cache.SetCacheability(HttpCacheability.Public);
           
            response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(CacheAgeMinutes));
            response.Cache.SetETagFromFileDependencies();
            response.Cache.SetLastModifiedFromFileDependencies();

            //response.Cache.SetOmitVaryStar(true);

            //WriteCss is called multiple times when there is an error in the less file and the AspResponseLogger is being used.
            //SetVaryByCustom("Accept-Encoding") can only be set once, ignore this error when it happens.
            try
            {
                response.Cache.SetVaryByCustom("Accept-Encoding");
            }
            catch { }

            base.WriteCss(css);
        }
    }
}