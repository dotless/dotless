using System;

namespace dotless.Core.Parser.Functions
{
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;
    using Utils;

    public abstract class ColorMixFunction : Function
    {
        protected override Node Evaluate(Env env)
        {
            Guard.ExpectNumArguments(2, Arguments.Count, this, Location);
            Guard.ExpectAllNodes<Color>(Arguments, this, Location);

            var color1 = (Color) Arguments[0];
            var color2 = (Color) Arguments[1];

            var resultAlpha = color2.Alpha + color1.Alpha*(1 - color2.Alpha);

            return new Color(
                Compose(color1, color2, resultAlpha, c => c.R),
                Compose(color1, color2, resultAlpha, c => c.G),
                Compose(color1, color2, resultAlpha, c => c.B),
                resultAlpha);
        }

        /// <summary>
        /// Color composition rules from http://www.w3.org/TR/compositing-1/
        /// (translated from the less.js version
        /// </summary>
        private double Compose(Color backdrop, Color source, double ar, Func<Color, double> channel) {
            var cb = channel(backdrop);
            var cs = channel(source);
            var ab = backdrop.Alpha;
            var @as  = source.Alpha;
            double result = Operate(cb, cs);
            if (ar > 0)
            {
                result = (@as * cs + ab * (cb - @as * (cb + cs - result))) / ar;
            }
            return result;
        }

        protected abstract double Operate(double a, double b);
    }
}