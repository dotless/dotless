namespace dotless.Core.Parser.Functions
{
    using System;
    using Infrastructure.Nodes;
    using Tree;

    public class CeilFunction : NumberFunctionBase
    {
        protected override Node Eval(Number number, Node[] args)
        {
            return new Number(Math.Ceiling(number.Value), number.Unit);
        }
    }
}