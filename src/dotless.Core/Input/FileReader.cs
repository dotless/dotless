namespace dotless.Core.Input
{
    using System.IO;

    public class FileReader : IFileReader
    {
        public IPathResolver PathResolver { get; set; }

        public FileReader() : this(new RelativePathResolver())
        {
        }

        public FileReader(IPathResolver pathResolver)
        {
            PathResolver = pathResolver;
        }

        public byte[] GetBinaryFileContents(string fileName)
        {
            fileName = PathResolver.GetFullPath(fileName);

            return File.ReadAllBytes(fileName);
        }

        public string GetFileContents(string fileName)
        {
            fileName = PathResolver.GetFullPath(fileName);

            return File.ReadAllText(fileName);
        }

        public bool DoesFileExist(string fileName)
        {
            fileName = PathResolver.GetFullPath(fileName);

            return File.Exists(fileName);
        }

        public bool UseCacheDependencies { get { return true; } }
    }
}