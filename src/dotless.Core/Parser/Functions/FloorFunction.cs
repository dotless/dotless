namespace dotless.Core.Parser.Functions
{
    using System;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;

    public class FloorFunction : NumberFunctionBase
    {
        protected override Node Eval(Env env, Number number, Node[] args)
        {
            return new Number(Math.Floor(number.Value), number.Unit);
        }
    }
}