namespace dotless.Core.Loggers
{
    public interface ILogger
    {
        void Log(LogLevel level, string message);
        void Info(string message);
        void Debug(string message);
        void Warn(string message);
        void Error(string message);
    }

    public enum LogLevel
    {
        Info = 1,
        Debug,
        Warn,
        Error
    }
}