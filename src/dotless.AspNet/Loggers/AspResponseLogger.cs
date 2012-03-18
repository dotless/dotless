namespace dotless.Core.Loggers
{
    using Response;

    public class AspResponseLogger : Logger
    {
        public IResponse Response { get; set; }

        public AspResponseLogger(LogLevel level, IResponse response) : base(level)
        {
            Response = response;
        }

        protected override void Log(string message)
        {
            Response.WriteCss(message);
        }
    }
}