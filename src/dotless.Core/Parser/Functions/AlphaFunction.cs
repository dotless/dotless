namespace dotless.Core.Parser.Functions
{
    using Infrastructure.Nodes;
    using Tree;

    public class AlphaFunction : ColorFunctionBase
    {
        protected override Node Eval(Color color)
        {
            return new Number(color.Alpha);
        }

        protected override Node EditColor(Color color, Number number)
        {
            var alpha = number.Value;

            if (number.Unit == "%")
                alpha = alpha/100d;

            return new Color(color.R, color.G, color.B, color.Alpha + alpha);
        }
    }

    public class FadeInFunction : AlphaFunction {}

    public class FadeOutFunction : AlphaFunction
    {
        protected override Node EditColor(Color color, Number number)
        {
            return base.EditColor(color, -number);
        }
    }
}