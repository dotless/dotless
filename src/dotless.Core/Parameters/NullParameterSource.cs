using System.Collections.Generic;

namespace dotless.Core.Parameters {
    public class NullParameterSource : IParameterSource
    {
        public IDictionary<string, string> GetParameters()
        {
            return new Dictionary<string, string>();
        }
    }
}