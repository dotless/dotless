namespace dotless.Core.Parser.Functions
{
    using Infrastructure.Nodes;
    using Tree;
    using Utils;

    public abstract class HslColorFunctionBase : ColorFunctionBase
    {
        protected override Node Eval(Color color)
        {
            var hsl = HslColor.FromRgbColor(color);

            return EvalHsl(hsl);
        }

        protected override Node EditColor(Color color, Number number)
        {
            var hsl = HslColor.FromRgbColor(color);

            return EditHsl(hsl, number);
        }

        protected abstract Node EvalHsl(HslColor color);

        protected abstract Node EditHsl(HslColor color, Number number);
    }
}