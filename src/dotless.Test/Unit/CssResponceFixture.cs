namespace dotless.Test.Unit
{
    using System.Web;
    using Core.Abstractions;
    using Moq;
    using NUnit.Framework;

    public class CssResponceFixture : HttpFixtureBase
    {
        CssResponse cssResponse;

        [SetUp]
        public void Setup()
        {
            cssResponse = new CssResponse(Http.Object);
        }

        [Test]
        public void ContentTypeIsTextCss()
        {
            cssResponse.WriteCss(null);

            HttpResponse.VerifySet(r => r.ContentType = "text/css", Times.Once());
        }

        [Test]
        public void CssIsWrittenToResponse()
        {
            var str = "testing";

            cssResponse.WriteCss(str);

            HttpResponse.Verify(r => r.Write(str), Times.Once());
        }

        [Test]
        public void SetsCachabilityPublic()
        {
            cssResponse.WriteCss(null);

            HttpCache.Verify(c => c.SetCacheability(HttpCacheability.Public), Times.Once());
        }

        [Test]
        public void ResponseEndIsCalled()
        {
            cssResponse.WriteCss(null);

            HttpResponse.Verify(r => r.End(), Times.Once());
        }
    }
}