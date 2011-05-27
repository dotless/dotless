namespace dotless.Core.Parser.Functions 
{
    using Infrastructure.Nodes;
    using Tree;
    using Utils;

    public abstract class HslColorAdjustmentFunctionBase : HslColorFunctionBase 
    {
        protected override Node EvalHsl(HslColor color) 
        {
            // REVIEW: Not used. Should a different base class be used instead?
            return null;
        }

        protected override Node EditHsl(HslColor color, Number number) 
        {
            var newColor = new HslColor(color.Hue, color.Saturation, color.Lightness, color.Alpha);
            AdjustColor(newColor, number);
            return newColor.ToRgbColor();
        }

        protected abstract void AdjustColor(HslColor color, Number adjustment);

        protected static double Clamp(double value) 
        {
            return NumberExtensions.Normalize(value);
        }
    }
}