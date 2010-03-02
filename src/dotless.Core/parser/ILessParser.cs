using dotless.Core.engine;

namespace dotless.Core.parser
{
    public interface ILessParser
    {
        ElementBlock Parse(string source);
        ElementBlock Parse(string source, ElementBlock tail);
    }
}