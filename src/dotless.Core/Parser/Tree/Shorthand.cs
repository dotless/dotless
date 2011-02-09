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

        public override void AppendCSS(Env env)
        {
            env.Output
                .Append(First)
                .Append("/")
                .Append(Second);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(First);
            visitor.Visit(Second);
        }
    }
}