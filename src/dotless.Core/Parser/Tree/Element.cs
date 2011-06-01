namespace dotless.Core.Parser.Tree
{
    using Infrastructure;
    using Infrastructure.Nodes;
    using System.Text;

    public class Element : Node
    {
        public Combinator Combinator { get; set; }
        public string Value { get; set; }

        public Element(Combinator combinator, string value)
        {
            Combinator = combinator ?? new Combinator("");
            Value = value == null ? "" : value.Trim();
        }

        public override StringBuilder ToCSS(Env env, StringBuilder output)
        {
            return output.AppendCSS(Combinator, env)
                .Append(Value);
        }
    }
}