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

        public IPlugin CreatePlugin(IEnumerable<IPluginParameter> pluginParameters)
        {
            ConstructorInfo defaultConstructor;
            ConstructorInfo parameterConstructor;
            GetConstructorInfos(out parameterConstructor, out defaultConstructor);

            if (pluginParameters.Count() == 0 || pluginParameters.Any(parameter => parameter.Value == null))
            {
                if (defaultConstructor == null)
                {
                    throw new Exception();
                }
                return (T)defaultConstructor.Invoke(new object[] { });
            }

            return (T)parameterConstructor.Invoke(parameterConstructor.GetParameters()
                .OrderBy(parameter => parameter.Position)
                .Select(parameter => pluginParameters.First(pluginParameter => pluginParameter.Name == parameter.Name).Value)
                .ToArray());
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
                throw new Exception();
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
                    throw new Exception();
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
                parameter.Name, parameter.ParameterType, defaultConstructor == null)); 
        }
    }
}
