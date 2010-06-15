namespace dotless.Core.Parser.Functions
{
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;

    public class IncrementFunction : NumberFunctionBase
    {
        protected override Node Eval(Env env, Number number, Node[] args)
        {
            return new Number(number.Value + 1, number.Unit);
        }
    }
}