namespace dotless.Core.Response
{
    using System;
    using System.Web;
    using Abstractions;

    public class CachedCssResponse : CssResponse
    {
        private readonly IClock _clock;
        public const int DefaultCacheAgeMinutes = 10080; //7 days

        public CachedCssResponse(IHttp http, bool isCompressionHandledByResponse) :
            this(http, isCompressionHandledByResponse, new Clock())
        {
        }

        public CachedCssResponse(IHttp http, bool isCompressionHandledByResponse, IClock clock) 
            : base(http, isCompressionHandledByResponse)
        {
            _clock = clock;
        }

        public override void WriteHeaders()
        {
            var response = Http.Context.Response;

            response.Cache.SetCacheability(HttpCacheability.Public);

            response.Cache.SetExpires(_clock.GetUtcNow().AddMinutes(DefaultCacheAgeMinutes));
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