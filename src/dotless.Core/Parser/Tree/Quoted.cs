namespace dotless.Core.Parser.Tree
{
    using System.Text.RegularExpressions;
    using Infrastructure;
    using Infrastructure.Nodes;

    public class Quoted : TextNode
    {
        public char? Quote { get; set; }
        public bool Escaped { get; set; }

        public Quoted(string value, char? quote)
            : base(value)
        {
            Quote = quote;
        }

        public Quoted(string value, string contents, bool escaped)
            : base(contents)
        {
            Escaped = escaped;
            Quote = value[0];
        }

        public Quoted(string value, bool escaped)
            : base(value)
        {
            Escaped = escaped;
            Quote = null;
        }

        public override string ToCSS(Env env)
        {
            if(Escaped)
                return Value;

            return Quote + Value + Quote;
        }

        private readonly Regex _unescape = new Regex(@"(^|[^\\])\\(.)");

        public string UnescapeContents()
        {
            return _unescape.Replace(Value, @"$1$2");
        }
    }
}