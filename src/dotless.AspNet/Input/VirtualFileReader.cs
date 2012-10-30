namespace dotless.Core.Input
{
    using System.IO;
    using System.Web.Hosting;

    public class VirtualFileReader : IFileReader
    {
        public byte[] GetBinaryFileContents(string fileName)
        {
            var virtualPathProvider = HostingEnvironment.VirtualPathProvider;
            var virtualFile = virtualPathProvider.GetFile(fileName);
            using (var stream = virtualFile.Open())
            {
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int)stream.Length);
                return buffer;
            }
        }

        public string GetFileContents(string fileName)
        {
            var virtualPathProvider = HostingEnvironment.VirtualPathProvider;
            var virtualFile = virtualPathProvider.GetFile(fileName);
            using (var streamReader = new StreamReader(virtualFile.Open()))
            {
                return streamReader.ReadToEnd();
            }
        }

        public bool DoesFileExist(string fileName)
        {
            var virtualPathProvider = HostingEnvironment.VirtualPathProvider;
            return virtualPathProvider.GetFile(fileName) != null;
        }

        public bool UseCacheDependencies { get { return false; } }
    }
}