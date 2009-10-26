namespace dotless.Core.engine
{
    using System.Collections.Generic;
    using System.Linq;

    public class Entity : INode
    {
        public string Value { get; set; }
        public INode Parent { get; set; }

        protected Entity(){
        }

        public Entity(string value)
            : this(value, null){
            }

        public Entity(string value, INode parent){
            Value = value;
            Parent = parent;
        }

        public INode Last
        {
            get { return Path(null).Last(); }
        }

        public virtual string ToCss()
        {
            return Value;
        }

        public virtual string ToCSharp()
        {
            return Value;
        }

        /// <summary>
        /// Returns the path from any given node, to the root
        /// </summary>
        /// <param name="node"></param>
        /// <returns>ex: ['color', 'p', '#header', 'body', '*']</returns>
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
        public IList<INode> Path(){ return Path(this); }
        public virtual string Inspect()
        {
            return Value;
        }
    }
}