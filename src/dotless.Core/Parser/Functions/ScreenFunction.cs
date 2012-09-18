namespace dotless.Core.Parser.Functions
{
    public class ScreenFunction : ColorMixFunction
    {
        protected override double Operate(double a, double b)
        {
            return 255 - (255 - a) * (255 - b) / 255;
        }
    }
}
