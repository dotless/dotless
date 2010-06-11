namespace dotless.Test
{
    using System.Web;
    using Core.Abstractions;
    using Moq;
    using NUnit.Framework;

    public class HttpFixtureBase
    {
        protected Mock<HttpContextBase> HttpContext { get; set; }
        protected Mock<HttpRequestBase> HttpRequest { get; set; }
        protected Mock<HttpResponseBase> HttpResponse { get; set; }
        protected Mock<HttpSessionStateBase> HttpSession { get; set; }
        protected Mock<HttpServerUtilityBase> HttpServer { get; set; }
        protected Mock<HttpCachePolicyBase> HttpCache { get; set; }
        protected Mock<IHttp> Http { get; set; }

        [SetUp]
        public void BaseSetup()
        {
            HttpContext = new Mock<HttpContextBase>();
            HttpRequest = new Mock<HttpRequestBase>();
            HttpResponse = new Mock<HttpResponseBase>();
            HttpSession = new Mock<HttpSessionStateBase>();
            HttpServer = new Mock<HttpServerUtilityBase>();
            HttpCache = new Mock<HttpCachePolicyBase>();
            Http = new Mock<IHttp>();

            Http.SetupGet(h => h.Context).Returns(HttpContext.Object);

            HttpContext.SetupGet(c => c.Request).Returns(HttpRequest.Object);
            HttpContext.SetupGet(c => c.Response).Returns(HttpResponse.Object);
            HttpContext.SetupGet(c => c.Server).Returns(HttpServer.Object);
            HttpContext.SetupGet(c => c.Session).Returns(HttpSession.Object);
            HttpResponse.SetupGet(r => r.Cache).Returns(HttpCache.Object);
        }
    }
}