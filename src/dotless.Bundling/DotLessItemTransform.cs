using System.Web;
using System.Web.Optimization;
using dotless.Core;
using dotless.Core.configuration;

namespace dotless.Bundling
{
    public class DotLessItemTransform : IItemTransform
    {
        private readonly BundleContext _context;

        public DotLessItemTransform(BundleContext context)
        {
            _context = context;
        }

        public string Process(string includedVirtualPath, string input)
        {
            // todo: support passing in web.config configuration
            var configuration = new WebConfigConfigurationLoader().GetConfiguration();
            configuration.DisableParameters = true; // todo: what?

            var engine = new EngineFactory(configuration).GetEngine(new BundlingContainerFactory(_context.HttpContext));
            return engine.TransformToCss(input, includedVirtualPath);
        }
    }
}