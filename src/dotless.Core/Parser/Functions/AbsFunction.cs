namespace dotless.Core.Parser.Functions
{
    using System;
    using Infrastructure.Nodes;
    using Tree;

    public class AbsFunction : NumberFunctionBase
    {
        protected override Node Eval(Number number, Node[] args)
        {
            return new Number(Math.Abs(number.Value), number.Unit);
        }
    }
}