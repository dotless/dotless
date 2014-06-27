using System.Web.Optimization;

namespace dotless.Bundling
{
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
                file.Transforms.Add(new DotLessItemTransform());
                file.Transforms.Add(new CssRewriteUrlTransform());

                output += file.ApplyTransforms();
            }

            response.Content = output;
            response.ContentType = "text/css";
        }
    }
}