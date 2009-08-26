using System;

namespace nless.Core.Exceptions
{
    internal class ParsingException : Exception
    {
        public ParsingException(string s) : base(s)
        {
        }
    }
}