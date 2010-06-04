namespace dotless.Core.Importers
{
    using System.IO;

    public class FileImporter : Importer
    {
        protected override string GetImportContents(string path)
        {
            return File.ReadAllText(path);
        }
    }
}