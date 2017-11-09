namespace dotless.Core
{
    using System;
    using System.Web;
    using Abstractions;
    using configuration;
    using Microsoft.Extensions.DependencyInjection;
    
    public class LessCssHttpHandlerFactory : LessCssHttpHandlerBase, IHttpHandlerFactory
    {
        public IHttpHandler GetHandler()
        {
            bool sessionRequired;
            switch (Config.SessionMode)
            {
                case DotlessSessionStateMode.Enabled:
                    sessionRequired = true;
                    break;
                case DotlessSessionStateMode.QueryParam:
                    var http = Container.GetRequiredService<IHttp>();
                    var paramValue = http.Context.Request.QueryString[Config.SessionQueryParamName];
                    sessionRequired = !string.IsNullOrEmpty(paramValue) &&
                                      paramValue != "0" && !paramValue.Equals("false", StringComparison.OrdinalIgnoreCase);
                    break;
                default:
                    sessionRequired = false;
                    break;
            }
            return sessionRequired
                       ? new LessCssWithSessionHttpHandler()
                       : new LessCssHttpHandler();
        }

        IHttpHandler IHttpHandlerFactory.GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            return GetHandler();
        }

        void IHttpHandlerFactory.ReleaseHandler(IHttpHandler handler)
        {
        }
    }
}
