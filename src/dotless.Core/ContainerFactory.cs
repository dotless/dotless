namespace dotless.Core
{
    using Cache;
    using configuration;
    using Input;
    using Loggers;
    using Microsoft.Practices.ServiceLocation;
    using Pandora;
    using Pandora.Fluent;
    using Parameters;
    using Stylizers;
    using dotless.Core.Plugins;
    using System.Collections.Generic;
    using dotless.Core.Importers;

    public class ContainerFactory
    {
        protected PandoraContainer Container { get; set; }

        public IServiceLocator GetContainer(DotlessConfiguration configuration)
        {
            Container = new PandoraContainer();

            Container.Register(pandora => RegisterServices(pandora, configuration));

            return new CommonServiceLocatorAdapter(Container);
        }

        protected virtual void RegisterServices(FluentRegistration pandora, DotlessConfiguration configuration)
        {
            OverrideServices(pandora, configuration);

            if (!configuration.Web)
                RegisterLocalServices(pandora);

            RegisterCoreServices(pandora, configuration);
        }

        protected virtual void OverrideServices(FluentRegistration pandora, DotlessConfiguration configuration)
        {
            if (configuration.Logger != null)
                pandora.Service<ILogger>().Implementor(configuration.Logger);
        }

        protected virtual void RegisterLocalServices(FluentRegistration pandora)
        {
            pandora.Service<ICache>().Implementor<InMemoryCache>();
            pandora.Service<IParameterSource>().Implementor<ConsoleArgumentParameterSource>();
            pandora.Service<ILogger>().Implementor<ConsoleLogger>().Parameters("level").Set("error-level");
            pandora.Service<IPathResolver>().Implementor<RelativePathResolver>();
        }

        protected virtual void RegisterCoreServices(FluentRegistration pandora, DotlessConfiguration configuration)
        {
            pandora.Service<LogLevel>("error-level").Instance(configuration.LogLevel);
            pandora.Service<IStylizer>().Implementor<PlainStylizer>();

            var importer = pandora.Service<IImporter>().Implementor<Importer>();

            importer.Parameters("inlineCssFiles").Set("default-inline-css-files").Lifestyle.Transient();
            importer.Parameters("disableUrlRewriting").Set("default-disable-url-rewriting").Lifestyle.Transient();
            importer.Parameters("importAllFilesAsLess").Set("default-import-all-files-as-less").Lifestyle.Transient();

            pandora.Service<bool>("default-disable-url-rewriting").Instance(configuration.DisableUrlRewriting);
            pandora.Service<bool>("default-inline-css-files").Instance(configuration.InlineCssFiles);
            pandora.Service<bool>("default-import-all-files-as-less").Instance(configuration.ImportAllFilesAsLess);

            pandora.Service<Parser.Parser>().Implementor<Parser.Parser>().Parameters("optimization").Set("default-optimization").Lifestyle.Transient();
            pandora.Service<int>("default-optimization").Instance(configuration.Optimization);

            if (!configuration.DisableParameters)
                pandora.Service<ILessEngine>().Implementor<ParameterDecorator>().Lifestyle.Transient();

            if (configuration.CacheEnabled)
                pandora.Service<ILessEngine>().Implementor<CacheDecorator>().Lifestyle.Transient();

            pandora.Service<ILessEngine>().Implementor<LessEngine>()
                .Parameters("compress").Set("minify-output")
                .Parameters("keepFirstSpecialComment").Set("keepFirstSpecialComment")
                .Parameters("debug").Set("debug")
                .Parameters("disableVariableRedefines").Set("disableVariableRedefines")
                .Lifestyle.Transient();
            pandora.Service<bool>("minify-output").Instance(configuration.MinifyOutput);
            pandora.Service<bool>("debug").Instance(configuration.Debug);
            pandora.Service<bool>("disableVariableRedefines").Instance(configuration.DisableVariableRedefines);
            pandora.Service<bool>("keepFirstSpecialComment").Instance(configuration.KeepFirstSpecialComment);

            pandora.Service<ILessEngine>().Implementor<LessEngine>().Parameters("plugins").Set("default-plugins").Lifestyle.Transient();
            pandora.Service<IEnumerable<IPluginConfigurator>>("default-plugins").Instance(configuration.Plugins);

            pandora.Service<IFileReader>().Instance(configuration.LessSource);
        }
    }
}