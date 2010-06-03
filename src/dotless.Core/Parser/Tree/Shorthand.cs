namespace dotless.Core.Parser.Tree
{
    using Infrastructure.Nodes;

    public class Shorthand : Node
    {
        public Node First { get; set; }
        public Node Second { get; set; }

        public Shorthand(Node first, Node second)
        {
            First = first;
            Second = second;
        }

        public override string ToCSS()
        {
            return First.ToCSS() + "/" + Second.ToCSS();
        }
    }
}