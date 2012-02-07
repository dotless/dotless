namespace dotless.Core.Importers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Input;
    using Parser;
    using Parser.Tree;
    using Utils;

    public class Importer : IImporter
    {
        public IFileReader FileReader { get; set; }
        public List<string> Imports { get; set; }
        public Func<Parser> Parser { get; set; }
        private readonly List<string> _paths = new List<string>();
        public List<string> Paths
        {
            get
            {
                return _paths;
            }
        }
        public string CurrentDirectory
        {
            get
            {
                return System.Environment.CurrentDirectory;
            }
        }

        public Importer() : this(new FileReader())
        {
        }

        public Importer(IFileReader fileReader)
        {
            FileReader = fileReader;
            Imports = new List<string>();
        }

        /// <summary>
        ///  Imports the file inside the import as a dot-less file.
        /// </summary>
        /// <param name="import"></param>
        /// <returns> Whether the file was found - so false if it cannot be found</returns>
        public virtual bool Import(Import import)
        {
            if (Parser == null)
                throw new InvalidOperationException("Parser cannot be null.");

            var file = Paths.Concat(new[] { import.Path }).AggregatePaths(CurrentDirectory);

            if (!FileReader.DoesFileExist(file) && !file.EndsWith(".less"))
            {
                file = file + ".less";
            }

            if (!FileReader.DoesFileExist(file))
            {
                return false;
            }

            var contents = FileReader.GetFileContents(file);

            Paths.Add(Path.GetDirectoryName(import.Path));

            try
            {
                Imports.Add(file);
                import.InnerRoot = Parser().Parse(contents, file);
            }
            catch
            {
                Imports.Remove(file);
                throw;
            }
            finally
            {
                Paths.RemoveAt(Paths.Count - 1);
            }

            return true;
        }
    }
}