namespace dotless.Core.Plugins
{
    using System;
    using System.Collections.Generic;
    using dotless.Core.Parser.Infrastructure;

    public interface IFunctionPlugin : IPlugin
    {
        Dictionary<string, Type> GetFunctions();
    }
}
