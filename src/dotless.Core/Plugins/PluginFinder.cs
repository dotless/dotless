namespace dotless.Core.Plugins
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Reflection;
    using System.IO;
    using System.ComponentModel;

    public static class PluginFinder
    {
        /// <summary>
        ///  Gets a plugins name
        /// </summary>
        public static string GetName(this IPlugin plugin)
        {
            return GetName(plugin.GetType());
        }

        /// <summary>
        ///  Gets a plugins description
        /// </summary>
        public static string GetDescription(this IPlugin plugin)
        {
            return GetName(plugin.GetType());
        }

        /// <summary>
        ///  Gets a plugins description from its type
        /// </summary>
        public static string GetDescription(Type pluginType)
        {
            DescriptionAttribute description = pluginType
                .GetCustomAttributes(typeof(DescriptionAttribute), true)
                .FirstOrDefault() as DescriptionAttribute;

            if (description != null)
                return description.Description;
            else
                return "No Description";
        }

        /// <summary>
        ///  Gets a plugins name from its type
        /// </summary>
        public static string GetName(Type pluginType)
        {
            DisplayNameAttribute name = pluginType
                .GetCustomAttributes(typeof(DisplayNameAttribute), true)
                .FirstOrDefault() as DisplayNameAttribute;

            if (name != null)
                return name.DisplayName;
            else
                return pluginType.Name;
        }

        /// <summary>
        ///  Gets plugin configurators for all plugins, optionally scanning referenced assemblies and
        ///  a plugins folder underneath the executing assembly
        /// </summary>
        /// <param name="scanPluginsFolder">Look for a plugins folder and if exists, load plugins from it</param>
        /// <param name="scanLoadedDlls">Whether to look at referenced dll's and scan them for plugins</param>
        /// <returns></returns>
        public static IEnumerable<IPluginConfigurator> GetConfigurators(bool scanPluginsFolder, bool scanLoadedDlls)
        {
            List<IEnumerable<IPluginConfigurator>> pluginConfigurators = new List<IEnumerable<IPluginConfigurator>>();

            if (scanLoadedDlls) 
            {
                pluginConfigurators.AddRange(Assembly.GetEntryAssembly().GetReferencedAssemblies()
                    .Select(assembly => GetConfigurators(Assembly.Load(assembly))));
            }

            pluginConfigurators.Add(GetConfigurators(Assembly.GetAssembly(typeof(PluginFinder))));

            if (scanPluginsFolder)
            {
                string pluginsFolder = Path.Combine(Assembly.GetEntryAssembly().Location, "plugins");

                if (Directory.Exists(pluginsFolder))
                {
                    DirectoryInfo pluginsFolderDirectoryInfo = new DirectoryInfo(pluginsFolder);
                    foreach(FileInfo pluginAssembly in pluginsFolderDirectoryInfo.GetFiles("*.dll"))
                    {
                        pluginConfigurators.Add(GetConfigurators(Assembly.LoadFile(pluginAssembly.FullName)));
                    }
                }
            }

            return pluginConfigurators.Aggregate((group1, group2) => group1.Union(group2));
        }

        /// <summary>
        ///  Gets plugin configurators for every plugin in an assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<IPluginConfigurator> GetConfigurators(Assembly assembly)
        {
            IEnumerable<Type> types = assembly.GetTypes().Where(
                type => !type.IsAbstract && !type.IsGenericType && !type.IsInterface);

            IEnumerable<IPluginConfigurator> pluginConfigurators = types
                .Where(type => typeof(IPluginConfigurator).IsAssignableFrom(type))
                .Select(type => (IPluginConfigurator)type.GetConstructor(new Type[] {}).Invoke(new object[]{}));

            IEnumerable<Type> pluginsConfigurated = pluginConfigurators.Select(pluginConfigurator => pluginConfigurator.Configurates);

            Type genericPluginConfiguratorType = typeof(GenericPluginConfigurator<IPlugin>).GetGenericTypeDefinition();

            IEnumerable<IPluginConfigurator> plugins = types
                .Where(type => typeof(IPlugin).IsAssignableFrom(type))
                .Where(type => !pluginsConfigurated.Contains(type))
                .Select(type => (IPluginConfigurator)genericPluginConfiguratorType.MakeGenericType(type).GetConstructor(new Type[]{}).Invoke(new object[]{}));

            return plugins.Union(pluginConfigurators);
        }
    }
}
