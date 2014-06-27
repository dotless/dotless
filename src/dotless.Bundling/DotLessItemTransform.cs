using System.Web.Optimization;
using dotless.Core;

namespace dotless.Bundling
{
    public class DotLessItemTransform : IItemTransform
    {
        public string Process(string includedVirtualPath, string input)
        {
            // todo: support passing in web.config configuration
            var engine = new EngineFactory().GetEngine();
            return engine.TransformToCss(input, includedVirtualPath);
        }
    }
}