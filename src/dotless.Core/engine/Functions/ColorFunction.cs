using System;

namespace dotless.Core.engine.Functions
{
    class ColorFunction : FunctionBase
    {
        public override INode Evaluate()
        {
            if (Arguments.Length != 1 || !(Arguments[0] is String))
                throw new InvalidOperationException();
            var arg = (String) Arguments[0];
            if(arg.Content == "evil red")
            {
                return new Color(153, 153, 153);
            }
            return new Color(0, 0, 0);
        }
    }
}
