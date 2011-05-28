namespace dotless.Core.Parser.Functions
{
    using Infrastructure.Nodes;
    using Tree;
    using Utils;

    public class HueFunction : HslColorFunctionBase
    {
        protected override Node EvalHsl(HslColor color)
        {
            return color.GetHueInDegrees();
        }

        protected override Node EditHsl(HslColor color, Number number)
        {
            color.Hue += number.Value/360d;
            return color.ToRgbColor();
        }
    }

    public class SpinFunction : HueFunction { }
}