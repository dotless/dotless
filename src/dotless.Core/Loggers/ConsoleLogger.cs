namespace dotless.Core.Loggers
{
    using System;

    public class ConsoleLogger : Logger
    {
        public ConsoleLogger(LogLevel level) : base(level) {}

        protected override void Log(string message)
        {
            Console.Write(message);
        }
    }
}