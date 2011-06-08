namespace dotless.Core.Parser.Infrastructure
{
    using Tree;

    public class NamedArgument
    {
        public string Name { get; set; }
        public Expression Value { get; set; }
    }
}