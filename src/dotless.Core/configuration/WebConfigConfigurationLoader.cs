namespace dotless.Core.configuration
{
    using System.Configuration;

    public class WebConfigConfigurationLoader
    {
        public DotlessConfiguration GetConfiguration()
        {
            var webconfig = (DotlessConfiguration)ConfigurationManager.GetSection("dotless");
            
            if (webconfig == null)
                return DotlessConfiguration.GetDefaultWeb();

            webconfig.Web = true;

            return webconfig;
        }
    }
}