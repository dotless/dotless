namespace dotless.Core.Parser.Functions
{
    using Infrastructure.Nodes;
    using Tree;
    using Utils;

    public class ComplementFunction : HslColorFunctionBase
    {
        protected override Node EvalHsl(HslColor color)
        {
            color.Hue += 0.5;
            return color.ToRgbColor();
        }

        protected override Node EditHsl(HslColor color, Number number)
        {
            return null;
        }
    }
}