namespace dotless.Core.Parser.Tree
{
    using Infrastructure;
    using Infrastructure.Nodes;

    public class Comment : Node
    {
        public string Value { get; set; }
        public bool Silent { get; set; }

        public Comment(string value, bool silent)
        {
            Value = value;
            Silent = silent;
        }

        public override void AppendCSS(Env env)
        {
			if (!Silent)
				env.Output.Append(env.Compress ? "" : Value);
        }
    }
}