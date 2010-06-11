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
    using Response;
    using Stylizers;

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
            if (configuration.Web)
                RegisterWebServices(pandora, configuration);
            else
                RegisterLocalServices(pandora);

            RegisterCoreServices(pandora, configuration);
        }

        private void RegisterWebServices(FluentRegistration pandora, DotlessConfiguration configuration)
        {
            pandora.Service<IHttp>().Implementor<Http>();

            if (configuration.CacheEnabled)
                pandora.Service<IResponse>().Implementor<CachedCssResponse>();
            else
                pandora.Service<IResponse>().Implementor<CssResponse>();

            pandora.Service<ICache>().Implementor<HttpCache>();
            pandora.Service<ILogger>().Implementor<AspResponseLogger>().Parameters("level").Set("error-level");
            pandora.Service<IPathResolver>().Implementor<AspServerPathResolver>();
        }

        private void RegisterLocalServices(FluentRegistration pandora)
        {
            pandora.Service<ICache>().Implementor<InMemoryCache>();
            pandora.Service<ILogger>().Implementor<ConsoleLogger>().Parameters("level").Set("error-level");
            pandora.Service<IPathResolver>().Implementor<RelativePathResolver>();
        }

        private void RegisterCoreServices(FluentRegistration pandora, DotlessConfiguration configuration)
        {
            pandora.Service<LogLevel>("error-level").Instance(LogLevel.Error);
            pandora.Service<IStylizer>().Implementor<PlainStylizer>();

            pandora.Service<Parser.Parser>().Implementor<Parser.Parser>().Parameters("optimization").Set("default-optimization");
            pandora.Service<int>("default-optimization").Instance(1);

            if (configuration.CacheEnabled)
                pandora.Service<ILessEngine>().Implementor<CacheDecorator>();

            if (configuration.MinifyOutput)
                pandora.Service<ILessEngine>().Implementor<MinifierDecorator>();

            pandora.Service<ILessEngine>().Implementor<LessEngine>();
            pandora.Service<IFileReader>().Implementor(configuration.LessSource);
        }
    }
}