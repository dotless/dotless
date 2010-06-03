namespace dotless.Core.Parser.Functions
{
    using System.Linq;
    using Infrastructure.Nodes;
    using Tree;
    using Utils;

    public abstract class NumberFunctionBase : Function
    {
        protected override Node Evaluate()
        {
            Guard.ExpectMinArguments(1, Arguments.Count, this);
            Guard.ExpectNode<Number>(Arguments[0], this);

            var number = Arguments[0] as Number;
            var args = Arguments.Skip(1).ToArray();

            return Eval(number, args);
        }

        protected abstract Node Eval(Number number, Node[] args);
    }
}