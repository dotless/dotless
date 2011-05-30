namespace dotless.Core.Parser.Tree
{
    using Infrastructure;
    using Infrastructure.Nodes;
    using System.Text;

    public class Keyword : Node
    {
        public string Value { get; set; }

        public Keyword(string value)
        {
            Value = value;
        }

        public override Node Evaluate(Env env)
        {
            return (Node) Color.GetColorFromKeyword(Value) ?? this;
        }

        public override void ToCSS(Env env, StringBuilder output)
        {
            output.Append(Value);
        }
    }
}