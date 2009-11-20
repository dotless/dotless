using dotless.Core.engine;

namespace dotless.Core.parser
{
    public interface ILessParser
    {
        Element Parse(string source);
    }
}