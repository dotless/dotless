using System.Web.Optimization;
using dotless.Bundling;
using NUnit.Framework;

namespace dotless.Test.Unit.Bundling
{
    public class LessTransformerFixture : HttpFixtureBase
    {

        [Test]
        public void TransformFilesToLess()
        {
            string inputFilename1 = "~/content/file1.less";
            string inputFilename2 = "~/content/file2.less";
            string outputFilename = "~/Content/file.css";

            BundleTable.VirtualPathProvider = new InMemoryVirtualPathProvider()
                .AddFile(inputFilename1, "body { width: 1+1px; }")
                .AddFile(inputFilename2, "h1 { font-size: 3*6em; }");

            var bundle = new LessBundle(outputFilename)
                .Include(inputFilename1)
                .Include(inputFilename2);
            var bundleResponse = bundle.GenerateBundleResponse(CreateBundleContext(bundle, outputFilename));

            Assert.That(bundleResponse.ContentType, Is.EqualTo("text/css"));
            Assert.That(bundleResponse.Content, Is.EqualTo("body {\n  width: 2px;\n}\nh1 {\n  font-size: 18em;\n}\n"));
        }


        private BundleContext CreateBundleContext(Bundle bundle, string virtualPath)
        {
            return new BundleContext(HttpContext.Object, new BundleCollection {bundle}, virtualPath);
        }
    }
}
