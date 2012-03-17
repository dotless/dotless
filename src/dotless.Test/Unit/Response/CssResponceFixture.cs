namespace dotless.Test.Unit.Response
{
    using System.Web;
    using Core.Response;
    using Moq;
    using NUnit.Framework;

    public class CssResponceFixture : HttpFixtureBase
    {
        CssResponse CssResponse { get; set; }

        [SetUp]
        public void Setup()
        {
            CssResponse = new CssResponse(Http.Object);
        }

        [Test]
        public void ContentTypeIsTextCss()
        {
            CssResponse.WriteCss(null);

            HttpResponse.VerifySet(r => r.ContentType = "text/css", Times.Once());
        }

        [Test]
        public void CssIsWrittenToResponse()
        {
            var str = "testing";

            CssResponse.WriteCss(str);

            HttpResponse.Verify(r => r.Write(str), Times.Once());
        }
    }
}