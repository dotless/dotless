namespace dotless.Core.Response
{
    using System;
    using System.Web;
    using Abstractions;

    public class CachedCssResponse : CssResponse
    {
        private readonly int _httpExpiryInMinutes;
        private readonly IClock _clock;

        public CachedCssResponse(IHttp http, bool isCompressionHandledByResponse, int httpExpiryInMinutes) :
            this(http, isCompressionHandledByResponse, httpExpiryInMinutes, new Clock())
        {
        }

        public CachedCssResponse(IHttp http, bool isCompressionHandledByResponse, int httpExpiryInMinutes, IClock clock) 
            : base(http, isCompressionHandledByResponse)
        {
            _httpExpiryInMinutes = httpExpiryInMinutes;
            _clock = clock;
        }

        public override void WriteHeaders()
        {
            var response = Http.Context.Response;

            response.Cache.SetCacheability(HttpCacheability.Public);

            response.Cache.SetExpires(_clock.GetUtcNow().AddMinutes(_httpExpiryInMinutes));
            response.Cache.SetETagFromFileDependencies();
            response.Cache.SetLastModifiedFromFileDependencies();

            // only modify the vary header if we are modifying the encoding
            if (IsCompressionHandledByResponse)
            {
                // response.Cache.SetOmitVaryStar(true);
                response.Cache.SetVaryByCustom("Accept-Encoding");
            }

            base.WriteHeaders();
        }
    }
}