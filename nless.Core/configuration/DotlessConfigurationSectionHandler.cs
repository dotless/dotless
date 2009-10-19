namespace nless.Core.configuration
{
    using System.Configuration;
    using System.Xml;

    public class DotlessConfigurationSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            var interpreter = new XmlConfigurationInterpreter();
            DotlessConfiguration configuration = interpreter.Process(section);
            return configuration;
        }
    }
}