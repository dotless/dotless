namespace dotless.Core.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using dotless.Core.Loggers;

    public class TestLogger : Logger
    {
        public List<string> LogMessages { get; set; }

        public TestLogger(LogLevel logLevel) : base(logLevel)
        {
            LogMessages = new List<string>();
        }

        protected override void Log(string message)
        {
            LogMessages.Add(message);
        }
    }
}
