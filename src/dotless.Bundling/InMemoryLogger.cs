using System.Text;
using dotless.Core.Loggers;

namespace dotless.Bundling
{
    class InMemoryLogger : Logger
    {
        private readonly StringBuilder _errors = new StringBuilder();

        public InMemoryLogger(LogLevel logLevel)
            : base(logLevel)
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
}
