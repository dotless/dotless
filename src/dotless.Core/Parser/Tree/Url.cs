namespace dotless.Core.Parser.Tree
{
    using Infrastructure;
    using Infrastructure.Nodes;

    public class Url : Node
    {
        public Node Value { get; set; }

        public Url(Node value)
        {
            Value = value;
        }

        public string GetUrl()
        {
            if (Value is Quoted)
                return ((Quoted) Value).Contents;

            return Value.ToCSS(null); // null should be fine here since Value is either Quoted or TextNode.
        }

        public override string ToCSS(Env env)
        {
            return "url(" + Value.ToCSS(env) + ")";
        }
    }
}