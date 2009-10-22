namespace nless.Core.configuration
{
    using System;
    using System.Xml;

    public class XmlConfigurationInterpreter
    {
        private const long ONE_MINUTE = 3600;
        public DotlessConfiguration Process(XmlNode section)
        {
            XmlAttribute attribute = section.Attributes["minifyCss"];
            var dotlessConfiguration = new DotlessConfiguration();
            if (attribute != null && attribute.Value == "true")
                dotlessConfiguration.MinifyOutput = true;
            dotlessConfiguration.CacheConfiguration = GetDefaultCacheConfig();
            dotlessConfiguration.CacheConfiguration =
                ProcessCacheConfiguration(section, dotlessConfiguration.CacheConfiguration);
            return dotlessConfiguration;
        }

        private CacheConfig GetDefaultCacheConfig()
        {
            return new CacheConfig() {CacheEnabled = false, CacheExpiration = 0, CacheStrategy = new FileWatcherCacheStrategy()};
        }

        private CacheConfig ProcessCacheConfiguration(XmlNode section, CacheConfig configuration)
        {
            XmlAttribute attribute = section.Attributes["cache"];
            if (attribute != null && attribute.Value == "enabled")
            {
                configuration.CacheEnabled = true;
            }
            XmlAttribute expiresAttribute = section.Attributes["expires"];
            if (expiresAttribute != null)
            {
                try
                {
                    configuration.CacheExpiration = Convert.ToInt64(expiresAttribute.Value);
                }
                catch (FormatException)
                {
                    configuration.CacheExpiration = ONE_MINUTE;
                }
            }
            XmlAttribute strategyAttribute = section.Attributes["strategy"];
            if (strategyAttribute != null)
            {
                configuration.CacheStrategy = GetStrategy(strategyAttribute.Value, configuration);
            }
            return configuration;
        }

        private ICacheStrategy GetStrategy(string strategyName, CacheConfig configuration)
        {
            if(strategyName == "filewatcher")
            {
                return new FileWatcherCacheStrategy();
            }
            return new TimeExpiringCacheStrategy(configuration.CacheExpiration);
        }
    }
}