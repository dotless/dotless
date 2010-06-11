namespace dotless.Core.Parameters
{
    using System.Collections.Generic;

    public class ConsoleArgumentParameterSource : IParameterSource
    {
        public static IDictionary<string, string> ConsoleArguments = new Dictionary<string, string>();
        public IDictionary<string, string> GetParameters()
        {
            return ConsoleArguments;
        }
    }
}