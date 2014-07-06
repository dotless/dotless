using System.Collections.Generic;
using System.Text;
using System.Web.Optimization;
using dotless.Core;
using dotless.Core.configuration;

namespace dotless.Bundling
{
    public class LessTranform : IBundleTransform 
    {
        public void Process(BundleContext context, BundleResponse response)
        {
            // todo: what about cacheability?
            // todo: support running all files as a single less file
            // todo: minification
            // todo: what about in debug mode? - can the less handler help here?

            var sharedLessFile = CreateSharedLessFile(response.Files);

            response.Content = GenerateCss(sharedLessFile, context);
            response.ContentType = "text/css";
        }

        private string CreateSharedLessFile(IEnumerable<BundleFile> files)
        {
            var root = new StringBuilder();
            foreach (var file in files)
            {
                root.AppendFormat("@import \"{0}\";", file.IncludedVirtualPath);
            }
            return root.ToString();
        }

        private string GenerateCss(string import, BundleContext context)
        {
            var configuration = new WebConfigConfigurationLoader().GetConfiguration();
            configuration.DisableParameters = true; // todo: what?

            // todo: should we create the container each time?
            var engine = new EngineFactory(configuration).GetEngine(new BundlingContainerFactory(context.HttpContext, BundleTable.VirtualPathProvider));
            return engine.TransformToCss(import, context.BundleVirtualPath);
        }
    }
}