using System.Web;
using System.Web.Hosting;
using dotless.Core;
using dotless.Core.Abstractions;
using dotless.Core.Cache;
using dotless.Core.configuration;
using dotless.Core.Input;
using dotless.Core.Loggers;
using Pandora.Fluent;

namespace dotless.Bundling
{
    class BundlingContainerFactory : ContainerFactory
    {
        private readonly ILogger _logger;
        private readonly HttpContextBase _context;
        private readonly VirtualPathProvider _virtualPathProvider;

        public BundlingContainerFactory(HttpContextBase context, ILogger logger, VirtualPathProvider virtualPathProvider)
        {
            _context = context;
            _logger = logger;
            _virtualPathProvider = virtualPathProvider;
        }

        protected override void RegisterServices(FluentRegistration pandora, DotlessConfiguration configuration)
        {
            base.RegisterServices(pandora, configuration);
            RegisterWebServices(pandora);
        }

        private void RegisterWebServices(FluentRegistration pandora)
        {
            pandora.Service<HttpContextBase>().Instance(_context);
            pandora.Service<IHttp>().Implementor<Http>();
            pandora.Service<IFileReader>().Implementor<VirtualFileReader>();
            pandora.Service<VirtualPathProvider>().Instance(_virtualPathProvider);

            //if (!configuration.DisableParameters)
            //{
            //    pandora.Service<IParameterSource>().Implementor<QueryStringParameterSource>().Lifestyle.Transient();
            //}

            pandora.Service<ICache>().Implementor<HttpCache>().Lifestyle.Transient();
            pandora.Service<ILogger>().Instance(_logger);

            pandora.Service<IPathResolver>().Implementor<AspServerPathResolver>().Lifestyle.Transient();
        }
    }
}