using System.Collections.Generic;
using System.Web.Hosting;

namespace dotless.Test.Unit.Bundling
{
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
}