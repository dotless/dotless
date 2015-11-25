namespace dotless.Core.Parser.Tree
{
    using Infrastructure;
    using Infrastructure.Nodes;
    using Plugins;

    public class Paren : Node
    {
        public Node Value { get; set; }

        public Paren(Node value)
        {
            Value = value;
        }

        protected override Node CloneCore() {
            return new Paren(Value.Clone());
        }

        public override void AppendCSS(Env env)
        {
            env.Output
                .Append('(')
                .Append(Value)
                .Append(')');
        }

        public override Node Evaluate(Env env)
        {
            return new Paren(Value.Evaluate(env));
        }

        public override void Accept(IVisitor visitor)
        {
            Value = VisitAndReplace(Value, visitor);
        }
    }
}