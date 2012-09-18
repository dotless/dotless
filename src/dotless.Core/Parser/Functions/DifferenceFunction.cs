namespace dotless.Core.Parser.Functions
{
    using System;
    public class DifferenceFunction : ColorMixFunction
    {
        protected override double Operate(double a, double b)
        {
            return Math.Abs(a - b);
        }
    }
}
