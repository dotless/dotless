namespace dotless.Core
{
    using System.IO.Compression;
    using System.Web;
    using configuration;
    using Microsoft.Practices.ServiceLocation;
    using dotless.Core.Response;

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
                /// load encodings from header
                QValueList encodings = new QValueList(context.Request.Headers["Accept-Encoding"]);

                /// get the types we can handle, can be accepted and
                /// in the defined client preference
                QValue preferred = encodings.FindPreferred("gzip", "deflate", "identity");

                /// if none of the preferred values were found, but the
                /// client can accept wildcard encodings, we'll default
                /// to Gzip.
                if (preferred.IsEmpty && encodings.AcceptWildcard && encodings.Find("gzip").IsEmpty)
                {
                    preferred = new QValue("gzip");
                }

                // handle the preferred encoding
                switch (preferred.Name.ToLowerInvariant())
                {
                    case "gzip":
                        context.Response.AppendHeader("Content-Encoding", "gzip");
                        context.Response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress);
                        break;
                    case "deflate":
                        context.Response.AppendHeader("Content-Encoding", "deflate");
                        context.Response.Filter = new DeflateStream(context.Response.Filter, CompressionMode.Compress);
                        break;
                    case "identity":
                    default:
                        break;
                }

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