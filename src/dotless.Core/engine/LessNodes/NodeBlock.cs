using System;
using System.Collections.Generic;
using System.Linq;

namespace dotless.Core.engine
{
    public class NodeBlock : IBlock
    {
        public NodeBlock()
        {
            Children = new List<INode>();
        }

        public void Add(INode token)
        {
            token.Parent = this;
            Children.Add(token);
        }

        public bool IsEmpty
        {
            get { return Children.Count() == 0; }
        }

        public bool IsLeaf
        {
            get { return Elements.Count() == 0; }
        }

        public ElementBlock Last
        {
            get { return Elements.LastOrDefault(); }
        }

        public INode Parent { get; set; }
        public List<INode> Children { get; set; }

        public IList<INode> Path(INode node)
        {
            var path = new List<INode>();
            if (node == null) node = this;
            while (node != null)
            {
                path.Add(node);
                node = node.Parent;
            }
            return path;
        }

        [Obsolete("CSS rendering handled externally in CssBuilder.cs. We need to make all nodes act this way")]
        public virtual string ToCss()
        {
            return string.Empty;
        }

        public List<IBlock> SubBlocks
        {
            get { return Children.Where(c => c is IBlock).Select(c => (IBlock) c).ToList(); }
        }

        public IList<Property> Properties
        {
            get
            {
                return Children.Where(r => r.GetType() == typeof (Property))
                    .Select(r => (Property) r).ToList();
            }
        }

        public IList<Variable> Variables
        {
            get
            {
                return Children.Where(r => r.GetType() == typeof (Variable))
                    .Select(r => (Variable) r).ToList();
            }
        }

        public IList<ElementBlock> Elements
        {
            get
            {
                return Children.Where(r => r.GetType() == typeof (ElementBlock))
                    .Select(r => (ElementBlock) r).ToList();
            }
        }

        public IList<Insert> Inserts
        {
            get
            {
                return Children.Where(r => r.GetType() == typeof (Insert))
                    .Select(r => (Insert) r).ToList();
            }
        }

        public bool IsRoot()
        {
            return Parent == null;
        }

        public IList<INode> Path()
        {
            return Path(this);
        }
    }
}