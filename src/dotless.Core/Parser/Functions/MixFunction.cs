namespace dotless.Core.Parser.Functions
{
    using System.Linq;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;
    using Utils;

    public class MixFunction : Function
    {
        protected override Node Evaluate(Env env)
        {
            Guard.ExpectMinArguments(2, Arguments.Count, this, Location);
            Guard.ExpectMaxArguments(3, Arguments.Count, this, Location);
            Guard.ExpectAllNodes<Color>(Arguments.Take(2), this, Location);

            double weight = 50;
            if (Arguments.Count == 3)
            {
                Guard.ExpectNode<Number>(Arguments[2], this, Location);

                weight = ((Number)Arguments[2]).Value;
            }

            var colors = Arguments.Take(2).Cast<Color>().ToArray();

            return Mix(colors[0], colors[1], weight);
        }

        protected Color Mix(Color color1, Color color2, double weight)
        {
            // Note: this algorithm taken from http://github.com/nex3/haml/blob/0e249c844f66bd0872ed68d99de22b774794e967/lib/sass/script/functions.rb

            var p = weight / 100.0;
            var w = p * 2 - 1;
            var a = color1.Alpha - color2.Alpha;

            var w1 = (((w * a == -1) ? w : (w + a) / (1 + w * a)) + 1) / 2.0;
            var w2 = 1 - w1;

            var rgb = color1.RGB.Select((x, i) => x * w1 + color2.RGB[i] * w2).ToArray();

            var alpha = color1.Alpha * p + color2.Alpha * (1 - p);

            var color = new Color(rgb[0], rgb[1], rgb[2], alpha);
            return color;
        }
    }

    public class TintFunction : MixFunction
    {
        protected override Node Evaluate(Env env)
        {
            Guard.ExpectNumArguments(2, Arguments.Count, this, Location);
            Guard.ExpectNode<Color>(Arguments[0], this, Location);
            Guard.ExpectNode<Number>(Arguments[1], this, Location);

            double weight = ((Number)Arguments[1]).Value;

            return Mix(new Color(255, 255, 255),(Color)Arguments[0], weight);
        }
    }

    public class ShadeFunction : MixFunction
    {
        protected override Node Evaluate(Env env)
        {
            Guard.ExpectNumArguments(2, Arguments.Count, this, Location);
            Guard.ExpectNode<Color>(Arguments[0], this, Location);
            Guard.ExpectNode<Number>(Arguments[1], this, Location);

            double weight = ((Number)Arguments[1]).Value;

            return Mix(new Color(0, 0, 0), (Color)Arguments[0], weight);
        }
    }
}