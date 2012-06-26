namespace dotless.Test.Unit
{
    using NUnit.Framework;
    using Core;
    using Core.configuration;

    public class HttpHandlerFixture : HttpFixtureBase
    {
        [Test]
        public void HttpHandlerRequiresSession()
        {
            Config.SessionMode = DotlessSessionStateMode.Enabled;
            CheckHttpHandlerType(true);

            Config.SessionMode = DotlessSessionStateMode.Disabled;
            CheckHttpHandlerType(false);
        }

        [Test]
        [Ignore("Handler factory looks in IHtml.Context.Request.QueryString. Cannot mock IHtml implementor.")]
        public void SessionParam()
        {
            Config.SessionMode = DotlessSessionStateMode.QueryParam;
            CheckHttpHandlerType(false);
            QueryString[DotlessConfiguration.DEFAULT_SESSION_QUERY_PARAM_NAME] = "0";
            CheckHttpHandlerType(false);
            QueryString[DotlessConfiguration.DEFAULT_SESSION_QUERY_PARAM_NAME] = "false";
            CheckHttpHandlerType(true);
            QueryString[DotlessConfiguration.DEFAULT_SESSION_QUERY_PARAM_NAME] = "1";
            CheckHttpHandlerType(true);
            QueryString[DotlessConfiguration.DEFAULT_SESSION_QUERY_PARAM_NAME] = "true";
            CheckHttpHandlerType(true);
        }

        private static void CheckHttpHandlerType(bool expectedIsSessionAware)
        {
            var hdl = new LessCssHttpHandlerFactory().GetHandler(System.Web.HttpContext.Current, "GET", "file.less", @"c:\www\file.less");
            Assert.That(hdl, expectedIsSessionAware ? Is.TypeOf<LessCssWithSessionHttpHandler>() : Is.TypeOf<LessCssHttpHandler>());
        }
    }
}
