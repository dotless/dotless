namespace nless.Core.configuration
{
    using System.Xml;

    public class XmlConfigurationInterpreter
    {
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
            return configuration;
        }
    }
}