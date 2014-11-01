namespace dotless.Core.Parser.Functions
{
    using System;
    using System.Linq;
    using dotless.Core.Exceptions;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;
    using Utils;

    public abstract class ListFunctionBase : Function
    {
        protected override Node Evaluate(Env env)
        {
            Guard.ExpectMinArguments(1, Arguments.Count, this, Location);
            Guard.ExpectNodeToBeOneOf<Expression, Value>(Arguments[0], this, Arguments[0].Location);

            if(Arguments[0] is Expression)
            {
                var list = Arguments[0] as Expression;
                var args = Arguments.Skip(1).ToArray();
                return Eval(env, list.Value.ToArray(), args);
            }

            if(Arguments[0] is Value)
            {
                var list = Arguments[0] as Value;
                var args = Arguments.Skip(1).ToArray();
                return Eval(env, list.Values.ToArray(), args);
            }

            // We should never get here due to the type guard...
            throw new ParsingException(string.Format("First argument to the list function was a {0}", Arguments[0].GetType().Name.ToLowerInvariant()), Location);
        }

        protected abstract Node Eval(Env env, Node[] list, Node[] args);
    }
}