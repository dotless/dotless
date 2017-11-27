namespace dotless.Core
{
    using System;
 
    using configuration;

    public abstract class LessCssHttpHandlerBase
    {
        private DotlessConfiguration _config;
        private IServiceProvider _container;

        public DotlessConfiguration Config
        {
            get { return _config ?? (_config = new WebConfigConfigurationLoader().GetConfiguration()); }
            set
            {
                _config = value ?? throw new ArgumentNullException("value");
            }
        }

        public IServiceProvider Container
        {
            get { return _container ?? (_container = GetContainerFactory().GetContainer(Config)); }
            set
            {
                _container = value ?? throw new ArgumentNullException("value");
            }
        }

        protected virtual ContainerFactory GetContainerFactory()
        {
            return new AspNetHttpHandlerContainerFactory();
        }
    }
}