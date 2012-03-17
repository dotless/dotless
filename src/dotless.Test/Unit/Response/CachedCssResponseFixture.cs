namespace dotless.Test.Unit.Response
{
    using Core.Response;
    using Moq;
    using NUnit.Framework;
    using System.Web;

    public class CachedCssResponseFixture : HttpFixtureBase
    {
        CachedCssResponse CachedCssResponse { get; set; }

        [SetUp]
        public void Setup()
        {
            CachedCssResponse = new CachedCssResponse(Http.Object);
        }

        [Test]
        public void ContentTypeIsSetToTextCss()
        {
            CachedCssResponse.WriteCss(null);

            HttpResponse.VerifySet(r => r.ContentType = "text/css", Times.Once());
        }

        [Test]
        public void SetsCachabilityPublic()
        {
            CachedCssResponse.WriteCss(null);

            HttpCache.Verify(c => c.SetCacheability(HttpCacheability.Public), Times.Once());
        }
    }
}