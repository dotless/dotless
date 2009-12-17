using System;

namespace dotless.Core.engine.Functions
{
    public class IncrementFunction : FunctionBase
    {
        public override INode Evaluate()
        {
            if (Arguments.Length != 1 || !(Arguments[0] is Number))
                throw new InvalidOperationException();
            var arg = (Number)Arguments[0];
            return new Number(arg.Unit, arg.Value + 1);
        }
    }
}
