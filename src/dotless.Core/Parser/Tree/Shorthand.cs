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

        public override StringBuilder ToCSS(Env env, StringBuilder output)
        {
            return output.AppendCSS(First, env)
                    .Append("/")
                    .AppendCSS(Second, env);
        }
    }
}