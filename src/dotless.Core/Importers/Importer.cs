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
    using System.Text.RegularExpressions;

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

        /// <summary>
        ///  Import all files as if they are less regardless of file extension
        /// </summary>
        public bool ImportAllFilesAsLess { get; set; }

        /// <summary>
        ///  Import the css and include inline
        /// </summary>
        public bool InlineCssFiles { get; set; }


        public Importer() : this(new FileReader())
        {
        }

        public Importer(IFileReader fileReader) : this(fileReader, false, false, false)
        {
        }

        public Importer(IFileReader fileReader, bool disableUrlReWriting, bool inlineCssFiles, bool importAllFilesAsLess)
        {
            FileReader = fileReader;
            IsUrlRewritingDisabled = disableUrlReWriting;
            InlineCssFiles = inlineCssFiles;
            ImportAllFilesAsLess = importAllFilesAsLess;
            Imports = new List<string>();
        }

        /// <summary>
        ///  Whether a url has a protocol on it
        /// </summary>
        private static bool IsProtocolUrl(string url)
        {
            return Regex.IsMatch(url, @"^([a-zA-Z]+:)");
        }

        /// <summary>
        ///  Whether a url has a protocol on it
        /// </summary>
        private static bool IsNonRelativeUrl(string url)
        {
            return url.StartsWith(@"/");
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
            // if the import is protocol'd (e.g. http://www.opencss.com/css?blah) then leave the import alone
            if (IsProtocolUrl(import.Path))
            {
                if (import.Path.EndsWith(".less"))
                {
                    throw new FileNotFoundException(".less cannot import non local less files.", import.Path);
                }
                return ImportAction.LeaveImport;
            }

            var file = GetAdjustedFilePath(import.Path, _paths);

            if (!ImportAllFilesAsLess && import.Path.EndsWith(".css"))
            {
                if (InlineCssFiles && ImportCssFileContents(file, import))
                    return ImportAction.ImportCss;

                return ImportAction.LeaveImport;
            }

            if (Parser == null)
                throw new InvalidOperationException("Parser cannot be null.");

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
            if (pathList.Any() && !IsUrlRewritingDisabled && !IsProtocolUrl(url) && !IsNonRelativeUrl(url))
            {
                return GetAdjustedFilePath(url, pathList);
            }

            return url;
        }
    }
}