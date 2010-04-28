using System.Linq;
using dotless.Tree;
using dotless.Utils;

namespace dotless.Infrastructure
{
  public abstract class NumberFunctionBase : Function
  {
    protected override Node Evaluate()
    {
      Guard.ExpectMinArguments(1, Arguments.Count, this);
      Guard.ExpectNode<Number>(Arguments[0], this);

      var number = Arguments[0] as Number;
      var args = Arguments.Skip(1).ToArray();

      return Eval(number, args);
    }

    protected abstract Node Eval(Number number, Node[] args);
  }

}