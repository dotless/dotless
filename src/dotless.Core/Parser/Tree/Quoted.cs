namespace dotless.Core.Parser.Tree
{
    using System.Text.RegularExpressions;
    using Infrastructure;
    using Infrastructure.Nodes;

    public class Quoted : TextNode
    {
        public char? Quote { get; set; }

        public Quoted(string value, string contents)
          : base(contents)
        {
            Quote = value[0];
        }

        public Quoted(string value)
            : this(value, value)
        {
            Quote = null;
        }

        public override string ToCSS(Env env)
        {
            return Quote + Value + Quote;
        }

        private readonly Regex _unescape = new Regex(@"(^|[^\\])\\(.)");

        public string UnescapeContents()
        {
            return _unescape.Replace(Value, @"$1$2");
        }
    }
}