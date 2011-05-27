namespace dotless.Core.Parser.Functions 
{
    using Tree;
    using Utils;

    public class DesaturateFunction : HslColorAdjustmentFunctionBase 
    {
        protected override void AdjustColor(HslColor color, Number adjustment) 
        {
            color.Saturation = Clamp(color.Saturation - adjustment.ToNumber());
        }
    }
}
