using System;
using System.IO;
using System.Web;
using nless.Core.engine;

namespace nless.Core
{
    public class LessCssHttpHandler : IHttpHandler
    {


        public void ProcessRequest(HttpContext context)
        {
            // our unprocessed filename   
            var lessFile = context.Server.MapPath(context.Request.Url.LocalPath);
            var engine = new Engine(File.ReadAllText(lessFile), Console.Out);
            context.Response.ContentType = "text/css";
            context.Response.Write(engine.Parse().Css);
            context.Response.End();
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}