namespace dotless.Core.Parser.Functions
{
    using Exceptions;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;

    public class PercentageFunction : NumberFunctionBase
    {
        protected override Node Eval(Env env, Number number, Node[] args)
        {
            return new Number(number.Value * 100, "%");
        }
    }
}