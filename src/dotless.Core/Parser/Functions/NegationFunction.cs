namespace dotless.Core.Parser.Functions
{
    using System;
    public class NegationFunction : ColorMixFunction
    {
        protected override double Operate(double a, double b)
        {
            return 255 - Math.Abs(255 - b - a);
        }
    }
}
