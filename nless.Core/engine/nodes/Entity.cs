using System.Collections.Generic;
using System.Linq;

namespace nless.Core.engine.nodes
{
    public class Entity
    {
        public Entity Parent { get; set; }

        public Entity(): this(null)
        { 
        }
        public Entity(Entity parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Returns the path from any given node, to the root
        /// </summary>
        /// <param name="node"></param>
        /// <returns>ex: ['color', 'p', '#header', 'body', '*']</returns>
        public IList<Entity> Path(Entity node)
        {
            var path = new List<Entity>();
            if(node==null) node = this;
            while(node!=null)
            {
                path.Add(node);
                node = node.Parent;
            }
            return path;
        }

        public Entity Last
        {
            get
            {
                return Path(null).Last();
            }
        }

        public virtual string Inspect()
        {
            return ToString();
        }

        public virtual string ToCss()
        {
            return ToString();
        }
    }
    public class Anonymous : Entity
    {
    }

    public class Operator : Entity
    {
    }

    public class Paren : Entity
    {
    }
}
