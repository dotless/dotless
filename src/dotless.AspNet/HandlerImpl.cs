namespace dotless.Core
{
    using Abstractions;
    using Input;
    using Response;

    public class HandlerImpl
    {
        public readonly IHttp Http;
        public readonly IResponse Response;
        public readonly ILessEngine Engine;
        public readonly IFileReader FileReader;

        public HandlerImpl(IHttp http, IResponse response, ILessEngine engine, IFileReader fileReader)
        {
            Http = http;
            Response = response;
            Engine = engine;
            FileReader = fileReader;
        }

        public void Execute()
        {
            Engine.CurrentDirectory = Http.Context.Server.MapPath("~");

            var localPath = Http.Context.Request.Url.LocalPath;

            var source = FileReader.GetFileContents(localPath);

            Response.WriteHeaders();
            Response.WriteCss(Engine.TransformToCss(source, localPath));
        }
    }
}