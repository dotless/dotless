namespace dotless.Core.Parser.Tree
{
    using Infrastructure;
    using Infrastructure.Nodes;
    using System.Text;

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

        public override void ToCSS(Env env, StringBuilder output)
        {
            output.Append("alpha(opacity=");
            Value.ToCSS(env, output);
            output.Append(")");
        }
    }
}