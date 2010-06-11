namespace dotless.Core.Importers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Input;
    using Parser.Tree;
    using Parser;

    public class Importer
    {
        public IFileReader FileReader { get; set; }
        public List<string> Imports { get; set; }
        public Func<Parser> Parser { get; set; }
        private readonly List<string> paths = new List<string>();

        public Importer() : this(new FileReader())
        {
        }

        public Importer(IFileReader fileReader)
        {
            FileReader = fileReader;
            Imports = new List<string>();
        }

        public void Import(Import import)
        {
            if (Parser == null)
                throw new InvalidOperationException("Parser cannot be null.");

            var file = paths.Concat(new[] { import.Path }).Aggregate("", Path.Combine);

            var contents = FileReader.GetFileContents(file);

            paths.Add(Path.GetDirectoryName(import.Path));

            try
            {
                import.InnerRoot = Parser().Parse(contents, file);
                Imports.Add(file);
            }
            finally
            {
                paths.RemoveAt(paths.Count - 1);
            }
        }
    }
}