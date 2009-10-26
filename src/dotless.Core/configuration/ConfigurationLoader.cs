namespace dotless.Core.configuration
{
    using System.Configuration;

    public class ConfigurationLoader
    {
        public static DotlessConfiguration GetConfigurationFromWebconfig()
        {
            var webconfig = (DotlessConfiguration)ConfigurationManager.GetSection("dotless");
            if (webconfig == null)
                return new DotlessConfiguration();
            return webconfig;
        }
    }
}