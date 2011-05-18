using dotless.Core.Parser.Tree;

namespace dotless.Core.Parser.Infrastructure
{
    public class NamedArgument
    {
        public string Name { get; set; }
        public Expression Value { get; set; }
    }
}