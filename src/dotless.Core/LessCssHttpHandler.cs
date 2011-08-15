namespace dotless.Core
{
    using System.IO.Compression;
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
            Container = new ContainerFactory().GetContainer(Config);
        }

        public void ProcessRequest(HttpContext context)
        {
            try {
                string acceptEncoding = (context.Request.Headers["Accept-Encoding"] ?? "").ToUpperInvariant();

                if (acceptEncoding.Contains("GZIP"))
                {
                    context.Response.AppendHeader("Content-Encoding", "gzip");
                    context.Response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress);
                }
                else if (acceptEncoding.Contains("DEFLATE"))
                {
                    context.Response.AppendHeader("Content-Encoding", "deflate");
                    context.Response.Filter = new DeflateStream(context.Response.Filter,
                                                                CompressionMode.Compress);
                }

                var handler = Container.GetInstance<HandlerImpl>();
            
                handler.Execute();

            } catch (System.IO.FileNotFoundException ex){
                context.Response.StatusCode = 404;
                context.Response.Write("/* File Not Found while parsing: "+ex.Message+" */");
                context.Response.End(); 
            } catch (System.IO.IOException ex){
                context.Response.StatusCode = 500;
                context.Response.Write("/* Error in less parsing: "+ex.Message+" */");
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}