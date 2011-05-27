namespace dotless.Core.Parser.Functions 
{
    using Tree;
    using Utils;

    public class DarkenFunction : HslColorAdjustmentFunctionBase 
    {
        protected override void AdjustColor(HslColor color, Number adjustment) 
        {
            color.Lightness = Clamp(color.Lightness - adjustment.ToNumber());
        }
    }
}
