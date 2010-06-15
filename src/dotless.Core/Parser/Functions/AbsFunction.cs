namespace dotless.Core.Parser.Functions
{
    using System;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;

    public class AbsFunction : NumberFunctionBase
    {
        protected override Node Eval(Env env, Number number, Node[] args)
        {
            return new Number(Math.Abs(number.Value), number.Unit);
        }
    }
}