namespace dotless.SourceMapping {
    /// <summary>
    /// structure to combine all information about a generated code fragment
    /// </summary>
    public class SourceFragment {
        /// <summary>
        /// the source file where the fragment originated from
        /// </summary>
        public string SourceFile { get; set; }

        /// <summary>
        /// the fragment's line number within the source file
        /// </summary>
        public int SourceLine { get; set; }

        /// <summary>
        /// the columnnumber if the fragment within the source file
        /// </summary>
        public int SourceColumn { get; set; }

        /// <summary>
        /// the code fragment's line in the generated code
        /// </summary>
        public int GeneratedLine { get; set; }
        /// <summary>
        /// the column where to find the fragment in the generated code
        /// </summary>
        public int GeneratedColumn { get; set; }
        /// <summary>
        /// An optional original token name for this mapping. (unsupported)
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// creates a string representation of the data contained in this fragment - mostly used for debugging purposes
        /// </summary>
        /// <returns>a string representing the data contained in this source-code-fragment</returns>
        public override string ToString() {
            return string.Format("Gen({0}:{1}); {2}:Src({3}:{4});", 
                                  GeneratedLine, 
                                  GeneratedColumn,
                                  SourceFile,
                                  SourceLine,
                                  SourceColumn);
        }
    }
}