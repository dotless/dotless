namespace dotless.Core
{
    using Abstractions;
    using Cache;
    using configuration;
    using Input;
    using Loggers;
    using Microsoft.Practices.ServiceLocation;
    using Pandora;
    using Parameters;
    using Response;
    using Stylizers;

    public class ContainerFactory
    {
        private PandoraContainer Container { get; set; }

        public IServiceLocator GetContainer(DotlessConfiguration configuration)
        {
            Container = new PandoraContainer();

            RegisterServices(configuration);

            return new CommonServiceLocatorAdapter(Container);
        }

        private void RegisterServices(DotlessConfiguration configuration)
        {
            Container.Register(p =>
                                   {
                                       if (configuration.Web)
                                       {
                                           p.Service<IHttp>().Implementor<Http>();
                                           p.Service<IParameterSource>().Implementor<QueryStringParameterSource>();

                                           if (configuration.CacheEnabled)
                                               p.Service<IResponse>().Implementor<CachedCssResponse>();
                                           else
                                               p.Service<IResponse>().Implementor<CssResponse>();

                                           p.Service<ICache>().Implementor<HttpCache>();
                                           p.Service<ILogger>().Implementor<AspResponseLogger>().Parameters("level").Set("error-level");
                                           p.Service<IPathResolver>().Implementor<AspServerPathResolver>();
                                       }
                                       else
                                       {
                                           p.Service<IParameterSource>().Implementor<ConsoleArgumentParameterSource>();
                                           p.Service<IParameterSource>().Implementor<ConsoleArgumentParameterSource>();
                                           p.Service<ICache>().Implementor<InMemoryCache>();
                                           p.Service<ILogger>().Implementor<ConsoleLogger>().Parameters("level").Set("error-level");
                                           p.Service<IPathResolver>().Implementor<RelativePathResolver>();
                                       }

                                       p.Service<LogLevel>("error-level").Instance(LogLevel.Error);
                                       p.Service<IStylizer>().Implementor<PlainStylizer>();

                                       p.Service<Parser.Parser>().Implementor<Parser.Parser>().Parameters("optimization").Set("default-optimization");
                                       p.Service<int>("default-optimization").Instance(2);

                                       p.Service<ILessEngine>().Implementor<ParameterDecorator>();

                                       if (configuration.CacheEnabled)
                                            p.Service<ILessEngine>().Implementor<CacheDecorator>();

                                       if (configuration.MinifyOutput)
                                           p.Service<ILessEngine>().Implementor<MinifierDecorator>();

                                       p.Service<ILessEngine>().Implementor<LessEngine>();
                                       p.Service<IFileReader>().Implementor(configuration.LessSource);
                                   });
        }
    }
}