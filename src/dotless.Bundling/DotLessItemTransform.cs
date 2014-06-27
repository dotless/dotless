using System.Web.Optimization;
using dotless.Core;
using dotless.Core.configuration;

namespace dotless.Bundling
{
    public class DotLessItemTransform : IItemTransform
    {
        public string Process(string includedVirtualPath, string input)
        {
            // todo: support passing in web.config configuration
            //var configuration = new WebConfigConfigurationLoader().GetConfiguration();
            //configuration.CacheEnabled = false;

            //var engine = new EngineFactory(configuration).GetEngine();
            var engine = new EngineFactory().GetEngine();
            return engine.TransformToCss(input, includedVirtualPath);
        }
    }
}