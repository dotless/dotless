using System.Web;
using System.Web.Hosting;
using dotless.Core;
using dotless.Core.Abstractions;
using dotless.Core.Cache;
using dotless.Core.configuration;
using dotless.Core.Input;
using dotless.Core.Loggers;
using dotless.Core.Parameters;
using dotless.Core.Response;
using Pandora.Fluent;

namespace dotless.Bundling
{
    public class BundlingContainerFactory : ContainerFactory
    {
        private readonly HttpContextBase _httpContext;
        private readonly VirtualPathProvider _virtualPathProvider;

        public BundlingContainerFactory(HttpContextBase httpContext, VirtualPathProvider virtualPathProvider)
        {
            _httpContext = httpContext;
            _virtualPathProvider = virtualPathProvider;
        }

        protected override void RegisterServices(FluentRegistration pandora, DotlessConfiguration configuration)
        {
            base.RegisterServices(pandora, configuration);
            RegisterWebServices(pandora, configuration);
        }

        private void RegisterWebServices(FluentRegistration pandora, DotlessConfiguration configuration)
        {
            //pandora.Service<IHttp>().Instance(new Http(_httpContext));
            pandora.Service<IFileReader>().Implementor<VirtualFileReader>();
            pandora.Service<VirtualPathProvider>().Instance(_virtualPathProvider);

            //pandora.Service<HandlerImpl>().Implementor<HandlerImpl>().Lifestyle.Transient();

            //if (!configuration.DisableParameters)
            //{
            //    pandora.Service<IParameterSource>().Implementor<QueryStringParameterSource>().Lifestyle.Transient();
            //}

            var responseService = configuration.CacheEnabled ?
                pandora.Service<IResponse>().Implementor<CachedCssResponse>() :
                pandora.Service<IResponse>().Implementor<CssResponse>();

            //responseService.Parameters("isCompressionHandledByResponse").Set("default-is-compression-handled-by-response").Lifestyle.Transient();
            //pandora.Service<bool>("default-is-compression-handled-by-response").Instance(configuration.HandleWebCompression);

            //if (configuration.CacheEnabled)
            //{
            //    responseService.Parameters("httpExpiryInMinutes").Set("http-expiry-in-minutes").Lifestyle.Transient();
            //    pandora.Service<int>("http-expiry-in-minutes").Instance(configuration.HttpExpiryInMinutes);
            //}

            pandora.Service<ICache>().Implementor<InMemoryCache>().Lifestyle.Transient(); // TODO - need to figure out what caching to do
            pandora.Service<ILogger>().Implementor<AspResponseLogger>().Parameters("level").Set("error-level").Lifestyle.Transient(); // todo: does this make sense outside of webforms?

            pandora.Service<IPathResolver>().Implementor<AspServerPathResolver>().Lifestyle.Transient();
        }
    }
}