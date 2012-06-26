namespace dotless.Core
{
    using System;
    using Microsoft.Practices.ServiceLocation;
    using configuration;

    public abstract class LessCssHttpHandlerBase
    {
        private DotlessConfiguration _config;
        private IServiceLocator _container;

        public DotlessConfiguration Config
        {
            get { return _config ?? (_config = new WebConfigConfigurationLoader().GetConfiguration()); }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _config = value;
            }
        }

        public IServiceLocator Container
        {
            get { return _container ?? (_container = GetContainerFactory().GetContainer(Config)); }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _container = value;
            }
        }

        protected virtual ContainerFactory GetContainerFactory()
        {
            return new AspNetContainerFactory();
        }
    }
}