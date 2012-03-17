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
        ///  Get a list of the current paths, used to pass back in to alter url's after evaluation
        /// </summary>
        /// <returns></returns>
        public List<string> GetCurrentPathsClone()
        {
            return new List<string>(_paths);
        }

        /// <summary>
        ///  Imports the file inside the import as a dot-less file.
        /// </summary>
        /// <param name="import"></param>
        /// <returns> The action for the import node to process</returns>
        public virtual ImportAction Import(Import import)
        {
            if (import.Path.EndsWith(".css"))
            {
                return ImportAction.LeaveImport;
            }

            if (Parser == null)
                throw new InvalidOperationException("Parser cannot be null.");

            var file = GetAdjustedFilePath(import.Path, _paths);

            if (!ImportLessFile(file, import))
            {
                if (import.Path.EndsWith(".less", StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new FileNotFoundException("You are importing a file ending in .less that cannot be found.", import.Path);
                }
                return ImportAction.LeaveImport;
            }

            return ImportAction.ImportLess;
        }

        /// <summary>
        ///  Uses the paths to adjust the file path
        /// </summary>
        protected string GetAdjustedFilePath(string path, List<string> pathList)
        {
            return pathList.Concat(new[] { path }).AggregatePaths(CurrentDirectory);
        }

        /// <summary>
        ///  Imports a less file and puts the root into the import node
        /// </summary>
        protected bool ImportLessFile(string file, Import import)
        {
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
        ///  Imports a css file and puts the contents into the import node
        /// </summary>
        protected bool ImportCssFileContents(string file, Import import)
        {
            if (!FileReader.DoesFileExist(file))
            {
                return false;
            }

            import.InnerContent = FileReader.GetFileContents(file);

            return true;
        }

        /// <summary>
        ///  Called for every Url and allows the importer to adjust relative url's to be relative to the
        ///  primary url
        /// </summary>
        public string AlterUrl(string url, List<string> pathList)
        {
            if (pathList.Any() && !IsUrlRewritingDisabled)
            {
                return GetAdjustedFilePath(url, pathList);
            }

            return url;
        }
    }
}