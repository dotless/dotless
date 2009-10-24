namespace nless.Core
{
    using System.Web;
    using configuration;

    public class LessCssHttpHandler : IHttpHandler
    {
        private readonly EngineFactory engineFactory = new EngineFactory();

        public void ProcessRequest(HttpContext context)
        {
            var config = ConfigurationLoader.GetConfigurationFromWebconfig();
            ILessEngine engine = engineFactory.GetEngine(config);

            // our unprocessed filename   
            var lessFile = context.Server.MapPath(context.Request.Url.LocalPath);
            context.Response.ContentType = "text/css";
            string css = engine.TransformToCss(lessFile);
            context.Response.Write(css);
            context.Response.End();
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}