#define rawMode
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace dotless.SourceMapping {

    /// <summary>
    /// a class to hold a source map for a generated file. For the internal structure see:
    /// https://docs.google.com/document/d/1U1RGAehQwRypUTovF1KRlpiOFze0b-_2gc6fAH0KY0k/edit#
    /// </summary>
    public class SourceMap {
        /// <summary>
        /// simple class to hold some configuration to control the way the source mapping handle's things
        /// </summary>
        public class Configuration {            
            /// <summary>
            /// the debug flag will producure the mapping as indented json for better readability
            /// </summary>
            public bool Debug { get; set; }

            /// <summary>
            /// callback handler that is used to clean up files used during includes - it need to provide nice path-names for the given files
            /// </summary>
            public ProcessFileHandler OnProcessFilename { get; set; }

            /// <summary>
            /// the name of the file that is being generated...
            /// </summary>
            public string OutputFilename { get; set; }
        }

        /// <summary>
        /// the configuration used by the source map to control it's behavious
        /// </summary>
        public Configuration Config { get; private set; }

        /// <summary>
        /// an internal list of all known generated source code fragments
        /// </summary>
        protected List<SourceFragment> Fragments;

        /// <summary>
        /// a handler to the debug interface
        /// </summary>
        protected IDebugInterface Debug;


        ///<summary>
        /// ctor to create a source map with a default config (debug will be disabled)
        ///</summary>
        ///<param name="outputFilename">the file (generated ouput file) you are creating a mapping for</param>
        public SourceMap(string outputFilename) : this(outputFilename, new Configuration{ Debug = false, OutputFilename = outputFilename}) {
        }
        /// <summary>
        /// creates a new source map
        /// </summary>
        /// <param name="outputFilename">the generated file's name</param>
        /// <param name="config">the config that controlls the source map's behavior</param>
        public SourceMap(string outputFilename, Configuration config) {
            this.Config = config;
            this.Config.OutputFilename = outputFilename;
            this.Fragments = new List<SourceFragment>();
        }

        /// <summary>
        /// generates the source map as json representation and emmits it into the given stream
        /// </summary>
        /// <param name="stream">the stream to write the source map data to</param>
        public void GenerateSourceMap(Stream stream) {
            var data = new SourceMapDataStructure{File = this.Config.OutputFilename};

            
            #region sort data
            // sort fragments by line first then by column)
            this.Fragments.Sort((item1, item2) => {
                int cmp = item1.GeneratedLine - item2.GeneratedLine;
                // if they are both on the same line compare the columns
                return Math.Sign(cmp == 0 ? (item1.GeneratedColumn.CompareTo(item2.GeneratedColumn)) : cmp);
            });
            #endregion
            
            #region files & names
            // get all known source files and push into the data structure
            data.Sources = (from SourceFragment fragment in this.Fragments
                            where !String.IsNullOrEmpty(fragment.SourceFile)
                            select this.CleanUpFileName(fragment.SourceFile)).Distinct().ToArray();

            // get list of named code fragments
            data.Names   = (from SourceFragment fragment in this.Fragments
                            where !String.IsNullOrEmpty(fragment.Name)
                            select fragment.Name).Distinct().ToArray();
            #endregion

            #region mapping

            
            #if rawMode
            var rawMapping = new StringBuilder();
            #else
            var line = new List<Mapping>();
            var map  = new List<IEnumerable<Mapping>>();
            #endif
            // a ref to the previously processed fragment
            
            int  prevGeneratedLine  = 1;

            int prevSourceIndex     = 0;
            int prevGeneratedColumn = 0;
            
            int prevSourceLine      = 0;
            int prevSourceColumn    = 0;
            int prevNameIndex       = -1;

            foreach (var fragment in Fragments) {                                
                if (fragment.GeneratedLine > prevGeneratedLine) {
                    // reset column
                    prevGeneratedColumn = 0;
                    
                    while (prevGeneratedLine <= fragment.GeneratedLine) {
                        // add current line to the mapping
                        #if rawMode
                        rawMapping.Append(";");
                        #else
                        map.Add(line);                        
                        // create new list of lines
                        line = new List<Mapping>();
                        #endif 
                        prevGeneratedLine++;
                    }
                } 
                #if rawMode
                else if (fragment != Fragments.First()) {
                    rawMapping.Append(",");
                }
                #endif

                var cleanName = CleanUpFileName(fragment.SourceFile);
                var sourceIndex = Array.IndexOf(data.Sources, cleanName);
                
                int indx = Array.IndexOf(data.Names, fragment.Name);
                
                int? nameIndex = null;
                if (indx != -1)
                    nameIndex = indx - prevNameIndex;
                
                System.Diagnostics.Debug.WriteLine(String.Format("gen.addMapping({{ generated : {{ line : {0}, column : {1}}},  original : {{ line : {2}, column : {3} }},  source : \"{4}\", name : \"\"}}); //{5}",
                                                   fragment.GeneratedLine, fragment.GeneratedColumn, fragment.SourceLine, fragment.SourceColumn, CleanUpFileName(fragment.SourceFile), sourceIndex));
                    
                if (this.Debug != null) {

                        
                    this.Debug.WriteLine(String.Format("{4}-{0}-{1}-{2}-{3}", fragment.GeneratedColumn, fragment.SourceLine, fragment.SourceColumn, sourceIndex, fragment.GeneratedLine));
                    this.Debug.WriteLine(String.Format("gCol: {0}",  fragment.GeneratedColumn - prevGeneratedColumn));
                    this.Debug.WriteLine(String.Format("sFile: {0}",  sourceIndex              - prevSourceIndex));
                    this.Debug.WriteLine(String.Format("sLine: {0}",  fragment.SourceLine      - 1 - prevSourceLine));
                    this.Debug.WriteLine(String.Format("sCol: {0}",  fragment.SourceColumn    - prevSourceColumn));
                    this.Debug.WriteLine(String.Empty);
                }

                var mapping = new Mapping(fragment.GeneratedColumn - prevGeneratedColumn,
                                          fragment.SourceColumn    - prevSourceColumn,                      
                                          // -1 due to version 3
                                          fragment.SourceLine      - 1 - prevSourceLine,
                                          sourceIndex              - prevSourceIndex,                                          
                                          nameIndex);

                System.Diagnostics.Debug.WriteLine(mapping.ToString());
                     
                #if rawMode                
                    rawMapping.Append(mapping.Encode());
                #else
                    line.Add(mapping);
                #endif                
                
                prevGeneratedColumn = fragment.GeneratedColumn;
                prevSourceColumn    = fragment.SourceColumn;
                prevSourceLine      = fragment.SourceLine - 1;
                
                prevSourceIndex     = sourceIndex;
                              

                if (nameIndex != null)
                    prevNameIndex = (int) nameIndex;
            }
            #endregion

            #region generate mapping            
            #if !rawMode
            
            // add last row if there is any data
            if (line.Any())
                map.Add(line);            
            

            // merge mapping down into the format chunk,chunk,chunk;Line,Chunk,Chunk;
            data.Mappings = String.Join(";", (from IEnumerable<Mapping> aLine in map 
                                              select String.Join(",", (from Mapping aMap in aLine
                                                                       select aMap.Encode()))));
            #else
            data.Mappings = rawMapping.ToString();
            #endif
            #endregion

            #region output
            // take a writer and pump it into the stream)
            try {
                var writer = new StreamWriter(stream);
                
                string raw = JsonConvert.SerializeObject(data, Config.Debug ? Formatting.Indented : Formatting.None);
                writer.Write(raw);
                writer.Flush();
            } catch(Exception ex) {
                new SourceMapException("There was an error producing the json from the given data! See inner exeception for more details!", ex);
            }
            #endregion
        }

        /// <summary>
        /// helper method to invoke a callback that cleans up the file paths for a given source files
        /// </summary>
        /// <param name="sourceFile">a given source file</param>
        /// <returns>the cleaned-up system path for that source file</returns>
        private string CleanUpFileName(string sourceFile) {
            if (Config.OnProcessFilename != null) {
                var args = new ProcessFilenameArgs{ Filename = sourceFile };
                Config.OnProcessFilename(this, args);
                return args.Filename;
            }
            return sourceFile;
        }

        /// <summary>
        /// get the relative of a given expression or the current's value if there is no prev
        /// </summary>
        /// <param name="currentFragment">the current value</param>
        /// <param name="prevAddress">the previous address (or null)</param>
        /// <param name="exp">the expression returning the property of intrest</param>
        /// <returns>the delta number between the current and the previous fragmend as defined by exp or the whole numver of the current fragment (which should be 0 in most cases!)</returns>
        protected int GetRelativeAddress(SourceFragment currentFragment, int? prevAddress, Func<SourceFragment, int> exp) {
            return prevAddress != null ? exp(currentFragment) - (int) prevAddress : exp(currentFragment);
        }


        /// <summary>
        /// adds a code framgent to the source map
        /// </summary>
        /// <param name="fragment">the fragmet containing all information about a piece of generated code</param>
        public void AddSourceFragment(SourceFragment fragment) {
            this.Fragments.Add(fragment);
        }

        /// <summary>
        /// generates the string / json representation of the source map to export it to a browser
        /// </summary>
        /// <returns>the source map a json</returns>
        public string GenerateSourceMap() {
            using(var memStream = new MemoryStream()) {
                // generats the map into the mem stream
                this.GenerateSourceMap(memStream);

                // rewind
                memStream.Position = 0;

                // create a reader to get the mem stream's content
                using(var reader = new StreamReader(memStream))
                    // get it & return it
                    return reader.ReadToEnd();
            }
                
        }
    }

    /// <summary>
    /// a little interface to get some debug data during generation
    /// </summary>
    public interface IDebugInterface {
        /// <summary>
        /// log a message to the debug
        /// </summary>
        /// <param name="msg">the message to log</param>
        void WriteLine(string msg);
    }

    /// <summary>
    /// little helper class to aggregate all data needed for a code mapping
    /// </summary>
    public class Mapping {
        /// <summary>
        /// the column the code has been generated to (as relative delta to the previous mapping)
        /// </summary>
        public int GeneratedColumn { get; set; }

        /// <summary>
        /// the relative location of the column to the previous source column
        /// </summary>
        public int SourceColumn { get; set; }

        /// <summary>
        /// the source line relative to the prevous entry
        /// </summary>
        public int SourceLine { get; set; }

        /// <summary>
        /// the index of the mapping's name if there is one - if the value is 0 it is ommitted
        /// </summary>
        public int? NameIndex { get; set; }

        /// <summary>
        /// the index of the source file relative to it's previous occurence
        /// </summary>
        public int SourceFileIndex { get; set; }

        ///<summary>
        /// ctor to create a source mapping
        ///</summary>
        ///<param name="generatedColumn">the generated column of the frgment</param>
        ///<param name="sourceColumn">the original column the fragement originated from</param>
        ///<param name="sourceLine">the original source line the frament originated from</param>
        ///<param name="sourceFileIndex">the index of the source-file the fragment originated from</param>
        ///<param name="nameIndex">the index of the fragment's name in the name-list (no yet supported)</param>
        public Mapping(int generatedColumn, int sourceColumn, int sourceLine, int sourceFileIndex, int? nameIndex) {
            GeneratedColumn = generatedColumn;
            SourceColumn = sourceColumn;
            SourceLine = sourceLine;
            NameIndex = nameIndex;
            SourceFileIndex = sourceFileIndex;
        }

        /// <summary>
        /// this method takes care about the map encoding as Base64Vlq-Numbers
        /// </summary>
        /// <returns>the variable base64-encoded version of the data contained in this mapping</returns>
        public string Encode() {
            // magic goes here
            return String.Format("{0}{1}{2}{3}{4}", Base64Vlq.Encode(this.GeneratedColumn), 
                                                    Base64Vlq.Encode(this.SourceFileIndex),
                                                    Base64Vlq.Encode(this.SourceLine),
                                                    Base64Vlq.Encode(this.SourceColumn),
                                                    this.NameIndex != null ? Base64Vlq.Encode((int) this.NameIndex) : String.Empty);
        }

        public string ToString() {
            // magic goes here
            return String.Format("GenCol:{0}, SrcIndx:{1}, SrcLine:{2}, SrcCol:{3}, NameIndx{4}", 
                                  this.GeneratedColumn, 
                                  this.SourceFileIndex,
                                  this.SourceLine,
                                  this.SourceColumn,
                                  this.NameIndex);
        }
    }

    /// <summary>
    /// exception thrown when something goes wrong during map generation
    /// </summary>
    public class SourceMapException : Exception {
        ///<summary>
        /// ctor to create an exception during the map-handling
        ///</summary>
        ///<param name="message">the message displayed to the user</param>
        ///<param name="ex">the inner exception - if there is one</param>
        public SourceMapException(string message, Exception ex) : base(message, ex) {}
    }

    /// <summary>
    /// structure for the callback cleaning up the found filenames
    /// </summary>
    /// <param name="sender">this</param>
    /// <param name="args">arguments needed to clean up the retrieved filenames</param>
    public delegate void ProcessFileHandler(object sender, ProcessFilenameArgs args);

    /// <summary>
    /// structure caonting the information to cleanup the filenames found in a sourcemap
    /// </summary>
    public class ProcessFilenameArgs {
        /// <summary>
        /// the filename found in a sourcemap. Change it to match your requirements
        /// </summary>
        public string Filename { get; set; }
    }
}
