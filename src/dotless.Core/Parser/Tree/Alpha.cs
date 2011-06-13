namespace dotless.Core.Parser.Tree
{
    using Infrastructure;
    using Infrastructure.Nodes;

    public class Alpha : Call
    {
        public Node Value { get; set; }

        public Alpha(Node value)
        {
            Value = value;
        }

        public override Node Evaluate(Env env)
        {
            Value = Value.Evaluate(env);

            return this;
        }

        public override void AppendCSS(Env env)
        {
            env.Output
                .Append("alpha(opacity=")
                .Append(Value)
                .Append(")");
        }
    }
}