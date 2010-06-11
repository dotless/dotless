namespace dotless.Core
{
    using Abstractions;
    using Input;
    using Response;

    public class HandlerImpl
    {
        private readonly IHttp _http;
        private readonly IResponse _response;
        private readonly ILessEngine _engine;
        private readonly IFileReader _fileReader;

        public HandlerImpl(IHttp http, IResponse response, ILessEngine engine, IFileReader fileReader)
        {
            _http = http;
            _response = response;
            _engine = engine;
            _fileReader = fileReader;
        }

        public void Execute()
        {
            var localPath = _http.Context.Request.Url.LocalPath;

            var source = _fileReader.GetFileContents(localPath);

            _response.WriteCss(_engine.TransformToCss(source, localPath));
        }
    }
}