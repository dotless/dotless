namespace dotless.Core
{
    using Cache;
    using configuration;
    using Input;
    using Loggers;
    using Microsoft.Extensions.DependencyInjection;
    using Parameters;
    using Stylizers;
    using dotless.Core.Plugins;
    using System.Collections.Generic;
    using dotless.Core.Importers;

    public class ContainerFactory
    {
        protected IServiceCollection Container { get; set; }

        public System.IServiceProvider GetContainer(DotlessConfiguration configuration)
        {  
            var factory = new DefaultServiceProviderFactory();
            var builder = factory.CreateBuilder(null);
            
            RegisterServices(builder, configuration);

            return factory.CreateServiceProvider(builder);
        }

        protected virtual void RegisterServices(IServiceCollection services, DotlessConfiguration configuration)
        {
            OverrideServices(services, configuration);

            if (!configuration.Web)
                RegisterLocalServices(services);

            RegisterCoreServices(services, configuration);
        }

        protected virtual void OverrideServices(IServiceCollection services, DotlessConfiguration configuration)
        {
            if (configuration.Logger != null)
                services.AddSingleton(typeof(ILogger), configuration.Logger);
        }

        protected virtual void RegisterLocalServices(IServiceCollection services)
        {
            services.AddSingleton<ICache, InMemoryCache>();
            services.AddSingleton<IParameterSource, ConsoleArgumentParameterSource>();
            services.AddSingleton<ILogger, ConsoleLogger>();
            services.AddSingleton<IPathResolver, RelativePathResolver>();
        }

        protected virtual void RegisterCoreServices(IServiceCollection services, DotlessConfiguration configuration)
        {
            services.AddSingleton(configuration);
            services.AddSingleton<IStylizer, PlainStylizer>();

            services.AddSingleton<IImporter, Importer>();
            services.AddSingleton<Parser.Parser>();

            if (!configuration.DisableParameters)
                services.AddTransient<ILessEngine, ParameterDecorator>();

            if (configuration.CacheEnabled)
                services.AddTransient<ILessEngine, CacheDecorator>();

            services.AddTransient<ILessEngine, LessEngine>();
            services.AddSingleton<IEnumerable<IPluginConfigurator>>(configuration.Plugins);
            services.AddSingleton(typeof(IFileReader), configuration.LessSource);
        }
    }
}