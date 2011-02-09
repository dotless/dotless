namespace dotless.Core.Plugins
{
    using Parser.Infrastructure.Nodes;
    using Parser.Tree;

    public abstract class VisitorPlugin : IPlugin, IVisitor
    {
        public Ruleset Apply(Ruleset tree)
        {
            Visit(tree);

            return tree;
        }

        public void Visit(Node node)
        {
            if (Execute(node))
                node.Accept(this);
        }

        public abstract bool Execute(Node node);
    }
}