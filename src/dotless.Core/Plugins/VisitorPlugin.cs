namespace dotless.Core.Plugins
{
    using System;
    using Parser.Infrastructure.Nodes;
    using Parser.Tree;
    using dotless.Core.Parser.Infrastructure;

    public abstract class VisitorPlugin : IVisitorPlugin, IVisitor
    {
        public Root Apply(Root tree)
        {
            Visit(tree);

            return tree;
        }

        public abstract VisitorPluginType AppliesTo { get; }

        public Node Visit(Node node)
        {
            bool visitDeeper;
            node = Execute(node, out visitDeeper);
            if (visitDeeper)
                node.Accept(this);

            return node;
        }

        public abstract Node Execute(Node node, out bool visitDeeper);

        public virtual void OnPreVisiting(Env env)
        {
        }

        public virtual void OnPostVisiting(Env env)
        {
        }
    }
}