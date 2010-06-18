namespace dotless.Core.Parser.Functions
{
    using Exceptions;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;

    public class PercentageFunction : NumberFunctionBase
    {
        protected override Node Eval(Env env, Number number, Node[] args)
        {
            if (number.Unit == "%")
                return number;

            if (string.IsNullOrEmpty(number.Unit))
                return new Number(number.Value * 100, "%");

            throw new ParsingException(string.Format("Expected unitless number in function 'percentage', found {0}", number.ToCSS(env)), number.Index);
        }
    }
}