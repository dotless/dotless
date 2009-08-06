using System.Collections.Generic;
using System.Linq;

namespace nless.Core.engine
{
    public interface INode
    {
        INode Parent { get; set; }
        string ToCss();
        string ToCSharp();
    }
    public class Entity : INode
    {

        public INode Parent { get; set; }
        public string Value { get; set;}

        protected Entity()
        {
        }
        public Entity(string value)
            : this(value,null)
        { 
        }
        public Entity(string value, INode parent)
        {
            Value = value;
            Parent = parent;
        }

        /// <summary>
        /// Returns the path from any given node, to the root
        /// </summary>
        /// <param name="node"></param>
        /// <returns>ex: ['color', 'p', '#header', 'body', '*']</returns>
        public IList<INode> Path(INode node)
        {
            var path = new List<INode>();
            if(node==null) node = this;
            while(node!=null)
            {
                path.Add(node);
                node = node.Parent;
            }
            return path;
        }

        public INode Last
        {
            get
            {
                return Path(null).Last();
            }
        }

        public virtual string Inspect()
        {
            return Value.ToString();
        }

        public virtual string ToCss()
        {
            return Value.ToString();
        }
        public virtual string ToCSharp()
        {
            return Value.ToString();
        }
    }

    public class Anonymous : Entity
    {
        protected Anonymous()
        {
        }

        public Anonymous(string value) : base(value)
        {
        }

        public Anonymous(string value, INode parent) : base(value, parent)
        {
        }
    }

    public class Operator : Entity
    {
        protected Operator()
        {
        }

        public Operator(string value) : base(value)
        {
        }

        public Operator(string value, INode parent) : base(value, parent)
        {
        }
    }

    public class Paren : Entity
    {
        protected Paren()
        {
        }

        public Paren(string value) : base(value)
        {
        }

        public Paren(string value, INode parent) : base(value, parent)
        {
        }
    }
}
