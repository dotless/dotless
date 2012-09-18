namespace dotless.Core.Parser.Functions
{
    public class HardlightFunction : ColorMixFunction
    {
        protected override double Operate(double a, double b)
        {
            return b < 128 ? 2 * b * a / 255 : 255 - 2 * (255 - b) * (255 - a) / 255;
        }
    }
}
