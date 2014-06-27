using System.Web.Optimization;
using dotless.Core;

namespace dotless.Bundling
{
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

    public class LessTranform : IBundleTransform 
    {
        public void Process(BundleContext context, BundleResponse response)
        {
            // todo: what about cacheability?
            // todo: virtual paths
            // todo: support running all files as a single less file
            // todo: minification

            var output = "";
            foreach (var file in response.Files)
            {
                // TODO - support a mixture of CSS and less files
                file.Transforms.Add(new CssRewriteUrlTransform());
                file.Transforms.Add(new DotLessItemTransform());

                output += file.ApplyTransforms();
            }

            response.Content = output;
            response.ContentType = "text/css";
        }
    }


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
