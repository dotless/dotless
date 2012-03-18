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
            pandora.Service<IHttp>().Implementor<Http>().Lifestyle.Transient();
            pandora.Service<HandlerImpl>().Implementor<HandlerImpl>().Lifestyle.Transient();
            pandora.Service<IParameterSource>().Implementor<QueryStringParameterSource>().Lifestyle.Transient();

            if (configuration.CacheEnabled)
                pandora.Service<IResponse>().Implementor<CachedCssResponse>().Lifestyle.Transient();
            else
                pandora.Service<IResponse>().Implementor<CssResponse>().Lifestyle.Transient();

            pandora.Service<ICache>().Implementor<HttpCache>().Lifestyle.Transient();
            pandora.Service<ILogger>().Implementor<AspResponseLogger>().Parameters("level").Set("error-level").Lifestyle.Transient();

            if (configuration.MapPathsToWeb)
                pandora.Service<IPathResolver>().Implementor<AspServerPathResolver>().Lifestyle.Transient();
            else
                pandora.Service<IPathResolver>().Implementor<AspRelativePathResolver>().Lifestyle.Transient();
        }
    }
}