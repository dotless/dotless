namespace dotless.Core.Parser.Infrastructure.Nodes
{
    public class CharMatchResult : TextNode
    {
        public char Char { get; set; }

        public CharMatchResult(char c) : base(c.ToString())
        {
            Char = c;
        }
    }
}