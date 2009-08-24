using System;

namespace nless.Core.parser
{
    internal class ParsingException : Exception
    {
        public ParsingException(string s) : base(s)
        {
        }
    }
}