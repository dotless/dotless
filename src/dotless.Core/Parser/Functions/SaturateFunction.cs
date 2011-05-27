namespace dotless.Core.Parser.Functions 
{
    using Tree;
    using Utils;

    public class SaturateFunction : HslColorAdjustmentFunctionBase 
    {
        protected override void AdjustColor(HslColor color, Number adjustment) 
        {
            color.Saturation = Clamp(color.Saturation + adjustment.ToNumber());
        }
    }
}
