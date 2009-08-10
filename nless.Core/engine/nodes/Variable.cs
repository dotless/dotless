using System.Collections;
using System.Collections.Generic;

namespace nless.Core.engine
{
    public class Variable : Property, IEvaluatable
    {
        protected bool _declaration;

        public Variable(string key)
            : this(key, new List<INode>(), null)
        {
        }
        
        public Variable(string key, INode value)
            : this(key, value, null)
        {
        }
        public Variable(string key, INode value, Element parent)
            :this(key, new List<INode>{value}, parent)
        {
        }
        public Variable(string key, IEnumerable<INode> value)
            : this(key, value, null)
        {

        }
        public Variable(string key, IEnumerable<INode> value, Element parent)
            : base(key, value, parent)
        {
            _declaration = (value==null || ((IList)value).Count == 0)? false : true;
            Key = key.Replace("@", "");
        }
        public override string  ToString()
        {
            return "@" + Key;
        }
        public override INode Evaluate()
        {
            if(_declaration)
                _eval = _eval ?? Value.Evaluate();
            else
                _eval = _eval ?? (ParentAs<INearestResolver>()
                                    .NearestAs<IEvaluatable>(ToString()))
                                    .Evaluate();
            return _eval;
        }

        public override string  ToCSharp()
        {
            return Evaluate() == null ? "" : Evaluate().ToCSharp(); ;
        }
        public override string  ToCss()
        {
            return Evaluate()==null ? "" : Evaluate().ToCss(); ;
        } 
         
    }
}