using System.Collections.Generic;
using System.IO;
using dotless.Infrastructure;

namespace dotless.Tests
{
  public class DictionaryImporter : Importer
  {
    public Dictionary<string, string> Contents;

    public DictionaryImporter()
    {
      Contents = new Dictionary<string, string>();
    }

    public DictionaryImporter(Dictionary<string, string> contents)
    {
      Contents = contents;
    }

    protected override string GetImportContents(string path)
    {
      if (Contents.ContainsKey(path))
        return Contents[path];

      throw new FileNotFoundException("Import not found", path);
    }
  }
}