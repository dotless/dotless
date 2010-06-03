namespace dotless.Core.Parser.Infrastructure.Importers
{
    using Tree;

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