namespace dotless.Core.Loggers
{
    public abstract class Logger : ILogger
    {
        public LogLevel Level { get; set; }

        protected Logger(LogLevel level)
        {
            Level = level;
        }

        public void Log(LogLevel level, string message)
        {
            if (Level <= level)
                Log(message);
        }

        protected abstract void Log(string message);

        public void Info(string message) { Log(LogLevel.Info, message); }
        public void Debug(string message) { Log(LogLevel.Debug, message); }
        public void Warn(string message) { Log(LogLevel.Warn, message); }
        public void Error(string message) { Log(LogLevel.Error, message); }
    }
}