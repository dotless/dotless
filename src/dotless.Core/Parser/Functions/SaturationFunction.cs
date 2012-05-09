namespace dotless.Core.Parser.Functions
{
    using Infrastructure.Nodes;
    using Tree;
    using Utils;

    public class SaturateFunction : HslColorFunctionBase
    {
        protected override Node EvalHsl(HslColor color)
        {
            return color.GetSaturation();
        }

        protected override Node EditHsl(HslColor color, Number number)
        {
            color.Saturation += number.Value/100;
            return color.ToRgbColor();
        }
    }

    public class DesaturateFunction : SaturateFunction
    {
        protected override Node EditHsl(HslColor color, Number number)
        {
            return base.EditHsl(color, -number);
        }
    }

    public class SaturationFunction : SaturateFunction 
    {
        protected override Node EditHsl(HslColor color, Number number)
        {
            WarnNotSupportedByLessJS("saturation(color, number)", "saturate(color, number) or its opposite desaturate(color, number),");

            return base.EditHsl(color, number);
        }
    }

}