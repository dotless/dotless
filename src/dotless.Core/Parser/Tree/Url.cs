namespace dotless.Core.Parser.Tree
{
    using Infrastructure;
    using Infrastructure.Nodes;

    public class Url : Node
    {
        public TextNode Value { get; set; }

        public Url(TextNode value)
        {
            Value = value;
        }

        public string GetUrl()
        {
            return Value.Value;
        }

        public override string ToCSS(Env env)
        {
            return "url(" + Value.ToCSS(env) + ")";
        }
    }
}