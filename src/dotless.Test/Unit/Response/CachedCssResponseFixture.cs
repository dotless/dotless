using dotless.Core.configuration;
using dotless.Core.Response;
using Moq;
using NUnit.Framework;
using System.Web;

namespace dotless.Test.Unit.Response
{
    public class CachedCssResponseFixture : HttpFixtureBase
    {
        CachedCssResponse CachedCssResponse { get; set; }

        [SetUp]
        public void Setup()
        {
            CachedCssResponse = new CachedCssResponse(Http.Object, false, DotlessConfiguration.DefaultHttpExpiryInMinutes, Clock.Object);
        }

        [Test]
        public void ContentTypeIsSetToTextCss()
        {
            CachedCssResponse.WriteHeaders();

            HttpResponse.VerifySet(r => r.ContentType = "text/css", Times.Once());
        }

        [Test]
        public void SetsCachabilityPublic()
        {
            CachedCssResponse.WriteHeaders();
            CachedCssResponse.WriteCss("test1");
            CachedCssResponse.WriteCss("test2");

            HttpCache.Verify(c => c.SetCacheability(HttpCacheability.Public), Times.Once());
        }

        [Test]
        public void ExpiryIsSetToOneWeekByDefault()
        {
            CachedCssResponse.WriteHeaders();

            HttpCache.Verify(c => c.SetExpires(Now.AddMinutes(DotlessConfiguration.DefaultHttpExpiryInMinutes)), Times.Once());
        }

        [Test]
        public void CustomExpiryCanBeSetThroughConfiguration()
        {
            const int expiryInMinutes = 5;
            CachedCssResponse = new CachedCssResponse(Http.Object, false, expiryInMinutes, Clock.Object);

            CachedCssResponse.WriteHeaders();

            HttpCache.Verify(c => c.SetExpires(Now.AddMinutes(expiryInMinutes)), Times.Once());
        }
    }
}