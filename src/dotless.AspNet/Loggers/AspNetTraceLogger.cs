namespace dotless.Core.Loggers
{
    using Abstractions;

    public class AspNetTraceLogger : Logger
    {
        private readonly IHttp http;

        public AspNetTraceLogger(LogLevel level, IHttp http) : base(level)
        {
            this.http = http;
        }

        protected override void Log(string message)
        {
            http.Context.Trace.Write(message);
        }
    }
}