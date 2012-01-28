namespace dotless.Core.Plugins
{
    using System;
    using Parser.Infrastructure.Nodes;
    using Parser.Tree;

    public abstract class VisitorPlugin : IVisitorPlugin, IVisitor
    {
        public Ruleset Apply(Ruleset tree)
        {
            Visit(tree);

            return tree;
        }

        public abstract string Name { get; }
        public abstract VisitorPluginType AppliesTo { get; }

        public void Visit(Node node)
        {
            if (Execute(node))
                node.Accept(this);
        }

        public abstract bool Execute(Node node);
    }
}