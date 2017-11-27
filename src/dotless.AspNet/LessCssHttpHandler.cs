namespace dotless.Core
{
    using System.Web;
    using System.Web.SessionState;
    using Microsoft.Extensions.DependencyInjection;
    
    public class LessCssWithSessionHttpHandler : LessCssHttpHandler, IRequiresSessionState
    {
    }

    public class LessCssHttpHandler : LessCssHttpHandlerBase, IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var handler = Container.GetRequiredService<HandlerImpl>();

                handler.Execute();
            }
            catch (System.IO.FileNotFoundException ex)
            {
                context.Response.StatusCode = 404;
                if (context.Request.IsLocal)
                {
                    context.Response.Write("/* File Not Found while parsing: " + ex.Message + " */");
                }
                else
                {
                    context.Response.Write("/* Error Occurred. Consult log or view on local machine. */");
                }
                context.Response.End();
            }
            catch (System.IO.IOException ex)
            {
                context.Response.StatusCode = 500;
                if (context.Request.IsLocal)
                {
                    context.Response.Write("/* Error in less parsing: " + ex.Message + " */");
                }
                else
                {
                    context.Response.Write("/* Error Occurred. Consult log or view on local machine. */");
                }
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
