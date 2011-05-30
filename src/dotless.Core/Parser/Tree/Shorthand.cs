namespace dotless.Core.Parser.Tree
{
    using Infrastructure;
    using Infrastructure.Nodes;
    using System.Text;

    public class Shorthand : Node
    {
        public Node First { get; set; }
        public Node Second { get; set; }

        public Shorthand(Node first, Node second)
        {
            First = first;
            Second = second;
        }

        public override void ToCSS(Env env, StringBuilder output)
        {
            First.ToCSS(env, output);
            output.Append("/");
            Second.ToCSS(env, output);
        }
    }
}