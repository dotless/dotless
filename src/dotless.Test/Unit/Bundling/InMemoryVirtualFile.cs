using System.IO;
using System.Text;
using System.Web.Hosting;

namespace dotless.Test.Unit.Bundling
{
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