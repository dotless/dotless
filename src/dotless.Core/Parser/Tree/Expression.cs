namespace dotless.Core.Parser.Tree
{
    using Infrastructure;
    using Infrastructure.Nodes;
    using System.Linq;
    using System.Collections.Generic;
    using Plugins;

    public class Expression : Node
    {
        public NodeList Value { get; set; }
        private bool IsExpressionList { get; set; }

        public Expression(IEnumerable<Node> value) : this(value, false)
        {
        }

        public Expression(IEnumerable<Node> value, bool isExpressionList)
        {
            IsExpressionList = isExpressionList;

            if(value is NodeList)
                Value = value as NodeList;
            else
                Value = new NodeList(value);
        }

        public override Node Evaluate(Env env)
        {
            if (Value.Count > 1)
                return new Expression(new NodeList(Value.Select(e => e.Evaluate(env))), IsExpressionList).ReducedFrom<Node>(this);

            if (Value.Count == 1)
                return Value[0].Evaluate(env).ReducedFrom<Node>(this);

            return this;
        }

        protected override Node CloneCore() {
            return new Expression((NodeList)Value.Clone(), IsExpressionList);
        }

        public override void AppendCSS(Env env)
        {
            env.Output.AppendMany(Value, IsExpressionList ? ", " : " ");
        }

        public override void Accept(IVisitor visitor)
        {
            Value = VisitAndReplace(Value, visitor);
        }
    }
}