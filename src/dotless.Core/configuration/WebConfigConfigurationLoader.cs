namespace dotless.Core.configuration
{
    public class WebConfigConfigurationLoader
    {
        public DotlessConfiguration GetConfiguration()
        {
            var webconfig = DotlessConfiguration.ConfigurationManager.GetSection<DotlessConfiguration>("dotless");
            
            if (webconfig == null)
                return DotlessConfiguration.GetDefaultWeb();

            webconfig.Web = true;

            return webconfig;
        }
    }
}