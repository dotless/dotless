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

            return dotlessConfiguration;
        }
    }
}