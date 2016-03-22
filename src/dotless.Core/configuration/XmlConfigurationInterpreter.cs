using System.Configuration;

namespace dotless.Core.configuration
{
    using System;
    using System.Xml;
    using Loggers;
    using Plugins;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class XmlConfigurationInterpreter
    {
        public DotlessConfiguration Process(XmlNode section)
        {
            var dotlessConfiguration = DotlessConfiguration.GetDefaultWeb();

            dotlessConfiguration.MinifyOutput = GetBoolValue(section, "minifyCss") ?? dotlessConfiguration.MinifyOutput;
            dotlessConfiguration.Debug = GetBoolValue(section, "debug") ?? dotlessConfiguration.Debug;
            dotlessConfiguration.CacheEnabled = GetBoolValue(section, "cache") ?? dotlessConfiguration.CacheEnabled;
            dotlessConfiguration.HttpExpiryInMinutes = GetIntValue(section, "httpExpiryInMinutes") ?? dotlessConfiguration.HttpExpiryInMinutes;
            dotlessConfiguration.Optimization = GetIntValue(section, "optimization") ?? dotlessConfiguration.Optimization;
            dotlessConfiguration.DisableUrlRewriting = GetBoolValue(section, "disableUrlRewriting") ?? dotlessConfiguration.DisableUrlRewriting;
            dotlessConfiguration.InlineCssFiles = GetBoolValue(section, "inlineCssFiles") ?? dotlessConfiguration.InlineCssFiles;
            dotlessConfiguration.ImportAllFilesAsLess = GetBoolValue(section, "importAllFilesAsLess") ?? dotlessConfiguration.ImportAllFilesAsLess;
            dotlessConfiguration.MapPathsToWeb = GetBoolValue(section, "mapPathsToWeb") ?? dotlessConfiguration.MapPathsToWeb;
            dotlessConfiguration.HandleWebCompression = GetBoolValue(section, "handleWebCompression") ?? dotlessConfiguration.HandleWebCompression;
            dotlessConfiguration.DisableParameters = GetBoolValue(section, "disableParameters") ?? dotlessConfiguration.DisableParameters;
            dotlessConfiguration.KeepFirstSpecialComment = GetBoolValue(section, "keepFirstSpecialComment") ?? dotlessConfiguration.KeepFirstSpecialComment;
            dotlessConfiguration.RootPath = GetStringValue(section, "rootPath") ?? dotlessConfiguration.RootPath;
            dotlessConfiguration.DisableVariableRedefines = GetBoolValue(section, "disableVariableRedefines") ?? dotlessConfiguration.DisableVariableRedefines;
            dotlessConfiguration.StrictMath = GetBoolValue(section, "strictMath") ?? dotlessConfiguration.StrictMath;

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
            }

            var source = GetTypeValue(section, "source");
            if (source != null)
                dotlessConfiguration.LessSource = source;

            dotlessConfiguration.Logger = GetTypeValue(section, "logger");
            dotlessConfiguration.Plugins.AddRange(GetPlugins(section));

            var sessionMode = GetStringValue(section, "sessionMode");
            dotlessConfiguration.SessionMode = string.IsNullOrEmpty(sessionMode)
                                                   ? DotlessSessionStateMode.Disabled
                                                   : (DotlessSessionStateMode) Enum.Parse(typeof (DotlessSessionStateMode), sessionMode, true);
            
            dotlessConfiguration.SessionQueryParamName = GetStringValue(section, "sessionQueryParamName")
                                                         ?? DotlessConfiguration.DEFAULT_SESSION_QUERY_PARAM_NAME;

            if (dotlessConfiguration.SessionMode == DotlessSessionStateMode.QueryParam && string.IsNullOrEmpty(dotlessConfiguration.SessionQueryParamName))
            {
                throw new ConfigurationErrorsException("The 'sessionQueryParamName' should be not empty when sessionMode is set to 'queryParam'", section);
            }

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

        private static IEnumerable<IPluginConfigurator> GetPlugins(XmlNode section)
        {
            List<IPluginConfigurator> plugins = new List<IPluginConfigurator>();
            IEnumerable<IPluginConfigurator> dotlessPlugins = null; //lazy initiate incase of no plugins used
            List<string> assemblies = new List<string>();

            foreach (XmlNode node in section.SelectNodes("plugin"))
            {
                if (dotlessPlugins == null)
                {
                    dotlessPlugins = PluginFinder.GetConfigurators(false);
                }

                string assembly = GetStringValue(node, "assembly");

                if (assembly != null)
                {
                    if (!assemblies.Contains(assembly)) {
                        dotlessPlugins = dotlessPlugins.Union(PluginFinder.GetConfigurators(Assembly.Load(assembly)));
                        assemblies.Add(assembly);
                    }
                }

                string name = GetStringValue(node, "name");
                var plugin = dotlessPlugins.Where(p => p.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault();

                if (plugin == null)
                {
                    throw new Exception(
                        string.Format("Cannot find plugin called {0}. If it is an external plugin, make sure the assembly is referenced.", name));
                }
                var pluginParameters = plugin.GetParameters();

                foreach (XmlNode pluginParameter in node.SelectNodes("pluginParameter"))
                {
                    var pluginParameterName = GetStringValue(pluginParameter, "name");
                    var pluginParameterValue = GetStringValue(pluginParameter, "value");

                    var actualParameter = pluginParameters
                        .Where(p => p.Name.Equals(pluginParameterName, StringComparison.InvariantCultureIgnoreCase))
                        .FirstOrDefault();

                    if (actualParameter == null)
                    {
                        throw new Exception(
                            string.Format("Cannot find plugin argument {0} in plugin {1}", pluginParameterName, name));
                    }

                    actualParameter.SetValue(pluginParameterValue);
                }

                plugin.SetParameterValues(pluginParameters);
                plugins.Add(plugin);
            }
            return plugins;
        }
    }
}