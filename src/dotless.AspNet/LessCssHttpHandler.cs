namespace dotless.Core
{
    using System.Web;
    using configuration;
    using Microsoft.Practices.ServiceLocation;

    public class LessCssHttpHandler : IHttpHandler
    {
        public IServiceLocator Container { get; set; }
        public DotlessConfiguration Config { get; set; }

        public LessCssHttpHandler()
        {
            Config = new WebConfigConfigurationLoader().GetConfiguration();
            Container = GetContainerFactory().GetContainer(Config);
        }

        protected virtual ContainerFactory GetContainerFactory()
        {
            return new AspNetContainerFactory();
        }

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