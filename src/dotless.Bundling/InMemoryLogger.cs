using System.Text;
using dotless.Core.Abstractions;
using dotless.Core.Loggers;

namespace dotless.Bundling
{
    public class InMemoryLogger : Logger
    {
        private readonly StringBuilder _errors = new StringBuilder();

        public InMemoryLogger(LogLevel level) : base(level)
        {
        }

        protected override void Log(string message)
        {
            _errors.Append(message);
        }

        public string GetOutput()
        {
            return _errors.ToString().Trim();
        }
    }

    public class BundlingResponseLogger : Logger
    {
        private readonly IHttp _context;

        public BundlingResponseLogger(LogLevel logLevel, IHttp context)
            : base(logLevel)
        {
            _context = context;
        }

        protected override void Log(string message)
        {
            _context.Context.Response.Write(message);
        }
    }
}
