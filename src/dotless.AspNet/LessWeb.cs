using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotless.Core.configuration;

namespace dotless.Core
{
    public static class LessWeb
    {
        public static string Parse(string less, DotlessConfiguration config)
        {
            return new EngineFactory(config).GetEngine(new AspNetContainerFactory()).TransformToCss(less, null);
        }
    }
}
