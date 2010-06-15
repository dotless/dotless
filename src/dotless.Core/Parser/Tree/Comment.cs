namespace dotless.Core.Parser.Tree
{
    using Infrastructure;
    using Infrastructure.Nodes;

    public class Comment : Node
    {
        public string Value { get; set; }

        public Comment(string value)
        {
            Value = value;
        }

        public override string ToCSS(Env env)
        {
            return env.Compress ? "" : Value;
        }
    }
}