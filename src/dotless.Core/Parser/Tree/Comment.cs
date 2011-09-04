namespace dotless.Core.Parser.Tree
{
    using Infrastructure;
    using Infrastructure.Nodes;

    public class Comment : Node
    {
        public string Value { get; set; }
        public bool Silent { get; set; }
        public bool IsPreSelectorComment { get; set; }
        private bool IsCSSHack { get; set; }

        public Comment(string value, bool silent)
        {
            Value = value;
            Silent = silent;
            IsCSSHack = value == "/**/" || value == "/*\\*/";
        }

        public override void AppendCSS(Env env)
        {
            if (!Silent && (!env.Compress || IsCSSHack))
            {
                env.Output.Append(Value);

                if (!env.Compress && !IsCSSHack && IsPreSelectorComment)
                {
                    env.Output.Append("\n");
                }
            }
        }
    }
}