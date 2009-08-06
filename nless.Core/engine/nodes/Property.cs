using System.Collections.Generic;

namespace nless.Core.engine
{

    public class Property : Entity
    {
        public string Key { get; set; }
        new public Expression Value { get; set; }
        private bool _eval = false;
        private bool Empty
        {
            get
            {
                return Value == null;
            }
        }
        public Property(string key, INode value)
            : this(key, value, null)
        {
        }
        public Property(string key, INode value, INode parent)
            :this(key, new List<INode>{value}, parent)
        {
        }
        public Property(string key, IEnumerable<INode> value)
            : this(key, value, null)
        {

        }
        public Property(string key, IEnumerable<INode> value, INode parent)
        {
            Key = key;
            foreach (var node in value){
                node.Parent = this;
            }
            Value = new Expression(value, this);
            _eval = false;
        }

        public void Add(INode token)
        {
            token.Parent = this;
            Value.Add(token);
        }
        public void Add(string token)
        {
            var node = new Anonymous(token);
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

        //# TODO: Dont understand ||=
        //def evaluate    
        //  @eval ||= value.evaluate
        //end

        //TODO: This is wrong, but god knows what this should be
        public INode Evaluate()
        {
            return Value.Evaluate();
        }

        //TODO: Dont understand where parent.Nearest is set?
        //def nearest node
        //  parent.nearest node
        //end

        public override string ToCss()
        {
            return string.Format("{0}:{1}", Key, Value.Evaluate().ToCss());
        }
          
    }
}
