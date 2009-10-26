using System;

namespace dotless.Core.exceptions
{
    internal class ParsingException : Exception
    {
        public ParsingException(string s) : base(s)
        {
        }
    }
}