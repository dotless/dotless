using System.Web.Optimization;
using dotless.Bundling;
using dotless.Core.Input;
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
            SetUpPathProvider(pathProvider);

            var bundle = new LessBundle("~/Content/file.css")
                .Include(inputFilename1)
                .Include(inputFilename2);
            var bundleResponse = bundle.GenerateBundleResponse(CreateBundleContext(bundle));

            Assert.That(bundleResponse.ContentType, Is.EqualTo("text/css"));
            Assert.That(bundleResponse.Content, Is.EqualTo("body {\n  width: 2px;\n}\nh1 {\n  font-size: 18em;\n}\n"));
        }

        [Test]
        public void CanUseRelativePathsInImportStatements()
        {
            // complication: http://benfoster.io/blog/adding-less-support-to-the-aspnet-optimization-framework
            string inputFilename1 = "~/content/file1.less";
            string inputFilename2 = "~/content/file2.less";

            var pathProvider = new InMemoryVirtualPathProvider()
                .AddFile(inputFilename1, "@import \"file2.less\";")
                .AddFile(inputFilename2, ".selector { background: yellow; }");
            SetUpPathProvider(pathProvider);

            var bundle = new LessBundle("~/Content/file.css")
                .Include(inputFilename1);
            var bundleResponse = bundle.GenerateBundleResponse(CreateBundleContext(bundle));

            Assert.That(bundleResponse.Content, Is.EqualTo(".selector {\n  background: yellow;\n}\n"));
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
            SetUpPathProvider(pathProvider);

            var bundle = new LessBundle("~/Content/file.css")
                .Include(inputFilename1);
            var bundleResponse = bundle.GenerateBundleResponse(CreateBundleContext(bundle));

            Assert.That(bundleResponse.Content, Is.EqualTo(".selector {\n  background: yellow;\n}\n"));
        }

        [Test]
        public void IncludedFilesRunInTheSameScope()
        {
            string inputFilename1 = "~/content/file1.less";
            string inputFilename2 = "~/content/file2.less";

            var pathProvider = new InMemoryVirtualPathProvider()
                .AddFile(inputFilename1, "@nice-blue: #5B83AD;")
                .AddFile(inputFilename2, ".selector { background: @nice-blue; }");
            SetUpPathProvider(pathProvider);

            var bundle = new LessBundle("~/Content/file.css")
                .Include(inputFilename1)
                .Include(inputFilename2);
            var bundleResponse = bundle.GenerateBundleResponse(CreateBundleContext(bundle));

            Assert.That(bundleResponse.Content, Is.EqualTo(".selector {\n  background: #5b83ad;\n}\n"));
        }

        [Test]
        public void AppRelativePathsTranslatedToSiteRelative()
        {
            // complication: have to figure out how to plug in logic to figure out the paths
            // the HTTP handler does it, but I'm not sure it does it correctly (or how)
            string inputFilename = "~/content/input.less";
            var pathProvider = new InMemoryVirtualPathProvider(inputFilename, ".button { background: url('~/image.png'); }");
            SetUpPathProvider(pathProvider);

            var bundle = new LessBundle("~/Content/output.css")
                .Include(inputFilename);
            var bundleResponse = bundle.GenerateBundleResponse(CreateBundleContext(bundle));

            Assert.That(bundleResponse.Content, Is.EqualTo(".button {\n  background: url(/image.png);\n}\n"));
        }

        [Test]
        public void UsesDotLessConfigurationFromWebConfig()
        {
            string inputFilename = "~/content/input.less";
            var pathProvider = new InMemoryVirtualPathProvider(inputFilename, ".button { background: blue;}");
            SetUpPathProvider(pathProvider);
            Config.MinifyOutput = true;

            var bundle = new LessBundle("~/Content/output.css")
                .Include(inputFilename);
            var bundleResponse = bundle.GenerateBundleResponse(CreateBundleContext(bundle));

            Assert.That(bundleResponse.Content, Is.EqualTo(".button{background:blue}"));
        }

        [Test]
        public void ShowParsingErrorsInBundleResponse()
        {
            string inputFilename = "~/content/input.less";
            var pathProvider = new InMemoryVirtualPathProvider(inputFilename, ".button { background: blue;");
            SetUpPathProvider(pathProvider);

            var bundle = new LessBundle("~/Content/output.css")
                .Include(inputFilename);
            var bundleResponse = bundle.GenerateBundleResponse(CreateBundleContext(bundle));

            Assert.That(bundleResponse.Content, Is.StringStarting("Missing closing '}' on line 1 in file '~/content/input.less':"));
        }

        private BundleContext CreateBundleContext(Bundle bundle)
        {
            return new BundleContext(HttpContext.Object, new BundleCollection { bundle }, bundle.Path);
        }

        private void SetUpPathProvider(InMemoryVirtualPathProvider pathProvider)
        {
            BundleTable.VirtualPathProvider = pathProvider;
            Config.LessSource = typeof(VirtualFileReader);
        }
    }
}
