namespace dotless.Core.Plugins
{
    using System;

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
            Value = Convert.ChangeType(stringValue, Type);
        }
    }
}
