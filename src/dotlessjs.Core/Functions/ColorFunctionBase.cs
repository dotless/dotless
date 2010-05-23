using System.Linq;
using dotless.Infrastructure;
using dotless.Tree;
using dotless.Utils;

namespace dotless.Functions
{
  public abstract class ColorFunctionBase : Function
  {
    protected override Node Evaluate()
    {
      Guard.ExpectMinArguments(1, Arguments.Count(), this);
      Guard.ExpectNode<Color>(Arguments[0], this);

      var color = Arguments[0] as Color;

      if (Arguments.Count == 2)
      {
        Guard.ExpectNode<Number>(Arguments[1], this);

        var number = Arguments[1] as Number;
        var edit = EditColor(color, number);

        if (edit != null)
          return edit;
      }

      return Eval(color);
    }

    protected abstract Node Eval(Color color);

    protected virtual Node EditColor(Color color, Number number)
    {
      return null;
    }
  }
}