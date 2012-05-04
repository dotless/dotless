namespace dotless.Test
{
    using System.Collections.Specialized;
    using System.Web;
    using Core.Abstractions;
    using Moq;
    using NUnit.Framework;
    using System.IO;

    public class HttpFixtureBase
    {
        protected Mock<HttpContextBase> HttpContext { get; set; }
        protected Mock<HttpRequestBase> HttpRequest { get; set; }
        protected Mock<HttpResponseBase> HttpResponse { get; set; }
        protected Mock<HttpSessionStateBase> HttpSession { get; set; }
        protected Mock<HttpServerUtilityBase> HttpServer { get; set; }
        protected Mock<HttpCachePolicyBase> HttpCache { get; set; }
        protected Mock<IHttp> Http { get; set; }
        protected NameValueCollection QueryString { get; set; }
        protected NameValueCollection Form { get; set; }
        protected NameValueCollection Headers { get; set; }

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

            QueryString = new NameValueCollection();
            Form = new NameValueCollection();
            Headers = new NameValueCollection();

            Http.SetupGet(h => h.Context).Returns(HttpContext.Object);

            HttpContext.SetupGet(c => c.Request).Returns(HttpRequest.Object);
            HttpContext.SetupGet(c => c.Response).Returns(HttpResponse.Object);
            HttpContext.SetupGet(c => c.Server).Returns(HttpServer.Object);
            HttpContext.SetupGet(c => c.Session).Returns(HttpSession.Object);
            HttpResponse.SetupGet(r => r.Cache).Returns(HttpCache.Object);
            HttpResponse.SetupGet(r => r.Filter).Returns(new MemoryStream(new byte[1000], true));
            HttpRequest.SetupGet(r => r.QueryString).Returns(QueryString);
            HttpRequest.SetupGet(r => r.Form).Returns(Form);
            HttpRequest.SetupGet(r => r.Headers).Returns(Headers);
        }
    }
}