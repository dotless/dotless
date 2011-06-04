namespace dotless.Core.Parser.Functions
{
    using Exceptions;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;

    public class HexFunction : NumberFunctionBase
    {
        protected override Node Eval(Env env, Number number, Node[] args)
        {
            if (!string.IsNullOrEmpty(number.Unit))
                throw new ParsingException(string.Format("Expected unitless number in function 'hex', found {0}", number.ToCSS(env)), number.Index);

            number.Value = Clamp(number.Value, 255, 0);

            return new TextNode(((int)number.Value).ToString("X2"));
        }

        private static double Clamp(double value, double max, double min)
        {
            if (value < min) value = min;
            if (value > max) value = max;
            return value;
        }
    }
}