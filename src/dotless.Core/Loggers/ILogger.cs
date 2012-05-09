namespace dotless.Core.Loggers
{
    public interface ILogger
    {
        void Log(LogLevel level, string message);
        void Info(string message);
        void Info(string message, params object[] args);
        void Debug(string message);
        void Debug(string message, params object[] args);
        void Warn(string message);
        void Warn(string message, params object[] args);
        void Error(string message);
        void Error(string message, params object[] args);
    }

    public enum LogLevel
    {
        Info = 1,
        Debug,
        Warn,
        Error
    }
}