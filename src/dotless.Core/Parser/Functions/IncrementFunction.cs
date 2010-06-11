namespace dotless.Core.Parser.Functions
{
    using Infrastructure.Nodes;
    using Tree;

    public class IncrementFunction : NumberFunctionBase
    {
        protected override Node Eval(Number number, Node[] args)
        {
            return new Number(number.Value + 1, number.Unit);
        }
    }
}