namespace dotless.Core.Plugins
{
    using System;
    using System.Globalization;

    public class PluginParameter : IPluginParameter
    {
        public PluginParameter(string name, Type type, bool isMandatory)
        {
            Name = name;
            IsMandatory = isMandatory;
            Type = type;
        }

        public string Name
        {
            get;
            private set;
        }

        public bool IsMandatory
        {
            get;
            private set;
        }

        public object Value
        {
            get;
            private set;
        }

        private Type Type
        {
            get;
            set;
        }

        public string TypeDescription
        {
            get { return Type.Name; }
        }

        public void SetValue(string stringValue)
        {
            if (Type.Equals(typeof(Boolean)))
            {
                if (stringValue.Equals("true", StringComparison.InvariantCultureIgnoreCase) ||
                    stringValue.Equals("t", StringComparison.InvariantCultureIgnoreCase) ||
                    stringValue.Equals("1", StringComparison.InvariantCultureIgnoreCase))
                {
                    Value = true;
                }
                else
                {
                    Value = false;
                }
            }
            else
            {
                Value = Convert.ChangeType(stringValue, Type, CultureInfo.InvariantCulture);
            }
        }
    }
}
