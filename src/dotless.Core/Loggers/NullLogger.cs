namespace dotless.Core.Loggers
{
    public class NullLogger : Logger
    {
        public NullLogger(LogLevel level) : base(level)
        {
        }

        protected override void Log(string message)
        {
            //Swallow
        }

        private static readonly NullLogger instance = new NullLogger(LogLevel.Warn);
        public static NullLogger Instance
        {
            get
            {
                return instance;
            }
        }
    }
}