namespace dotless.Core.Parser.Functions 
{
    using Tree;
    using Utils;

    public class LightenFunction : HslColorAdjustmentFunctionBase 
    {
        protected override void AdjustColor(HslColor color, Number adjustment) 
        {
            color.Lightness = Clamp(color.Lightness + adjustment.ToNumber());
        }
    }
}
