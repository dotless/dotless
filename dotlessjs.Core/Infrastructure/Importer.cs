using dotless.Tree;

namespace dotless.Infrastructure
{
  public abstract class Importer
  {
    public void Import(Import import)
    {
      var contents = GetImportContents(import.Path);

      import.InnerRoot = new Parser {Importer = this}.Parse(contents);
    }

    protected abstract string GetImportContents(string path);
  }
}