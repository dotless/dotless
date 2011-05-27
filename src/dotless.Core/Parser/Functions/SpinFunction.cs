namespace dotless.Core.Parser.Functions 
{
    using Tree;
    using Utils;

    public class SpinFunction : HslColorAdjustmentFunctionBase 
    {
        protected override void AdjustColor(HslColor color, Number adjustment) 
        {
            // Make sure the hue is positive within 360 degrees and then scale it to [0,1)
            color.Hue = ((360 + (color.GetHueInDegrees().Value + adjustment.Value) % 360) % 360) / 360;
        }
    }
}
