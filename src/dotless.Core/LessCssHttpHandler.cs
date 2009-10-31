namespace dotless.Core
{
    using System;
    using System.Web;
    using System.Web.Caching;
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

            //context.Response.AddFileDependency(lessFile);
            context.Response.Cache.SetCacheability(HttpCacheability.Public);    //Anyone can cache this

            //TODO: Clean up this code. Seperate concerns here
            context.Response.ContentType = "text/css";
            if (context.Cache[lessFile] == null)
            {
                string css = engine.TransformToCss(lessFile);
                if (config.CacheEnabled)
                    context.Cache.Insert(lessFile, css, new CacheDependency(lessFile));
            }
            context.Response.Write(context.Cache[lessFile]);
            context.Response.End();

            
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}