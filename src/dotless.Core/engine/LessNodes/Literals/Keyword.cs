using System.Collections.Generic;

namespace dotless.Core.engine
{
    public class Keyword : Literal, IEvaluatable
    {
        public Keyword(string value) : base(value)
        {
        }

        public INode Evaluate()
        {
            INode result;

            result = Color.GetColorFromKeyword(Value);

            return result ?? this;
        }
    }
}