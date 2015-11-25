namespace dotless.Core.Parser.Tree
{
    using Infrastructure;
    using Infrastructure.Nodes;

    public class Script : Node
    {
        public string Expression { get; set; }

        public Script(string script)
        {
            Expression = script;
        }

        protected override Node CloneCore() {
            return new Script(Expression);
        }

        public override Node Evaluate(Env env)
        {
            return new TextNode("[script unsupported]");
        }
    }
}