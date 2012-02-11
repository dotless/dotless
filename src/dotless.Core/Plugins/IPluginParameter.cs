namespace dotless.Core.Plugins
{
    using System;

    public interface IPluginParameter
    {
        string Name { get; }

        bool IsMandatory { get; }

        object Value { get; }

        string TypeDescription { get; }

        void SetValue(string value);
    }
}
