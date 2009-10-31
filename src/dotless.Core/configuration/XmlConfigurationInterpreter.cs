namespace dotless.Core.configuration
{
    using System.Xml;

    public class XmlConfigurationInterpreter
    {
        public DotlessConfiguration Process(XmlNode section)
        {
            var dotlessConfiguration = DotlessConfiguration.Default;
            //Minify
            XmlAttribute attribute = section.Attributes["minifyCss"];
            if (attribute != null && attribute.Value == "true")
                dotlessConfiguration.MinifyOutput = true;
            //Cache
            XmlAttribute cacheAttribute = section.Attributes["cache"];
            if (cacheAttribute != null && cacheAttribute.Value == "false")
                dotlessConfiguration.CacheEnabled = false;

            return dotlessConfiguration;
        }
    }
}