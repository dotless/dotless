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
        protected readonly List<string> _paths = new List<string>();

        protected string CurrentDirectory
        {
            get
            {
                return System.Environment.CurrentDirectory;
            }
        }

        /// <summary>
        ///  Whether or not the importer should alter urls
        /// </summary>
        public bool IsUrlRewritingDisabled { get; set; }

        public Importer() : this(new FileReader())
        {
        }

        public Importer(IFileReader fileReader) : this(fileReader, false)
        {
        }

        public Importer(IFileReader fileReader, bool disableUrlReWriting)
        {
            FileReader = fileReader;
            IsUrlRewritingDisabled = disableUrlReWriting;
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

            var file = _paths.Concat(new[] { import.Path }).AggregatePaths(CurrentDirectory);

            if (!FileReader.DoesFileExist(file) && !file.EndsWith(".less"))
            {
                file = file + ".less";
            }

            if (!FileReader.DoesFileExist(file))
            {
                return false;
            }

            var contents = FileReader.GetFileContents(file);

            _paths.Add(Path.GetDirectoryName(import.Path));

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
                _paths.RemoveAt(_paths.Count - 1);
            }

            return true;
        }

        /// <summary>
        ///  Called for every Url and allows the importer to adjust relative url's to be relative to the
        ///  primary url
        /// </summary>
        public string AlterUrl(string url)
        {
            if (_paths.Any() && !IsUrlRewritingDisabled)
            {
                return _paths.Concat(new[] { url }).AggregatePaths(CurrentDirectory);
            }

            return url;
        }
    }
}