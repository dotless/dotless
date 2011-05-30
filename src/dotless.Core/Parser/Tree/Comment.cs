namespace dotless.Core.Parser.Tree
{
    using Infrastructure;
    using Infrastructure.Nodes;
    using System.Text;

    public class Comment : Node
    {
        public string Value { get; set; }
        public bool Silent { get; set; }

        public Comment(string value, bool silent)
        {
            Value = value;
            Silent = silent;
        }

        public override void ToCSS(Env env, StringBuilder output)
        {
            output.Append(env.Compress ? "" : Value);
        }
    }
}