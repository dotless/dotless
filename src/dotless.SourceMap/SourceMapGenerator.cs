namespace dotless.SourceMapping {
    ///<summary>
    /// Simple factory to get a source map
    ///</summary>
    public static class SourceMapGenerator {
        ///<summary>
        /// creates a source map for a given filename
        ///</summary>
        ///<param name="filename">the filename you want to generate the sourcemap for</param>
        ///<returns>a new sourcemap for that file</returns>
        public static SourceMap CreateMapForFile(string filename) {
            return new SourceMap(filename, new SourceMap.Configuration{Debug = false});
        }
    }
}
