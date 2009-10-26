namespace dotless.Core.configuration
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
            return new CacheConfig() {CacheEnabled = false, CacheExpiration = 0, CacheStragy = "file"};
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
            XmlAttribute strategy = section.Attributes["strategy"];
            if (strategy != null)
            {
                configuration.CacheStragy = GetCacheStrategyName(strategy.Value);
            }
            return configuration;
        }

        private static string GetCacheStrategyName(string attributeValue)
        {
            if (attributeValue == "expiration")
                return attributeValue;
            return "file";
        }
    }
}