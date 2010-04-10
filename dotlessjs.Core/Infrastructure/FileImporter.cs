using System.IO;

namespace dotless.Infrastructure
{
  public class FileImporter : Importer
  {
    protected override string GetImportContents(string path)
    {
      return File.ReadAllText(path);
    }
  }
}