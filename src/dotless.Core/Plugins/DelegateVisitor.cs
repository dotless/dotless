using System;
using System.Collections.Generic;
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

        public Node Visit(Node node) {
            var list = node as IList<Node>;
            if (list != null) {
                for (var i = 0; i < list.Count; i++) {
                    list[i] = Visit(list[i]);
                }
            }
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
        public static IVisitor For<TNode>(Action<TNode> action) where TNode : Node
        {
            return new DelegateVisitor(node =>
            {
                var typed = node as TNode;
                if (typed != null)
                {
                    action(typed);
                }
                return node;
            });
        }
    }
}