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

        public Quoted(string value, char? quote, bool escaped)
            : base(value)
        {
            Escaped = escaped;
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

        public override void AppendCSS(Env env)
        {
            if (Escaped)
            {
                env.Output.Append(Value);
                return;
            }

            env.Output
                .Append(Quote)
                .Append(Value)
                .Append(Quote);
        }

        public override Node Evaluate(Env env)
        {
          var value = Regex.Replace(Value, @"@\{([\w-]+)\}",
                        m =>
                          {
                            var v = new Variable('@' + m.Groups[1].Value) {Index = Index + m.Index}.Evaluate(env);
                            return v is TextNode ? (v as TextNode).Value : v.ToCSS(env);
                          });

          return new Quoted(value, Quote, Escaped).ReducedFrom<Quoted>(this);
        }

        private readonly Regex _unescape = new Regex(@"(^|[^\\])\\(.)");

        public string UnescapeContents()
        {
            return _unescape.Replace(Value, @"$1$2");
        }
    }
}