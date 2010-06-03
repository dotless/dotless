namespace dotless.Core.Parser.Functions
{
    using System.Linq;
    using Infrastructure.Nodes;
    using Tree;
    using Utils;

    public class AddFunction : Function
    {
        protected override Node Evaluate()
        {
            Guard.ExpectAllNodes<Number>(Arguments, this);

            var value = Arguments.Cast<Number>().Select(d => d.Value).Aggregate(0d, (a, b) => a + b);

            return new Number(value);
        }
    }
}