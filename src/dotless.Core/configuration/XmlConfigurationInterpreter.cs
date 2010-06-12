namespace dotless.Core.configuration
{
    using System;
    using System.Xml;
    using Loggers;

    public class XmlConfigurationInterpreter
    {
        public DotlessConfiguration Process(XmlNode section)
        {
            var dotlessConfiguration = DotlessConfiguration.DefaultWeb;

            dotlessConfiguration.MinifyOutput = GetBoolValue(section, "minifyCss") ?? dotlessConfiguration.MinifyOutput;
            dotlessConfiguration.CacheEnabled = GetBoolValue(section, "cache") ?? dotlessConfiguration.CacheEnabled;
            dotlessConfiguration.Optimization = GetIntValue(section, "optimization") ?? dotlessConfiguration.Optimization;

            var logLevel = GetStringValue(section, "log") ?? "default";
            switch (logLevel.ToLowerInvariant())
            {
                case "info":
                    dotlessConfiguration.LogLevel = LogLevel.Info;
                    break;
                case "debug":
                    dotlessConfiguration.LogLevel = LogLevel.Debug;
                    break;
                case "warn":
                    dotlessConfiguration.LogLevel = LogLevel.Warn;
                    break;
                case "error":
                    dotlessConfiguration.LogLevel = LogLevel.Error;
                    break;
                case "default":
                    break;
                default: 
                    break;
            }

            var source = GetTypeValue(section, "source");
            if (source != null)
                dotlessConfiguration.LessSource = source;

            dotlessConfiguration.Logger = GetTypeValue(section, "logger");

            return dotlessConfiguration;
        }

        private static string GetStringValue(XmlNode section, string property)
        {
            var attribute = section.Attributes[property];

            if (attribute == null)
                return null;

            return attribute.Value;
        }

        private static int? GetIntValue(XmlNode section, string property)
        {
            var attribute = section.Attributes[property];

            if (attribute == null)
                return null;

            int result;
            if (int.TryParse(attribute.Value, out result))
                return result;

            return null;
        }

        private static bool? GetBoolValue(XmlNode section, string property)
        {
            var attribute = section.Attributes[property];

            if (attribute == null)
                return null;

            bool result;
            if (bool.TryParse(attribute.Value, out result))
                return result;

            return null;
        }

        private static Type GetTypeValue(XmlNode section, string property)
        {
            var attribute = section.Attributes[property];

            if (attribute == null)
                return null;

            var value = attribute.Value;

            if (!string.IsNullOrEmpty(value))
                return Type.GetType(value);

            return null;
        }
    }
}