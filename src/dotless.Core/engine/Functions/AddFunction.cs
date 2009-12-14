using System;
using System.Linq;

namespace dotless.Core.engine.Functions
{
    public class AddFunction : FunctionBase
    {
        public override INode Evaluate()
        {
            if(!Arguments.All(x => x is Number))
                throw new InvalidOperationException();
            var args = Arguments.Cast<Number>();
            var result = new Number(0);
            foreach(var arg in args)
                result += arg;
            return result;
        }
    }
}
