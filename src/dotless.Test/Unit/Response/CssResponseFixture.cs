namespace dotless.Test.Unit.Response
{
    using System.Web;
    using Core.Response;
    using Moq;
    using NUnit.Framework;
    using System.IO.Compression;

    public class CssResponseFixture : HttpFixtureBase
    {
        CssResponse CssResponse { get; set; }
        CssResponse CompressedCssResponse { get; set; }

        [SetUp]
        public void Setup()
        {
            CssResponse = new CssResponse(Http.Object, false);
            CompressedCssResponse = new CssResponse(Http.Object, true);
        }

        private void RunCompressedResponse(string acceptEncoding)
        {
            RunCompressedResponse(acceptEncoding, true);
        }

        private void RunCompressedResponse(string acceptEncoding, bool include)
        {
            Headers.Clear();
            if (include)
            {
                Headers.Add("Accept-Encoding", acceptEncoding);
            }
            CompressedCssResponse.WriteHeaders();
            CompressedCssResponse.WriteCss("");
        }

        [Test]
        public void ContentTypeIsTextCss()
        {
            CssResponse.WriteHeaders();
            CssResponse.WriteCss(null);

            HttpResponse.VerifySet(r => r.ContentType = "text/css", Times.Once());
        }

        [Test]
        public void CssIsWrittenToResponse()
        {
            var str = "testing";
            CssResponse.WriteHeaders();
            CssResponse.WriteCss(str);

            HttpResponse.Verify(r => r.Write(str), Times.Once());
        }

        [Test]
        public void CompressionNoAcceptEncodingNull()
        {
            RunCompressedResponse("", false);

            HttpResponse.Verify(r => r.Write(""), Times.Once());
            HttpResponse.VerifySet(r => r.Filter = null, Times.Never());
        }

        [Test]
        public void CompressionNoAcceptEncodingEmpty()
        {
            RunCompressedResponse("");

            HttpResponse.Verify(r => r.Write(""), Times.Once());
            HttpResponse.VerifySet(r => r.Filter = null, Times.Never());
        }

        [Test]
        public void CompressionNoAcceptEncodingStrange()
        {
            RunCompressedResponse("fo'o,*'(=[]735");

            HttpResponse.Verify(r => r.Write(""), Times.Once());
            HttpResponse.VerifySet(r => r.Filter = null, Times.Never());
        }

        [Test]
        public void CompressionAcceptEncodingGzip()
        {
            RunCompressedResponse("gzip");

            HttpResponse.Verify(r => r.Write(""), Times.Once());
            HttpResponse.VerifySet(r => r.Filter = It.IsAny<GZipStream>(), Times.Once());
        }

        [Test]
        public void CompressionAcceptEncodingGzip2()
        {
            RunCompressedResponse("gzip,deflate");

            HttpResponse.Verify(r => r.Write(""), Times.Once());
            HttpResponse.VerifySet(r => r.Filter = It.IsAny<GZipStream>(), Times.Once());
        }

        [Test]
        public void CompressionAcceptEncodingGzip3()
        {
            RunCompressedResponse("deflate;q=0.5,gzip");

            HttpResponse.Verify(r => r.Write(""), Times.Once());
            HttpResponse.VerifySet(r => r.Filter = It.IsAny<GZipStream>(), Times.Once());
        }

        [Test]
        public void CompressionAcceptEncodingGzip4()
        {
            RunCompressedResponse("deflate;q=0,gzip");

            HttpResponse.Verify(r => r.Write(""), Times.Once());
            HttpResponse.VerifySet(r => r.Filter = It.IsAny<GZipStream>(), Times.Once());
        }

        [Test]
        public void CompressionAcceptEncodingGzip5()
        {
            RunCompressedResponse("*");

            HttpResponse.Verify(r => r.Write(""), Times.Once());
            HttpResponse.VerifySet(r => r.Filter = It.IsAny<GZipStream>(), Times.Once());
        }

        [Test]
        public void CompressionAcceptEncodingNoGzip1()
        {
            RunCompressedResponse("deflate,gzip");

            HttpResponse.Verify(r => r.Write(""), Times.Once());
            HttpResponse.VerifySet(r => r.Filter = It.IsAny<DeflateStream>(), Times.Once());
        }

        [Test]
        public void CompressionAcceptEncodingNoGzip2()
        {
            RunCompressedResponse("identity,deflate,gzip");

            HttpResponse.Verify(r => r.Write(""), Times.Once());
            HttpResponse.VerifySet(r => r.Filter = It.IsAny<DeflateStream>(), Times.Never());
        }

        [Test]
        public void CompressionAcceptEncodingNoGzip3()
        {
            RunCompressedResponse("gzip;q=0");

            HttpResponse.Verify(r => r.Write(""), Times.Once());
            HttpResponse.VerifySet(r => r.Filter = It.IsAny<DeflateStream>(), Times.Never());
        }

    }
}