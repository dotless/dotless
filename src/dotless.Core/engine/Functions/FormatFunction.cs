using System;
using System.Collections.Generic;
using System.Linq;

namespace dotless.Core.engine.Functions
{
    public class FormatFunction : FunctionBase
    {
        public override INode Evaluate()
        {
            if(Arguments.Length == 0)
                return new String("");

            var format = Arguments[0].ToString();

            var args = Arguments.Skip(1).Select(n => n.ToString()).ToArray();

            var result = string.Format(format, args);

            return new String(result);
        }
    }
}