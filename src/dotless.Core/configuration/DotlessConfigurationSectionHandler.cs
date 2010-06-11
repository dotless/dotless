namespace dotless.Core.configuration
{
    using System;
    using System.Configuration;
    using System.Xml;

    public class DotlessConfigurationSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            var configuration = DotlessConfiguration.Default;

            try
            {
                var interpreter = new XmlConfigurationInterpreter();
                configuration = interpreter.Process(section);
            }
            catch (Exception)
            {
                //TODO: Log the errormessage to somewhere
            }

            return configuration;
        }
    }
}