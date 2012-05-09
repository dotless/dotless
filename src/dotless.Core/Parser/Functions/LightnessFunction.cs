namespace dotless.Core.Parser.Functions
{
    using Infrastructure.Nodes;
    using Tree;
    using Utils;

    public class LightenFunction : HslColorFunctionBase
    {
        protected override Node EvalHsl(HslColor color)
        {
            return color.GetLightness();
        }

        protected override Node EditHsl(HslColor color, Number number)
        {
            color.Lightness += number.Value/100;
            return color.ToRgbColor();
        }
    }

    public class DarkenFunction : LightenFunction
    {
        protected override Node EditHsl(HslColor color, Number number)
        {
            return base.EditHsl(color, -number);
        }
    }

    public class LightnessFunction : LightenFunction
    {
        protected override Node EditHsl(HslColor color, Number number)
        {
            WarnNotSupportedByLessJS("lightness(color, number)", "lighten(color, number) or its opposite darken(color, number),");

            return base.EditHsl(color, number);
        }
    }
}