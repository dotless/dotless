namespace dotless.Core.Parser.Functions
{
    using System.Linq;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;
    using Utils;

    public class RgbaFunction : Function
    {
        protected override Node Evaluate(Env env)
        {
            if (Arguments.Count == 2)
            {
                var color = Guard.ExpectNode<Color>(Arguments[0], this, Location);
                var number = Guard.ExpectNode<Number>(Arguments[1], this, Location);

                return new Color(color.RGB, number.Value);
            }

            Guard.ExpectNumArguments(4, Arguments.Count, this, Location);
            var args = Guard.ExpectAllNodes<Number>(Arguments, this, Location);

            var values = args.Select(n => n.Value).ToList();
            var rgb = values.Take(3).ToArray();
            var alpha = values[3];

            return new Color(rgb, alpha);
        }
    }
}