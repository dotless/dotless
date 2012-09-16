namespace dotless.Core.Parser.Functions
{
    using System;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;
    using dotless.Core.Utils;

    public class RoundFunction : NumberFunctionBase
    {
        protected override Node Eval(Env env, Number number, Node[] args)
        {
            if (args.Length == 0)
            {
                return new Number(Math.Round(number.Value, MidpointRounding.AwayFromZero), number.Unit);
            }
            else
            {
                Guard.ExpectNode<Number>(args[0], this, args[0].Location);
                return new Number(Math.Round(number.Value, (int)((Number)args[0]).Value, MidpointRounding.AwayFromZero), number.Unit);
            }
        }
    }
}