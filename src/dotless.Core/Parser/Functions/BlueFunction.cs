namespace dotless.Core.Parser.Functions
{
    using Infrastructure.Nodes;
    using Tree;

    public class BlueFunction : ColorFunctionBase
    {
        protected override Node Eval(Color color)
        {
            return new Number(color.B);
        }

        protected override Node EditColor(Color color, Number number)
        {
            var value = number.Value;

            if (number.Unit == "%")
                value = (value*255)/100d;

            return new Color(color.R, color.G, color.B + value);
        }
    }
}