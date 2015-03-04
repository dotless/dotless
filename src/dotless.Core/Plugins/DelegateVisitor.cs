using System;
using dotless.Core.Parser.Infrastructure.Nodes;

namespace dotless.Core.Plugins
{
    public class DelegateVisitor : IVisitor
    {
        private readonly Func<Node, Node> visitor;

        public DelegateVisitor(Func<Node, Node> visitor)
        {
            this.visitor = visitor;
        }

        public Node Visit(Node node)
        {
            return visitor(node);
        }

        public static IVisitor For<TNode>(Func<TNode, Node> projection) where TNode : Node
        {
            return new DelegateVisitor(node =>
            {
                var typed = node as TNode;
                if (typed == null)
                {
                    return node;
                }
                return projection(typed);
            });
        }
    }
}