namespace dotless.Core
{
    using System.Web;
    using System.Web.SessionState;
    
    public class LessCssWithSessionHttpHandler : LessCssHttpHandler, IRequiresSessionState
    {
    }

    public class LessCssHttpHandler : LessCssHttpHandlerBase, IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var handler = Container.GetInstance<HandlerImpl>();

                handler.Execute();
            }
            catch (System.IO.FileNotFoundException ex)
            {
                context.Response.StatusCode = 404;
                context.Response.Write("/* File Not Found while parsing: " + ex.Message + " */");
                context.Response.End();
            }
            catch (System.IO.IOException ex)
            {
                context.Response.StatusCode = 500;
                context.Response.Write("/* Error in less parsing: " + ex.Message + " */");
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
