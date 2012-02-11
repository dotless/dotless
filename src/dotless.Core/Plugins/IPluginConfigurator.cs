namespace dotless.Core.Plugins
{
    using System;
    using System.Collections.Generic;

    public interface IPluginConfigurator
    {
        IPlugin CreatePlugin(IEnumerable<IPluginParameter> parameters);

        IEnumerable<IPluginParameter> GetParameters();

        string Name { get; }

        string Description { get; }

        Type Configurates { get; }
    }
}
