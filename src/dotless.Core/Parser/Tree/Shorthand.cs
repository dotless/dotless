namespace dotless.Core.Parser.Tree
{
    using Infrastructure;
    using Infrastructure.Nodes;
    using Plugins;

    public class Shorthand : Node
    {
        public Node First { get; set; }
        public Node Second { get; set; }

        public Shorthand(Node first, Node second)
        {
            First = first;
            Second = second;
        }

        public override Node Evaluate(Env env) {
            return new Shorthand(First.Evaluate(env), Second.Evaluate(env));
        }

        public override void AppendCSS(Env env)
        {
            env.Output
                .Append(First)
                .Append("/")
                .Append(Second);
        }

        public override void Accept(IVisitor visitor)
        {
            First = VisitAndReplace(First, visitor);
            Second = VisitAndReplace(Second, visitor);
        }
    }
}