using System;
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
        public IList<INode> Path()
        {
            return Path(this);
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

                var unit = Literals.Where(l => !string.IsNullOrEmpty(l.Unit)).Select(l => l.Unit).Distinct().ToArray();
                if (unit.Count() > 1 && Operators.Count() != 0) throw new MixedUnitsExeption(); 
                var entity = Literals.Where(e => unit.Contains(e.Unit)).FirstOrDefault() ?? Entities.First();

                if (result is Entity) returnNode = (INode)result;
                else if (result is Expression)
                    returnNode = ((Expression)result).Count() == 1
                                     ? ((Expression)result).First()
                                     : (Expression)result;
                else returnNode = entity is Number && unit.Count() > 0
                                     ? (INode)Activator.CreateInstance(entity.GetType(), unit.First(), float.Parse(result.ToString()))
                                     : (INode)Activator.CreateInstance(entity.GetType(), float.Parse(result.ToString()));
                return returnNode;
            }
            return this.Count() == 1 ? this.First() : this;
        }
    }
}
