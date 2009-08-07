using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nless.Core.utils;

namespace nless.Core.engine
{
    public class Expression : List<INode>, INode, IEvaluatable
    {
        public INode Parent { get; set; }
        public string ToCss()
        {
            var sb = new StringBuilder();
            foreach (var node in this)
            {
                sb.AppendFormat(" {0} ", node.ToCss());
            }
            return sb.ToString();
        }
        public string ToCSharp()
        {
            var sb = new StringBuilder();
            foreach (var node in this)
            {
                sb.AppendFormat(" {0} ", node.ToCSharp());
            }
            return sb.ToString();
        }

        public Expression(IEnumerable<INode> arr) : this(arr, null)
        {
        }
        public Expression(IEnumerable<INode> arr, INode parent)
        {
            AddRange(arr); //NOTE: This may not be correct approach 
            Parent = parent;
        }

        public bool Terminal {
            get { return Expressions.Count() == 0; }
        }

        public IList<Expression> Expressions
        {
            get
            {
                return this.Where(node => node is Expression)
                    .Select(node => (Expression)node).ToList();
            }
        }
        public IList<Entity> Entities
        {
            get
            {
                return this.Where(node => node is Entity)
                    .Select(node => (Entity)node).ToList();
            }
        }
        public IList<Literal> Literals
        {
            get
            {
                return this.Where(node => node is Literal)
                    .Select(node => (Literal)node).ToList();
            }
        }
        public IList<Operator> Operators
        {
            get
            {
                return this.Where(node => node is Operator)
                    .Select(node => (Operator)node).ToList();
            }
        }
        public INode Evaluate()
        {
            if(this.Count() > 2 || !Terminal)
            {
                for (var i=0; i<Count; i++){
                    this[i] = this[i] is IEvaluatable ? ((IEvaluatable)this[i]).Evaluate() : this[i];
                }
                var result = Operators.Count() == 0 ? this : CsEval.Eval(ToCSharp());
                INode returnNode;

                var unit = Literals.Where(l => l.Unit!=null).Select(l => l.Unit).Distinct().ToArray();
                if (unit.Count() > 1 && Operators.Count() == 0) throw new MixedUnitsExeption(); //TODO: Work out what is passed to exception
                var entity = Literals.Where(e => unit.Contains(e.Unit)).FirstOrDefault() ?? Entities.First();

                if (result is Entity) returnNode = (INode)result;
                else if (result is Expression)
                    returnNode = ((Expression)result).Count() == 1
                                     ? ((Expression)result).First()
                                     : (Expression)result;
                else returnNode = entity is Number && unit.Count() > 0
                                     ? (INode)Activator.CreateInstance(entity.GetType(), unit.First(), result)  
                                     : (INode)Activator.CreateInstance(entity.GetType(), result);
                return returnNode;
            }
            else if(this.Count() == 1)
            {
                return this.First();
            }
            return this;
        }

        //  if size > 2 or !terminal?
        //  # Replace self with an evaluated sub-expression
        //  replace map {|e| e.respond_to?(:evaluate) ? e.evaluate : e }

        //  unit = literals.map do |node|
        //    node.unit
        //  end.compact.uniq.tap do |ary|
        //    raise MixedUnitsError, self * ' ' if ary.size > 1 && !operators.empty?
        //  end.join
          
        //  entity = literals.find {|e| e.unit == unit } || entities.first
        //  result = operators.empty?? self : eval(to_ruby.join)
          
        //  case result
        //    when Entity     then result
        //    when Expression then result.one?? result.first : self.class.new(result)
        //    else entity.class.new(result, *(unit if entity.class == Node::Number))
        //  end
        //elsif size == 1
        //  first
        //else
        //  self
    }

    public class MixedUnitsExeption : Exception
    {
    }

    public interface IEvaluatable
    {
        INode Evaluate();
    }
}
