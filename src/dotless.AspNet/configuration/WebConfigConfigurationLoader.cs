using System;

namespace dotless.Core.configuration
{
    public class WebConfigConfigurationLoader
    {
        internal static IConfigurationManager _configurationManager;

        public DotlessConfiguration GetConfiguration()
        {
            var webconfig = ConfigurationManager.GetSection<DotlessConfiguration>("dotless");

            if (webconfig == null)
                return DotlessConfiguration.GetDefaultWeb();

            webconfig.Web = true;

            return webconfig;
        }

        public static IConfigurationManager ConfigurationManager
        {
            get { return _configurationManager ?? (_configurationManager = new ConfigurationManagerWrapper()); }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _configurationManager = value;
            }
        }
    }
}