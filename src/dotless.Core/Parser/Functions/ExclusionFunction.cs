namespace dotless.Core.Parser.Functions
{
    public class ExclusionFunction : ColorMixFunction
    {
        protected override double Operate(double a, double b)
        {
            return a + b * (255 - a - a) / 255;
        }
    }
}
