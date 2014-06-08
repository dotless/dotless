using System;

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
            CachedCssResponse = new CachedCssResponse(Http.Object, false, Clock.Object);
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

            HttpCache.Verify(c => c.SetExpires(Now.AddMinutes(CachedCssResponse.DefaultCacheAgeMinutes)), Times.Once());
        }
    }
}