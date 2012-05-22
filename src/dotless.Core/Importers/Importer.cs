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
    using System.Reflection;

    public class Importer : IImporter
    {
        private static readonly Regex _embeddedResourceRegex = new Regex(@"^dll://(?<Assembly>.+?)/(?<Resource>.+)$");

        public static Regex EmbeddedResourceRegex { get { return _embeddedResourceRegex; } }
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
            return Regex.IsMatch(url, @"^([a-zA-Z]{2,}:)");
        }

        /// <summary>
        ///  Whether a url has a protocol on it
        /// </summary>
        private static bool IsNonRelativeUrl(string url)
        {
            return url.StartsWith(@"/") || url.StartsWith(@"~/") || Regex.IsMatch(url, @"^[a-zA-Z]:");
        }

        /// <summary>
        /// Whether a url represents an embedded resource
        /// </summary>
        private static bool IsEmbeddedResource(string path)
        {
            return _embeddedResourceRegex.IsMatch(path);
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
            if (IsProtocolUrl(import.Path) && !IsEmbeddedResource(import.Path))
            {
                if (import.Path.EndsWith(".less"))
                {
                    throw new FileNotFoundException(".less cannot import non local less files.", import.Path);
                }
                return ImportAction.LeaveImport;
            }

            var file = import.Path;
            
            if (!IsNonRelativeUrl(file)) 
            {
                file = GetAdjustedFilePath(import.Path, _paths);
            }

            if (!ImportAllFilesAsLess && import.Path.EndsWith(".css"))
            {
                if (InlineCssFiles)
                {
                    if (IsEmbeddedResource(import.Path) && ImportEmbeddedCssContents(file, import))                         
                        return ImportAction.ImportCss;
                    if (ImportCssFileContents(file, import))
                        return ImportAction.ImportCss;
                }

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
            string contents;
            if (IsEmbeddedResource(file))
            {
                contents = ResourceLoader.GetResource(file);
                if (contents == null) return false;
            }
            else
            {
                if (!FileReader.DoesFileExist(file) && !file.EndsWith(".less"))
                {
                    file = file + ".less";
                }

                if (!FileReader.DoesFileExist(file))
                {
                    return false;
                }

                contents = FileReader.GetFileContents(file);
            }

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
        ///  Imports a css file from an embedded resource and puts the contents into the import node
        /// </summary>
        /// <param name="file"></param>
        /// <param name="import"></param>
        /// <returns></returns>
        private bool ImportEmbeddedCssContents(string file, Import import)
        {
            string content = ResourceLoader.GetResource(file);
            if (content == null) return false;
            import.InnerContent = content;
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

    /// <summary>
    /// Utility class used to retrieve the content of an embedded resource using a separate app domain in order to unload the assembly when done.
    /// </summary>
    class ResourceLoader : MarshalByRefObject
    {
        private string _assemblyName;
        private string _resourceName;
        private string _resourceContent;

        /// <summary>
        /// Gets the text content of an embedded resource.
        /// </summary>
        /// <param name="file">The path in the form: dll://AssemblyName/ResourceName</param>
        /// <returns>The content of the resource</returns>
        public static string GetResource(string file)
        {
            var match = Importer.EmbeddedResourceRegex.Match(file);
            if (!match.Success) return null;

            var loader = new ResourceLoader();
            loader._resourceName = match.Groups["Resource"].Value;
            loader._assemblyName = match.Groups["Assembly"].Value;

            try
            {
                var domainSetup = new AppDomainSetup();
                domainSetup.ApplicationBase = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var domain = AppDomain.CreateDomain("LoaderDomain", null, domainSetup);
                domain.DoCallBack(loader.LoadResource);
                AppDomain.Unload(domain);
            }
            catch (Exception) 
            {
                throw new FileNotFoundException("Unable to load resource [" + loader._resourceName + "] in assembly [" + loader._assemblyName + "]");
            }

            return loader._resourceContent;
        }

        // Runs in the separate app domain
        private void LoadResource()
        {
            var assembly = Assembly.ReflectionOnlyLoadFrom(_assemblyName);
            _resourceContent = new StreamReader(assembly.GetManifestResourceStream(_resourceName)).ReadToEnd();
        }
    }
}