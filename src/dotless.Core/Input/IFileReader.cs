namespace dotless.Core.Input
{
    public interface IFileReader
    {
        string GetFileContents(string fileName);

        bool DoesFileExist(string fileName);
    }
}