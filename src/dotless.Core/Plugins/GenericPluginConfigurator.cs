namespace dotless.Core.Plugins
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Reflection;
    using System.ComponentModel;

    public class GenericPluginConfigurator<T> : IPluginConfigurator where T : IPlugin
    {
        public string Name
        {
            get
            {
                return PluginFinder.GetName(typeof(T));
            }
        }

        public string Description
        {
            get
            {
                return PluginFinder.GetDescription(typeof(T));
            }
        }

        public Type Configurates
        {
            get
            {
                return typeof(T);
            }
        }

        private Func<IPlugin> _pluginCreator = null;
        public void SetParameterValues(IEnumerable<IPluginParameter> pluginParameters)
        {
            ConstructorInfo defaultConstructor;
            ConstructorInfo parameterConstructor;
            GetConstructorInfos(out parameterConstructor, out defaultConstructor);

            if (pluginParameters == null || pluginParameters.Count() == 0 || pluginParameters.Any(parameter => parameter.Value == null))
            {
                if (defaultConstructor == null)
                {
                    throw new Exception("No parameters provided but no default constructor");
                }
                _pluginCreator = () => (T)defaultConstructor.Invoke(new object[] { });
            }
            else
            {
                var constructorArguments = parameterConstructor.GetParameters()
                    .OrderBy(parameter => parameter.Position)
                    .Select(parameter => pluginParameters.First(pluginParameter => pluginParameter.Name == parameter.Name).Value)
                    .ToArray();

                _pluginCreator = () => (T)parameterConstructor.Invoke(constructorArguments);
            }
        }

        public IPlugin CreatePlugin()
        {
            if (_pluginCreator == null)
            {
                SetParameterValues(null);
            }

            return _pluginCreator();
        }

        private class ConstructorParameterSet
        {
            public ParameterInfo[] Parameter { get; set; }
            public int Count { get; set; }
        }

        private void GetConstructorInfos(out ConstructorInfo parameterConstructor, out ConstructorInfo defaultConstructor)
        {
            List<ConstructorInfo> constructors = typeof(T).GetConstructors()
                .Where(constructorInfo => constructorInfo.IsPublic && !constructorInfo.IsStatic).ToList();

            if (constructors.Count > 2  || constructors.Count == 0) 
            {
                throw new Exception("Generic plugin configurator doesn't support less than 1 or more than 2 constructors. Add your own IPluginConfigurator to the assembly.");
            } else if (constructors.Count == 2) 
            {
                if (constructors[0].GetParameters().Length == 0) 
                {
                    defaultConstructor = constructors[0];
                    parameterConstructor = constructors[1];
                } else if (constructors[1].GetParameters().Length == 0) 
                {
                    defaultConstructor = constructors[1];
                    parameterConstructor = constructors[0];
                } else 
                {
                    throw new Exception("Generic plugin configurator only supports 1 parameterless constructor and 1 with parameters. Add your own IPluginConfigurator to the assembly.");
                }
            } else {
                if (constructors[0].GetParameters().Length == 0)
                {
                    defaultConstructor = constructors[0];
                    parameterConstructor = null;
                }
                else
                {
                    defaultConstructor = null;
                    parameterConstructor = constructors[0];
                }
            }
        }

        public IEnumerable<IPluginParameter> GetParameters()
        {
            ConstructorInfo defaultConstructor;
            ConstructorInfo parameterConstructor;
            GetConstructorInfos(out parameterConstructor, out defaultConstructor);
            if (parameterConstructor == null) 
            {
                return new List<IPluginParameter>();
            }

            return parameterConstructor.GetParameters().Select(parameter => (IPluginParameter)new PluginParameter(
                parameter.Name, parameter.ParameterType, defaultConstructor == null)).ToList(); 
        }
    }
}
