namespace dotless.Core.Parser.Functions
{
    using System;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;

    public class RoundFunction : NumberFunctionBase
    {
        protected override Node Eval(Env env, Number number, Node[] args)
        {
            return new Number(Math.Round(number.Value, MidpointRounding.AwayFromZero), number.Unit);
        }
    }
}