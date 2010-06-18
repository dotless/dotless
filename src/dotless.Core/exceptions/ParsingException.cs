namespace dotless.Core.Exceptions
{
    using System;

    public class ParsingException : Exception
    {
        public int Index { get; set; }

        public ParsingException(string message, int index)
            : base(message)
        {
            Index = index;
        }

        public ParsingException(Exception innerException, int index)
            : this(innerException.Message, innerException, index)
        {
        }

        public ParsingException(string message, Exception innerException, int index)
            : base(message, innerException)
        {
            Index = index;
        }
    }
}