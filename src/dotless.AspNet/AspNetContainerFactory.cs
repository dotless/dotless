namespace dotless.Core
{
    using Pandora.Fluent;
    using Abstractions;
    using Cache;
    using Input;
    using Loggers;
    using Parameters;
    using Response;
    using configuration;

    public class AspNetContainerFactory : ContainerFactory
    {
        protected override void RegisterServices(FluentRegistration pandora, DotlessConfiguration configuration)
        {
            base.RegisterServices(pandora, configuration);
            RegisterWebServices(pandora, configuration);
        }

        private void RegisterWebServices(FluentRegistration pandora, DotlessConfiguration configuration)
        {
            pandora.Service<IParameterSource>().Implementor<NullParameterSource>().Lifestyle.Transient();

            pandora.Service<IHttp>().Implementor<Http>().Lifestyle.Transient();
            pandora.Service<HandlerImpl>().Implementor<HandlerImpl>().Lifestyle.Transient();

            var responseService = configuration.CacheEnabled ?
                pandora.Service<IResponse>().Implementor<CachedCssResponse>() :
                pandora.Service<IResponse>().Implementor<CssResponse>();

            responseService.Parameters("isCompressionHandledByResponse").Set("default-is-compression-handled-by-response").Lifestyle.Transient();
            pandora.Service<bool>("default-is-compression-handled-by-response").Instance(configuration.HandleWebCompression);

            if (configuration.CacheEnabled)
            {
                responseService.Parameters("httpExpiryInMinutes").Set("http-expiry-in-minutes").Lifestyle.Transient();
                pandora.Service<int>("http-expiry-in-minutes").Instance(configuration.HttpExpiryInMinutes);
            }

            pandora.Service<ICache>().Implementor<HttpCache>().Lifestyle.Transient();
            pandora.Service<ILogger>().Implementor<AspResponseLogger>().Parameters("level").Set("error-level").Lifestyle.Transient();

            if (configuration.MapPathsToWeb)
                pandora.Service<IPathResolver>().Implementor<AspServerPathResolver>().Lifestyle.Transient();
            else
                pandora.Service<IPathResolver>().Implementor<AspRelativePathResolver>().Lifestyle.Transient();
        }
    }
}