namespace dotless.Core.Parser.Functions
{
    using Infrastructure.Nodes;
    using Tree;

    public class GreenFunction : ColorFunctionBase
    {
        protected override Node Eval(Color color)
        {
            return new Number(color.G);
        }

        protected override Node EditColor(Color color, Number number)
        {
            var value = number.Value;

            if (number.Unit == "%")
                value = (value*255)/100d;

            return new Color(color.R, color.G + value, color.B);
        }
    }
}