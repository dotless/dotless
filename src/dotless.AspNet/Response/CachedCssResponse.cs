namespace dotless.Core.Response
{
    using System;
    using System.Web;
    using Abstractions;

    public class CachedCssResponse : CssResponse
    {
        private readonly int _cacheAgeInMinutes;
        private readonly IClock _clock;
        public const int DefaultCacheAgeInMinutes = 10080; //7 days

        public CachedCssResponse(IHttp http, bool isCompressionHandledByResponse, int cacheAgeInMinutes) :
            this(http, isCompressionHandledByResponse, cacheAgeInMinutes, new Clock())
        {
        }

        public CachedCssResponse(IHttp http, bool isCompressionHandledByResponse, int cacheAgeInMinutes, IClock clock) 
            : base(http, isCompressionHandledByResponse)
        {
            _cacheAgeInMinutes = cacheAgeInMinutes;
            _clock = clock;
        }

        public override void WriteHeaders()
        {
            var response = Http.Context.Response;

            response.Cache.SetCacheability(HttpCacheability.Public);

            response.Cache.SetExpires(_clock.GetUtcNow().AddMinutes(_cacheAgeInMinutes));
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