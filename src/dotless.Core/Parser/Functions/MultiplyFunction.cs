namespace dotless.Core.Parser.Functions
{
    public class MultiplyFunction : ColorMixFunction
    {
        protected override double Operate(double a, double b)
        {
            return a * b / 255;
        }
    }
}
