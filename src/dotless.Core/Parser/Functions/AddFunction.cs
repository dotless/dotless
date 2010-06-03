using System.Linq;
using dotless.Infrastructure;
using dotless.Tree;
using dotless.Utils;

namespace dotless.Functions
{
    public class AddFunction : Function
    {
      protected override Node Evaluate()
      {
        Guard.ExpectAllNodes<Number>(Arguments, this);

        var value = Arguments.Cast<Number>().Select(d => d.Value).Aggregate(0d, (a, b) => a + b);

        return new Number(value);
      }
    }
}
