namespace dotless.Core.Response
{
    using Abstractions;
    using System.IO.Compression;

    public class CssResponse : IResponse
    {
        public readonly IHttp Http;

        public CssResponse(IHttp http, bool isCompressionHandledByResponse)
        {
            Http = http;
            IsCompressionHandledByResponse = isCompressionHandledByResponse;
        }

        /// <summary>
        ///  Whether to handle the Accept-Encoding compression
        /// </summary>
        public bool IsCompressionHandledByResponse
        {
            get;
            set;
        }

        public virtual void WriteHeaders()
        {
            if (IsCompressionHandledByResponse)
            {
                HandleCompression();
            }

            Http.Context.Response.ContentType = "text/css";
        }

        public virtual void WriteCss(string css)
        {
            Http.Context.Response.Write(css);
        }

        /// <summary>
        ///  deal with the request accept encoding and add the necessary filter to the response
        /// </summary>
        protected void HandleCompression()
        {
            var context = Http.Context;

            // load encodings from header
            QValueList encodings = new QValueList(context.Request.Headers["Accept-Encoding"]);

            // get the types we can handle, can be accepted and
            // in the defined client preference
            QValue preferred = encodings.FindPreferred("gzip", "deflate", "identity");

            // if none of the preferred values were found, but the
            // client can accept wildcard encodings, we'll default
            // to Gzip.
            if (preferred.IsEmpty && encodings.AcceptWildcard && encodings.Find("gzip").IsEmpty)
            {
                preferred = new QValue("gzip");
            }

            if (preferred.Name != null)
            {
                // handle the preferred encoding
                switch (preferred.Name.ToLowerInvariant())
                {
                    case "gzip":
                        context.Response.AppendHeader("Content-Encoding", "gzip");
                        context.Response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress);
                        break;
                    case "deflate":
                        context.Response.AppendHeader("Content-Encoding", "deflate");
                        context.Response.Filter = new DeflateStream(context.Response.Filter, CompressionMode.Compress);
                        break;
                    case "identity":
                    default:
                        break;
                }
            }
        }
    }
}