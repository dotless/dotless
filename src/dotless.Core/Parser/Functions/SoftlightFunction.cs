namespace dotless.Core.Parser.Functions
{
    public class SoftlightFunction : ColorMixFunction
    {
        protected override double Operate(double a, double b)
        {
            double t = b * a / 255;
            return t + a * (255 - (255 - a) * (255 - b) / 255 - t) / 255;
        }
    }
}
