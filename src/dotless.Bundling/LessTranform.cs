using System.Collections.Generic;
using System.Text;
using System.Web.Optimization;
using dotless.Core;
using dotless.Core.configuration;

namespace dotless.Bundling
{
    /// <summary>
    /// Use this transformation to add support for the Less CSS preprocessor to the <see cref="System.Web.Optimization.Bundle"/> 
    /// functionality.
    /// </summary>
    /// <remarks>
    /// <example>
    /// To add the Less transformation to your bundling, just add the transform to your bundle and then include any .less files.
    /// <code>
    /// bundles.Add(new Bundle("~/Content/css/bootstrap.css", new LessTranform())
    ///        .Include("~/content/bootstrap.less"));
    /// </code>
    /// </example>
    /// <para>The LessTransform will need to be the first item in your bundling pipeline because any other transformers
    /// won't work until the files have been turned into CSS.</para>
    /// <para>If you don't want to use any other transforms, you can create a <see cref="LessBundle"/> and add your
    /// files directly to that.</para>
    /// <example>
    /// To minify the output, add the Dotless configuration section to your web.config and set minify to true.
    /// <code>
    /// <section name="dotless" type="dotless.Core.configuration.DotlessConfigurationSectionHandler,dotless.Core" />
    /// ...
    /// <dotless minifyCss="false" /> 
    /// </code>
    /// </example>
    /// </remarks>
    public class LessTranform : IBundleTransform 
    {
        public void Process(BundleContext context, BundleResponse response)
        {
            var sharedLessFile = CreateSharedLessFile(response.Files);

            response.Content = GenerateCss(sharedLessFile, context);
            response.ContentType = "text/css";
        }

        private string CreateSharedLessFile(IEnumerable<BundleFile> files)
        {
            // This makes the different relative paths of the source files resolve properly 
            // even if they do not live underneath the destination virtual path
            // Also has the added benefit that you can create different bundles with the same 
            // source files where you can override variables in different ways if you need 
            // to support something like different browser support or branding
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
            configuration.DisableParameters = true; 

            var logger = new InMemoryLogger(configuration.LogLevel);
            var engine = new EngineFactory(configuration).GetEngine(CreateContainer(context, logger));
            var cssOutput = engine.TransformToCss(import, context.BundleVirtualPath);
            if (!engine.LastTransformationSuccessful)
            {
                return logger.GetOutput();
            }

            return cssOutput;
        }

        private BundlingContainerFactory CreateContainer(BundleContext context, InMemoryLogger logger)
        {
            return new BundlingContainerFactory(context.HttpContext, logger, BundleTable.VirtualPathProvider);
        }
    }
}