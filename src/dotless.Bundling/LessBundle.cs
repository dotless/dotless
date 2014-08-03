using System.Web.Optimization;

namespace dotless.Bundling
{
    /// <summary>
    /// Use this bundle type to compile and minify the Less CSS preprocessor files for the <see cref="System.Web.Optimization"/>
    /// bundling.
    /// </summary>
    /// <remarks>
    /// <example>
    /// To create a less bundle, add a new LessBundle to your bundling tables and include any less files.
    /// <code>
    /// bundles.Add(new LessBundle("~/Content/css/bootstrap.css")
    ///        .Include("~/content/bootstrap/bootstrap.less"));
    /// </code>
    /// </example>
    /// <example>
    /// To minify the output, add the Dotless configuration section to your web.config and set minify to true.
    /// <code>
    /// <section name="dotless" type="dotless.Core.configuration.DotlessConfigurationSectionHandler,dotless.Core" />
    /// ...
    /// <dotless minifyCss="false" /> 
    /// </code>
    /// </example>
    /// </remarks>
    public class LessBundle : StyleBundle
    {
        public LessBundle(string virtualPath) : base(virtualPath)
        {
            AddTransform();
        }

        public LessBundle(string virtualPath, string cdnPath) : base(virtualPath, cdnPath)
        {
            AddTransform();
        }

        private void AddTransform()
        {
            Transforms.Add(new LessTranform());
        }
    }
}
