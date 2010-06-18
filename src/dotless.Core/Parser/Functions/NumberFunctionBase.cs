namespace dotless.Core.Parser.Functions
{
    using System.Linq;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;
    using Utils;

    public abstract class NumberFunctionBase : Function
    {
        protected override Node Evaluate(Env env)
        {
            Guard.ExpectMinArguments(1, Arguments.Count, this, Index);
            Guard.ExpectNode<Number>(Arguments[0], this, Arguments[0].Index);

            var number = Arguments[0] as Number;
            var args = Arguments.Skip(1).ToArray();

            return Eval(env, number, args);
        }

        protected abstract Node Eval(Env env, Number number, Node[] args);
    }
}