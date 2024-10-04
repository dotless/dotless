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
        private static readonly Regex _embeddedResourceRegex = new Regex(@"^dll://(?<Assembly>.+?)#(?<Resource>.+)$");

        public static Regex EmbeddedResourceRegex { get { return _embeddedResourceRegex; } }
        public IFileReader FileReader { get; set; }
        
        /// <summary>
        ///  List of successful imports
        /// </summary>
        public List<string> Imports { get; set; }
        
        public Func<Parser> Parser { get; set; }
        private readonly List<string> _paths = new List<string>();

        /// <summary>
        ///  The raw imports of every @import node, for use with @import
        /// </summary>
        protected readonly List<string> _rawImports = new List<string>();

        /// <summary>
        /// Duplicates of reference imports should be ignored just like normal imports
        /// but a reference import must not interfere with a regular import, hence a different list
        /// </summary>
        private readonly List<string> _referenceImports = new List<string>();

        public virtual string CurrentDirectory { get; set; }

        /// <summary>
        ///  Whether or not the importer should alter urls
        /// </summary>
        public bool IsUrlRewritingDisabled { get; set; }

        public string RootPath { get; set; }

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

        public Importer(IFileReader fileReader) : this(fileReader, false, "", false, false)
        {
        }

        public Importer(IFileReader fileReader, bool disableUrlReWriting, string rootPath, bool inlineCssFiles, bool importAllFilesAsLess)
        {
            FileReader = fileReader;
            IsUrlRewritingDisabled = disableUrlReWriting;
            RootPath = rootPath;
            InlineCssFiles = inlineCssFiles;
            ImportAllFilesAsLess = importAllFilesAsLess;
            Imports = new List<string>();
            CurrentDirectory = "";
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
        ///  returns true if the import should be ignored because it is a duplicate and import-once was used
        /// </summary>
        /// <param name="import"></param>
        /// <returns></returns>
        protected bool CheckIgnoreImport(Import import)
        {
            return CheckIgnoreImport(import, import.Path);
        }

        /// <summary>
        ///  returns true if the import should be ignored because it is a duplicate and import-once was used
        /// </summary>
        /// <param name="import"></param>
        /// <returns></returns>
        protected bool CheckIgnoreImport(Import import, string path)
        {
            if (IsOptionSet(import.ImportOptions, ImportOptions.Multiple))
            {
                return false;
            }

            // The import option Reference is set at parse time,
            // but the IsReference bit is set at evaluation time (inherited from parent)
            // so we check both.
            if (import.IsReference || IsOptionSet(import.ImportOptions, ImportOptions.Reference))
            {
                if (_rawImports.Contains(path, StringComparer.InvariantCultureIgnoreCase))
                {
                    // Already imported as a regular import, so the reference import is redundant
                    return true;
                }

                return CheckIgnoreImport(_referenceImports, path);
            }
            
            return CheckIgnoreImport(_rawImports, path);
        }

        private bool CheckIgnoreImport(List<string> importList, string path) {
            if (importList.Contains(path, StringComparer.InvariantCultureIgnoreCase))
            {
                return true;
            }
            importList.Add(path);
            return false;
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
                    throw new FileNotFoundException(string.Format(".less cannot import non local less files [{0}].", import.Path), import.Path);
                }

                if (CheckIgnoreImport(import))
                {
                    return ImportAction.ImportNothing;
                }

                return ImportAction.LeaveImport;
            }

            var file = import.Path;
            
            if (!IsNonRelativeUrl(file)) 
            {
                file = GetAdjustedFilePath(import.Path, _paths);
            }

            if (CheckIgnoreImport(import, file))
            {
                return ImportAction.ImportNothing;
            }

            bool importAsless = ImportAllFilesAsLess || IsOptionSet(import.ImportOptions, ImportOptions.Less);

            if (!importAsless && import.Path.EndsWith(".css") && !import.Path.EndsWith(".less.css"))
            {
                if (InlineCssFiles || IsOptionSet(import.ImportOptions, ImportOptions.Inline))
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
                if (IsOptionSet(import.ImportOptions, ImportOptions.Optional))
                {
                    return ImportAction.ImportNothing;
                }

                if (IsOptionSet(import.ImportOptions, ImportOptions.Css))
                {
                    return ImportAction.LeaveImport;
                }

                if (import.Path.EndsWith(".less", StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new FileNotFoundException(string.Format("You are importing a file ending in .less that cannot be found [{0}].", file), file);
                }
                return ImportAction.LeaveImport;
            }

            return ImportAction.ImportLess;
        }

        public IDisposable BeginScope(Import parentScope) {
            return new ImportScope(this, Path.GetDirectoryName(parentScope.Path));
        }

        /// <summary>
        ///  Uses the paths to adjust the file path
        /// </summary>
        protected string GetAdjustedFilePath(string path, IEnumerable<string> pathList)
        {
            return pathList.Concat(new[] { path }).AggregatePaths(CurrentDirectory);
        }

        /// <summary>
        ///  Imports a less file and puts the root into the import node
        /// </summary>
        protected bool ImportLessFile(string lessPath, Import import)
        {
            string contents, file = null;
            if (IsEmbeddedResource(lessPath))
            {
                contents = ResourceLoader.GetResource(lessPath, FileReader, out file);
                if (contents == null) return false;
            }
            else
            {
                string fullName = lessPath;
                if (Path.IsPathRooted(lessPath))
                {
                    fullName = lessPath;
                }
                else if (!string.IsNullOrEmpty(CurrentDirectory)) {
                    fullName = CurrentDirectory.Replace(@"\", "/").TrimEnd('/') + '/' + lessPath;
                }

                bool fileExists = FileReader.DoesFileExist(fullName);
                if (!fileExists && !fullName.EndsWith(".less"))
                {
                    fullName += ".less";
                    fileExists = FileReader.DoesFileExist(fullName);
                }

                if (!fileExists) return false;

                contents = FileReader.GetFileContents(fullName);

                file = fullName;
            }

            _paths.Add(Path.GetDirectoryName(import.Path));

            try
            {
                if (!string.IsNullOrEmpty(file))
                {
                    Imports.Add(file);
                }
                import.InnerRoot = Parser().Parse(contents, lessPath);
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
            string content = ResourceLoader.GetResource(file, FileReader, out file);
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
            Imports.Add(file);

            return true;
        }

        /// <summary>
        ///  Called for every Url and allows the importer to adjust relative url's to be relative to the
        ///  primary url
        /// </summary>
        public string AlterUrl(string url, List<string> pathList)
        {
            if (!IsProtocolUrl (url) && !IsNonRelativeUrl (url))
            {
                if (pathList.Any() && !IsUrlRewritingDisabled)
                {
                    url = GetAdjustedFilePath(url, pathList);
                }
                return RootPath + url;
            }
            return url;
        }

        public void ResetImports()
        {
            Imports.Clear();
            _rawImports.Clear();
            _referenceImports.Clear();
        }

        public IEnumerable<string> GetImports()
        {
            return Imports.Distinct();
        }

        private class ImportScope : IDisposable {
            private readonly Importer importer;

            public ImportScope(Importer importer, string path) {
                this.importer = importer;
                this.importer._paths.Add(path);
            }

            public void Dispose() {
                this.importer._paths.RemoveAt(this.importer._paths.Count - 1);
            }
        }
		
        private bool IsOptionSet(ImportOptions options, ImportOptions test)
        {
            return (options & test) == test;
        }		
    }

    /// <summary>
    /// Utility class used to retrieve the content of an embedded resource using a separate app domain in order to unload the assembly when done.
    /// </summary>
    class ResourceLoader : MarshalByRefObject
    {
        private byte[] _fileContents;
        private string _resourceName;
        private string _resourceContent;

        /// <summary>
        /// Gets the text content of an embedded resource.
        /// </summary>
        /// <param name="file">The path in the form: dll://AssemblyName#ResourceName</param>
        /// <returns>The content of the resource</returns>
        public static string GetResource(string file, IFileReader fileReader, out string fileDependency)
        {
            fileDependency = null;

            var match = Importer.EmbeddedResourceRegex.Match(file);
            if (!match.Success) return null;

            var loader = new ResourceLoader
            {
                _resourceName = match.Groups["Resource"].Value
            };

            try
            {
                fileDependency = match.Groups["Assembly"].Value;

                LoadFromCurrentAppDomain(loader, fileDependency);

                if (String.IsNullOrEmpty(loader._resourceContent))
                    LoadFromNewAppDomain(loader, fileReader, fileDependency);
                
            }
            catch (Exception)
            {
                throw new FileNotFoundException("Unable to load resource [" + loader._resourceName + "] in assembly [" + fileDependency + "]");
            }
            finally
            {
                loader._fileContents = null;
            }

            return loader._resourceContent;
        }

        private static void LoadFromCurrentAppDomain(ResourceLoader loader, String assemblyName)
        {
            foreach (var assembly in AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(x => !IsDynamicAssembly(x) && x.Location.EndsWith(assemblyName, StringComparison.InvariantCultureIgnoreCase)))
            {
                if (assembly.GetManifestResourceNames().Contains(loader._resourceName))
                {
                    using (var stream = assembly.GetManifestResourceStream(loader._resourceName))
                    using (var reader = new StreamReader(stream))
                    {
                        loader._resourceContent = reader.ReadToEnd();

                        if (!String.IsNullOrEmpty(loader._resourceContent))
                            return;
                    }
                }
            }
        }

        private static bool IsDynamicAssembly(Assembly assembly) {
            try {
                string loc = assembly.Location;
                return false;
            } catch (NotSupportedException) {
                // Location is not supported for dynamic assemblies, so it will throw a NotSupportedException
                return true;
            }
        }

        private static void LoadFromNewAppDomain(ResourceLoader loader, IFileReader fileReader, String assemblyName)
        {
            if (!fileReader.DoesFileExist(assemblyName))
            {
                throw new FileNotFoundException("Unable to locate assembly file [" + assemblyName + "]");
            }

            loader._fileContents = fileReader.GetBinaryFileContents(assemblyName);
            
            var domain = AppDomain.CreateDomain("LoaderDomain");
            var assembly = domain.Load(loader._fileContents);

            using (var stream = assembly.GetManifestResourceStream(loader._resourceName))
            using (var reader = new StreamReader(stream))
            {
                loader._resourceContent = reader.ReadToEnd();
            }

            AppDomain.Unload(domain);
        }  
    }
}