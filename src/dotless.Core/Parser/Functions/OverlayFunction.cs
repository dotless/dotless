namespace dotless.Core.Parser.Functions
{
    public class OverlayFunction : ColorMixFunction
    {
        protected override double Operate(double a, double b)
        {
            return a < 128 ? 2 * a * b / 255 : 255 - 2 * (255 - a) * (255 - b) / 255;
        }
    }
}
