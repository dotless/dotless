namespace dotless.Core
{
    using Microsoft.Extensions.DependencyInjection;
    using Abstractions;
    using Cache;
    using Input;
    using Loggers;
    using Parameters;
    using Response;
    using configuration;

    public class AspNetContainerFactory : ContainerFactory
    {
        protected override void RegisterServices(IServiceCollection services, DotlessConfiguration configuration)
        {
            base.RegisterServices(services, configuration);

            RegisterParameterSource(services, configuration);

            RegisterWebServices(services, configuration);
        }

        protected virtual void RegisterParameterSource(IServiceCollection services, DotlessConfiguration configuration)
        {
            services.AddTransient<IParameterSource, NullParameterSource>();
        }

        private void RegisterWebServices(IServiceCollection services, DotlessConfiguration configuration)
        {

            services.AddTransient<IClock, Clock>();
            services.AddTransient<IHttp, Http>();
            services.AddTransient<HandlerImpl>();

            if (configuration.CacheEnabled)
                services.AddSingleton<IResponse, CachedCssResponse>();
            else
                services.AddSingleton<IResponse, CssResponse>();


            services.AddTransient<ICache, HttpCache>();
            services.AddTransient<ILogger, AspResponseLogger>();

            if (configuration.MapPathsToWeb)
                services.AddTransient<IPathResolver, AspServerPathResolver>();
            else
                services.AddTransient<IPathResolver, AspRelativePathResolver>();
        }
    }
}
