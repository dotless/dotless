using dotless.Exceptions;
using dotless.Infrastructure;
using dotless.Tree;

namespace dotless.Functions
{
  public class PercentageFunction : NumberFunctionBase
  {
    protected override Node Eval(Number number, Node[] args)
    {
      if (number.Unit == "%")
        return number;

      if (string.IsNullOrEmpty(number.Unit))
        return new Number(number.Value * 100, "%");

      throw new ParsingException(string.Format("Expected unitless number in function 'percentage', found {0}", number.ToCSS()));
    }
  }
}