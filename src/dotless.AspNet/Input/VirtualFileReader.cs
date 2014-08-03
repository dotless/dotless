namespace dotless.Core.Input
{
    using System.IO;
    using System.Web.Hosting;

    public class VirtualFileReader : IFileReader
    {
        private readonly VirtualPathProvider _virtualPathProvider;

        public VirtualFileReader(VirtualPathProvider virtualPathProvider)
        {
            _virtualPathProvider = virtualPathProvider;
        }

        public byte[] GetBinaryFileContents(string fileName)
        {
            var virtualFile = _virtualPathProvider.GetFile(fileName);
            using (var stream = virtualFile.Open())
            {
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int)stream.Length);
                return buffer;
            }
        }

        public string GetFileContents(string fileName)
        {
            var virtualFile = _virtualPathProvider.GetFile(fileName);
            using (var streamReader = new StreamReader(virtualFile.Open()))
            {
                return streamReader.ReadToEnd();
            }
        }

        public bool DoesFileExist(string fileName)
        {
            return _virtualPathProvider.FileExists(fileName);
        }

        public bool UseCacheDependencies { get { return false; } }
    }
}