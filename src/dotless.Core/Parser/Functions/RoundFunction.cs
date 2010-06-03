namespace dotless.Core.Parser.Functions
{
    using System;
    using Infrastructure.Nodes;
    using Tree;

    public class RoundFunction : NumberFunctionBase
    {
        protected override Node Eval(Number number, Node[] args)
        {
            return new Number(Math.Round(number.Value), number.Unit);
        }
    }
}