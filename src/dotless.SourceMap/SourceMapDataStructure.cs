using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace dotless.SourceMapping {
    /// <summary>
    /// see: https://docs.google.com/document/d/1U1RGAehQwRypUTovF1KRlpiOFze0b-_2gc6fAH0KY0k/edit
    /// </summary>
    public class SourceMapDataStructure {
        /// <summary>
        /// File version (always the first entry in the object) and must be a positive integer.
        /// </summary>
        [JsonProperty("version")]
        public int Version {
            get { return 3; }
            set { if (value != 3) throw new InvalidVersionException(); }
        }

        /// <summary>
        /// The name of the generated code that this source map is associated with.
        /// </summary>
        [JsonProperty("file")]
        public string File {get; set; }

        /// <summary>
        /// An optional source root, useful for relocating source files on a server or removing repeated values in the “sources” entry.  This value is prepended to the individual entries in the “source” field. (Not really used in this implementation)
        /// </summary>
        [JsonProperty("sourceRoot")]
        [JsonIgnore]
        public string SourceRoot { get; set; }

        /// <summary>
        ///
        /// An optional list of source content, useful when the “source” can’t be hosted. (unspported)
        /// </summary>
        //todo: Create support for this
        [JsonProperty("sourcesContent")]
        [JsonIgnore]
        public String[] SourcesContent { get; set; }

        /// <summary>
        /// A list of original sources used by the “mappings” entry. 
        /// </summary>
        [JsonProperty("sources")]
        public String[] Sources { get; set; }


        /// <summary>
        /// A list of symbol names used by the “mappings” entry.
        /// </summary>
        //todo: Create support for this
        [JsonProperty("names")]
        public String[] Names { get; set; }

        /// <summary>
        /// A string with the encoded mapping data.
        /// </summary>
        [JsonProperty("mappings")]
        public String Mappings { get; set; }
    }

    ///<summary>
    /// exception throw when a different version then 3 is loaded by the structure
    ///</summary>
    public class InvalidVersionException : Exception {
        ///<summary>
        /// ctor - creating the exceptino with the message that only version 3 is supported
        ///</summary>
        public InvalidVersionException() : base("Only version 3 is supported!"){}
    }
}
