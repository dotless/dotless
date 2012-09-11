namespace dotless.Core.Response
{
    public interface IResponse
    {
        /// <summary>
        ///  Write the css out and set any required response headers
        /// </summary>
        void WriteCss(string css);

        /// <summary>
        ///  Write out any header information
        /// </summary>
        void WriteHeaders();

        /// <summary>
        ///  Whether this IResponse should handle the compression request (e.g. Accept-Encoding)
        /// </summary>
        bool IsCompressionHandledByResponse { get; set; }
    }
}