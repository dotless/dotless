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
                var handler = Container.GetInstance<HandlerImpl>();
                handler.Execute();            
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
