using System.Web;
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
                .AddFile(inputFilename2, "h1 { font-size: 3*16em; }");
            SetUpPathProvider(pathProvider);

            var bundle = new LessBundle("~/Content/file.css")
                .Include(inputFilename1)
                .Include(inputFilename2);
            var bundleResponse = bundle.GenerateBundleResponse(CreateBundleContext(bundle));

            Assert.That(bundleResponse.ContentType, Is.EqualTo("text/css"));
            Assert.That(bundleResponse.Content, Is.EqualTo("body {\n  width: 2px;\n}\nh1 {\n  font-size: 48em;\n}\n"));
        }

        [Test]
        public void CanUseRelativePathsInImportStatements()
        {
            string inputFilename1 = "~/content/parent.less";
            string inputFilename2 = "~/content/child.less";

            var pathProvider = new InMemoryVirtualPathProvider()
                .AddFile(inputFilename1, "@import \"child.less\";")
                .AddFile(inputFilename2, ".selector { background: yellow; }");
            SetUpPathProvider(pathProvider);

            var bundle = new LessBundle("~/output/relative-paths.css")
                .Include(inputFilename1);
            var bundleResponse = bundle.GenerateBundleResponse(CreateBundleContext(bundle));

            Assert.That(bundleResponse.Content, Is.EqualTo(".selector {\n  background: yellow;\n}\n"));
        }

        [Test]
        public void NestedRelativeImportsAreRelativeToTheCurrentFile()
        {
            string inputFilename1 = "~/content/relative-parent.less";
            string inputFilename2 = "~/content/project/child.less";
            string inputFilename3 = "~/content/project/sub/grandchild.less";

            var pathProvider = new InMemoryVirtualPathProvider()
                .AddFile(inputFilename1, "@import \"project/child.less\";")
                .AddFile(inputFilename2, "@import \"sub/grandchild.less\";")
                .AddFile(inputFilename3, ".selector { background: yellow; }");
            SetUpPathProvider(pathProvider);

            var bundle = new LessBundle("~/output/relative-paths.css")
                .Include(inputFilename1);
            var bundleResponse = bundle.GenerateBundleResponse(CreateBundleContext(bundle));

            Assert.That(bundleResponse.Content, Is.EqualTo(".selector {\n  background: yellow;\n}\n"));
        }

        [Test]
        public void IncludedFilesRunInTheSameScope()
        {
            string inputFilename1 = "~/content/variable.less";
            string inputFilename2 = "~/content/selector.less";

            var pathProvider = new InMemoryVirtualPathProvider()
                .AddFile(inputFilename1, "@nice-blue: #5B83AD;")
                .AddFile(inputFilename2, ".selector { background: @nice-blue; }");
            SetUpPathProvider(pathProvider);

            var bundle = new LessBundle("~/output/same-scope.css")
                .Include(inputFilename1)
                .Include(inputFilename2);
            var bundleResponse = bundle.GenerateBundleResponse(CreateBundleContext(bundle));

            Assert.That(bundleResponse.Content, Is.EqualTo(".selector {\n  background: #5b83ad;\n}\n"));
        }

        [Test]
        public void UsesDotLessConfigurationFromWebConfig()
        {
            string inputFilename = "~/content/minified.less";
            var pathProvider = new InMemoryVirtualPathProvider(inputFilename, ".button { background: blue;}");
            SetUpPathProvider(pathProvider);
            Config.MinifyOutput = true;

            var bundle = new LessBundle("~/output/minified.css")
                .Include(inputFilename);
            var bundleResponse = bundle.GenerateBundleResponse(CreateBundleContext(bundle));

            Assert.That(bundleResponse.Content, Is.EqualTo(".button{background:blue}"));
        }

        [Test]
        public void ShowParsingErrorsInBundleResponse()
        {
            string inputFilename = "~/content/error.less";
            var pathProvider = new InMemoryVirtualPathProvider(inputFilename, ".button { background: blue;");
            SetUpPathProvider(pathProvider);

            var bundle = new LessBundle("~/output/error.css")
                .Include(inputFilename);
            var bundleResponse = bundle.GenerateBundleResponse(CreateBundleContext(bundle));

            Assert.That(bundleResponse.Content, Is.StringStarting("Missing closing '}' on line 1 in file '~/content/error.less':"));
        }

        [Test]
        public void OtherTransformsCanRunAfterLessTransformInBundlingPipeline()
        {
            string inputFilename = "~/content/multiple-transform.less";
            var pathProvider = new InMemoryVirtualPathProvider(inputFilename, ".button { background: blue; }");
            SetUpPathProvider(pathProvider);

            var bundle = new Bundle("~/output/multiple-transform.css", new LessTranform(), new CssMinify())
                .Include(inputFilename);
            var bundleResponse = bundle.GenerateBundleResponse(CreateBundleContext(bundle));

            Assert.That(bundleResponse.Content, Is.EqualTo(".button{background:blue}"));
        }

        private BundleContext CreateBundleContext(Bundle bundle)
        {
            //var context =  new HttpContext(new HttpRequest(null, "http://tempuri.org", null),
            //                               new HttpResponse(null));
            return new BundleContext(HttpContext.Object, new BundleCollection { bundle }, bundle.Path);
        }

        private void SetUpPathProvider(InMemoryVirtualPathProvider pathProvider)
        {
            BundleTable.VirtualPathProvider = pathProvider;
            Config.LessSource = typeof(VirtualFileReader);
        }
    }
}
