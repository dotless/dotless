
namespace dotless.Core.Parser.Functions
{
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;
    using Utils;

    public class ContrastFunction : Function
    {
        protected override Node Evaluate(Env env)
        {
            Guard.ExpectMinArguments(1, Arguments.Count, this, Location);
            Guard.ExpectMaxArguments(4, Arguments.Count, this, Location);
            Guard.ExpectNode<Color>(Arguments[0], this, Location);

            var color = (Color) Arguments[0];

            if (Arguments.Count > 1)
                Guard.ExpectNode<Color>(Arguments[1], this, Location);
            if (Arguments.Count > 2)
                Guard.ExpectNode<Color>(Arguments[2], this, Location);
            if (Arguments.Count > 3)
                Guard.ExpectNode<Number>(Arguments[3], this, Location);

            var lightColor = Arguments.Count > 1 ? (Color)Arguments[1] : new Color(255d, 255d, 255d);
            var darkColor = Arguments.Count > 2 ? (Color)Arguments[2] : new Color(0d, 0d, 0d);
            var threshold = Arguments.Count > 3 ? ((Number) Arguments[3]).ToNumber() : 0.43d;

            var luma = (0.2126 * color.R / 255d) + (0.7152 * color.G / 255d) + (0.0722 * color.B / 255d);

            return (luma < threshold) ? lightColor : darkColor;
        }
    }
}