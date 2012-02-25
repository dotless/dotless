namespace dotless.Core.Parser.Tree
{
    using Infrastructure;
    using Infrastructure.Nodes;
    using Plugins;

    public class Condition : Node
    {
        public Node Left { get; set; }
        public Node Right { get; set; }
        public string Operation { get; set; }
        public bool Negate { get; set; }

        public Condition(Node left, string operation, Node right, bool negate)
        {
            Left = left;
            Right = right;
            Operation = operation;
            Negate = negate;
        }

        public override void AppendCSS(Env env)
        {
        }

        public override Node Evaluate(Env env)
        {
            return this;
        }

        public override void Accept(IVisitor visitor)
        {
            Left = VisitAndReplace(Left, visitor);
            Right = VisitAndReplace(Right, visitor);
        }
    }
}