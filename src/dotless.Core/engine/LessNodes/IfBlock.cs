using System.Collections.Generic;
using dotless.Core.exceptions;

namespace dotless.Core.engine.LessNodes
{
    public class IfBlock : NodeBlock
    {
        public BoolExpression Expression { get; private set; }
        public IfBlock(BoolExpression expression)
        {
            Expression = expression;
        }
    }

    public class BoolExpression : Expression
    {
        public BoolExpression(IEnumerable<INode> arr) : base(arr)
        {
        }

        public BoolExpression(IEnumerable<INode> arr, INode parent) : base(arr, parent)
        {
        }

        public new Bool Evaluate()
        {
            var value = base.Evaluate();
            if (value.GetType() != typeof(Bool))
                throw new ParsingException("Bool expressions must evauate to true or false");
            return (Bool) value;
        }
    }
}
