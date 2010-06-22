namespace dotless.Core.Exceptions
{
    using System;

    public class ParsingException : Exception
    {
        public int Index { get; set; }
        public int Call { get; set; }

        public ParsingException(string message, int index) : this(message, null, index, 0) {}

        public ParsingException(string message, int index, int call) : this(message, null, index, call) {}

        public ParsingException(Exception innerException, int index) : this(innerException, index, 0) {}

        public ParsingException(Exception innerException, int index, int call) : this(innerException.Message, innerException, index, call) {}

        public ParsingException(string message, Exception innerException, int index, int call) : base(message, innerException)
        {
          Index = index;
          Call = call;
        }
    }
}