using System.Collections;
using System.Collections.Generic;

namespace dotless.Core.engine
{
    public class Variable : Property, IEvaluatable
    {
        protected bool Declaration;

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
            Declaration = (value==null || ((IList)value).Count == 0)? false : true;
            Key = key.Replace("@", "");
        }
        public override string  ToString()
        {
            return "@" + Key;
        }

        /// <summary>
        /// Evaluates the variables value i.e. @color: #fff +1;
        /// </summary>
        /// <returns></returns>
        /// <remarks>Only evaluates first time, next time will just return last evaluation</remarks>
        public override INode Evaluate()
        {
            if(Declaration)
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