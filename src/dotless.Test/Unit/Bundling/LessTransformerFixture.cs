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

            var pathProvider = new InMemoryVirtualPathProvider()
                .AddFile(inputFilename1, "body { width: 1+1px; }")
                .AddFile(inputFilename2, "h1 { font-size: 3*6em; }");
            BundleTable.VirtualPathProvider = pathProvider;

            var bundle = new LessBundle("~/Content/file.css")
                .Include(inputFilename1)
                .Include(inputFilename2);
            var bundleResponse = bundle.GenerateBundleResponse(CreateBundleContext(bundle));

            Assert.That(bundleResponse.ContentType, Is.EqualTo("text/css"));
            Assert.That(bundleResponse.Content, Is.EqualTo("body {\n  width: 2px;\n}\nh1 {\n  font-size: 18em;\n}\n"));
        }

        // TODO: can it resolve relative path imports?

        [Test]
        public void CanUseRelativePathsInImportStatements()
        {
            // complication: http://benfoster.io/blog/adding-less-support-to-the-aspnet-optimization-framework
            string inputFilename1 = "~/content/file1.less";
            string inputFilename2 = "~/content/file2.less";

            var pathProvider = new InMemoryVirtualPathProvider()
                .AddFile(inputFilename1, "@import \"file2.less\";")
                .AddFile(inputFilename2, ".selector { background: yellow; }");
            BundleTable.VirtualPathProvider = pathProvider;

            var bundle = new LessBundle("~/Content/file.css")
                .Include(inputFilename1);
            var bundleResponse = bundle.GenerateBundleResponse(CreateBundleContext(bundle));

            Assert.That(bundleResponse.Content, Is.EqualTo(".selector {\n background: yellow;\n}\n"));
        }

        [Test]
        public void NestedRelativeImportsAreRelativeToTheCurrentFile()
        {
            // complication: http://benfoster.io/blog/adding-less-support-to-the-aspnet-optimization-framework
            string inputFilename1 = "~/content/file1.less";
            string inputFilename2 = "~/content/project/file2.less";
            string inputFilename3 = "~/content/project/sub/file3.less";

            var pathProvider = new InMemoryVirtualPathProvider()
                .AddFile(inputFilename1, "@import \"project/file2.less\";")
                .AddFile(inputFilename2, "@import \"sub/file3.less\";")
                .AddFile(inputFilename3, ".selector { background: yellow; }");
            BundleTable.VirtualPathProvider = pathProvider;

            var bundle = new LessBundle("~/Content/file.css")
                .Include(inputFilename1);
            var bundleResponse = bundle.GenerateBundleResponse(CreateBundleContext(bundle));

            Assert.That(bundleResponse.Content, Is.EqualTo(".selector {\n background: yellow;\n}\n"));
        }


        [Test]
        public void IncludedFilesRunInTheSameScope()
        {
            // complication: each of these files could be in a different directory, which could change the
            // relative paths of the imports
            string inputFilename1 = "~/content/file1.less";
            string inputFilename2 = "~/content/file2.less";

            var pathProvider = new InMemoryVirtualPathProvider()
                .AddFile(inputFilename1, "@nice-blue: #5B83AD;")
                .AddFile(inputFilename2, ".selector { background: @nice-blue; }");
            BundleTable.VirtualPathProvider = pathProvider;

            var bundle = new LessBundle("~/Content/file.css")
                .Include(inputFilename1)
                .Include(inputFilename2);
            var bundleResponse = bundle.GenerateBundleResponse(CreateBundleContext(bundle));

            Assert.That(bundleResponse.Content, Is.EqualTo(".selector {\n background: @nice-blue;\n}\n"));
        }

        [Test]
        public void AppRelativePathsTranslatedToSiteRelative()
        {
            // complication: have to figure out how to plug in logic to figure out the paths
            // the HTTP handler does it, but I'm not sure it does it correctly (or how)
            string inputFilename = "~/content/input.less";
            var pathProvider = new InMemoryVirtualPathProvider(inputFilename, ".button { background: url('~/image.png'); }");
            BundleTable.VirtualPathProvider = pathProvider;

            var bundle = new LessBundle("~/Content/output.css")
                .Include(inputFilename);
            var bundleResponse = bundle.GenerateBundleResponse(CreateBundleContext(bundle));

            Assert.That(bundleResponse.Content, Is.EqualTo(".button { background: url('/images/image.png'); }"));
        }

        [Test]
        public void UsesDotLessConfigurationFromWebConfig()
        {
            // complication: have to figure out how the damn IOC container works (and most likely
            // write a new setup just for bundling)
            string inputFilename = "~/content/input.less";
            var pathProvider = new InMemoryVirtualPathProvider(inputFilename, ".button { background: blue; }");
            BundleTable.VirtualPathProvider = pathProvider;
            Config.MinifyOutput = true;

            var bundle = new LessBundle("~/Content/output.css")
                .Include(inputFilename);
            var bundleResponse = bundle.GenerateBundleResponse(CreateBundleContext(bundle));

            Assert.That(bundleResponse.Content, Is.EqualTo(".button{background:blue;}"));
        }

        private BundleContext CreateBundleContext(Bundle bundle)
        {
            return new BundleContext(HttpContext.Object, new BundleCollection { bundle }, bundle.Path);
        }
    }
}
