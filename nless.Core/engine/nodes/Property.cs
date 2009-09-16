using System.Collections.Generic;

namespace nless.Core.engine
{
    public class Property : Entity, INearestResolver
    {
        public INode _eval { get; set; }

        public Property(string key)
            : this(key, new List<INode>(), null)
        {
        }

        public Property(string key, INode value)
            : this(key, value, null)
        {
        }

        public Property(string key, INode value, Element parent)
            : this(key, new List<INode> {value}, parent)
        {
        }

        public Property(string key, IEnumerable<INode> value)
            : this(key, value, null)
        {
        }

        public Property(string key, IEnumerable<INode> value, Element parent)
        {
            Key = key;
            foreach (var node in value){
                node.Parent = this;
            }
            Value = new Expression(value, this);
            _eval = null;
            Parent = parent;
        }

        public string Key { get; set; }
        public new Expression Value { get; set; }

        public T ParentAs<T>()
        {
            return (T) Parent;
        }

        private bool Empty
        {
            get { return Value == null; }
        }

        public void Add(INode token)
        {
            token.Parent = this;
            Value.Add(token);
        }

        public void Add(string token)
        {
            var node = new Anonymous(token) {Parent = this};
            Add(node);
        }

        public static bool operator ==(Property property1, Property property2)
        {
            return property1.ToString() == property2.ToString();
        }

        public static bool operator !=(Property property1, Property property2)
        {
            return property1.ToString() != property2.ToString();
        }

        public override bool Equals(object obj)
        {
            return (obj is Property && ToString() == obj.ToString() && Value.Equals(((Property) obj).Value));
        }


        public virtual INode Evaluate()
        {
            _eval = _eval ?? Value.Evaluate();
            return _eval;
        }

        public override string ToCss()
        {
            return string.Format("{0}: {1};", Key, Value.Evaluate().ToCss());
        }

        public override string ToString()
        {
            return Key;
        }

        public INode Nearest(string ident)
        {
            return ParentAs<Element>().Nearest(ident);
        }

        public T NearestAs<T>(string ident)
        {
            return (T)Nearest(ident);
        }
    }
}