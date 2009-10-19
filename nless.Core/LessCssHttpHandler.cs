namespace nless.Core
{
    using System;   
    using System.Configuration;
    using System.IO;
    using System.Text;
    using System.Web;
    using configuration;
    using engine;
    using minifier;

    public class LessCssHttpHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var config = ConfigurationLoader.GetConfigurationFromWebconfig();
            // our unprocessed filename   

            var lessFile = context.Server.MapPath(context.Request.Url.LocalPath);
            var engine = new Engine(File.ReadAllText(lessFile), Console.Out);
            context.Response.ContentType = "text/css";
            string buffer = engine.Parse().Css;
            if (config.MinifyOutput)
            {
                var processor = new Processor(buffer);
                buffer = new StringBuilder().Append(processor.Output).ToString();
            }
            context.Response.Write(buffer);
            context.Response.End();
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}