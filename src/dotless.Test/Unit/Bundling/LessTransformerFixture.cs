using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Hosting;
using System.Web.Optimization;
using dotless.Bundling;

namespace dotless.Test.Unit
{
    using NUnit.Framework;

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

    public class InMemoryVirtualPathProvider : VirtualPathProvider
    {
        readonly IDictionary<string, string> _files = new Dictionary<string, string>();

        public InMemoryVirtualPathProvider(IDictionary<string, string> files)
        {
            _files = files;
        }

        public InMemoryVirtualPathProvider()
        {
        }

        public InMemoryVirtualPathProvider AddFile(string filename, string contents)
        {
            _files.Add(filename, contents);
            return this;
        } 

        public override bool FileExists(string virtualPath)
        {
            return _files.ContainsKey(virtualPath);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            return new InMemoryVirtualFile(virtualPath, _files[virtualPath]);
        }

         
    }

    public class InMemoryVirtualFile : VirtualFile
    {
        private readonly string _virtualPath;
        private readonly string _content;

        public InMemoryVirtualFile(string virtualPath, string content) : base(virtualPath.Replace("~", @"c:"))
        {
            _virtualPath = virtualPath;
            _content = content;
        }

        public override Stream Open()
        {
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(_content ?? ""));
            return memoryStream;
        }

        public override string Name
        {
            get { return _virtualPath; }
        }
    }
}
