namespace dotless.Core.Parser.Functions 
{
    using Infrastructure.Nodes;
    using Tree;
    using Utils;

    public class SpinFunction : HslColorFunctionBase 
    {
        protected override Infrastructure.Nodes.Node EvalHsl(HslColor color) 
        {
            // TODO: what should this be?
            return null;
        }

        protected override Node EditHsl(HslColor color, Number number) 
        {
            // Make sure the hue is positive within 360 degrees and then scale it to [0,1)
            color.Hue = ((360 + (color.GetHueInDegrees().Value + number.Value) % 360) % 360) / 360;
            return color.ToRgbColor();
        }
    }
}
