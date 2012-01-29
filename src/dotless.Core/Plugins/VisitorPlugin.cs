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

        public abstract string Name { get; }
        public abstract VisitorPluginType AppliesTo { get; }

        public Node Visit(Node node)
        {
            if (Execute(ref node))
                node.Accept(this);

            return node;
        }

        public abstract bool Execute(ref Node node);

        public virtual void OnPreVisiting(Env env)
        {
        }

        public virtual void OnPostVisiting(Env env)
        {
        }
    }
}