namespace dotless.Core
{
    using Abstractions;
    using Cache;
    using configuration;
    using Input;
    using Loggers;
    using Microsoft.Practices.ServiceLocation;
    using Pandora;
    using Pandora.Fluent;
    using Parameters;
    using Response;
    using Stylizers;
    using dotless.Core.Plugins;
    using System.Collections.Generic;
    using dotless.Core.Importers;

    public class ContainerFactory
    {
        private PandoraContainer Container { get; set; }

        public IServiceLocator GetContainer(DotlessConfiguration configuration)
        {
            Container = new PandoraContainer();

            Container.Register(pandora => RegisterServices(pandora, configuration));

            return new CommonServiceLocatorAdapter(Container);
        }

        private void RegisterServices(FluentRegistration pandora, DotlessConfiguration configuration)
        {
            OverrideServices(pandora, configuration);

            if (configuration.Web)
                RegisterWebServices(pandora, configuration);
            else
                RegisterLocalServices(pandora);

            RegisterCoreServices(pandora, configuration);
        }

        private void OverrideServices(FluentRegistration pandora, DotlessConfiguration configuration)
        {
            if (configuration.Logger != null)
                pandora.Service<ILogger>().Implementor(configuration.Logger);
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
            pandora.Service<IPathResolver>().Implementor<AspServerPathResolver>().Lifestyle.Transient();
        }

        private void RegisterLocalServices(FluentRegistration pandora)
        {
            pandora.Service<ICache>().Implementor<InMemoryCache>();
            pandora.Service<IParameterSource>().Implementor<ConsoleArgumentParameterSource>();
            pandora.Service<ILogger>().Implementor<ConsoleLogger>().Parameters("level").Set("error-level");
            pandora.Service<IPathResolver>().Implementor<RelativePathResolver>();
        }

        private void RegisterCoreServices(FluentRegistration pandora, DotlessConfiguration configuration)
        {
            pandora.Service<LogLevel>("error-level").Instance(configuration.LogLevel);
            pandora.Service<IStylizer>().Implementor<PlainStylizer>();

            pandora.Service<IImporter>().Implementor<Importer>();

            pandora.Service<Parser.Parser>().Implementor<Parser.Parser>().Parameters("optimization").Set("default-optimization").Lifestyle.Transient();
            pandora.Service<int>("default-optimization").Instance(configuration.Optimization);

            pandora.Service<ILessEngine>().Implementor<ParameterDecorator>().Lifestyle.Transient();

            if (configuration.CacheEnabled)
                pandora.Service<ILessEngine>().Implementor<CacheDecorator>().Lifestyle.Transient();

            pandora.Service<ILessEngine>().Implementor<LessEngine>().Parameters("compress").Set("minify-output").Lifestyle.Transient();
            pandora.Service<bool>("minify-output").Instance(configuration.MinifyOutput);

            pandora.Service<ILessEngine>().Implementor<LessEngine>().Parameters("plugins").Set("default-plugins").Lifestyle.Transient();
            pandora.Service<IEnumerable<IPluginConfigurator>>("default-plugins").Instance(configuration.Plugins);

            pandora.Service<IFileReader>().Implementor(configuration.LessSource);
        }
    }
}