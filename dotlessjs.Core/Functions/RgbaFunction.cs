using System.Collections.Generic;
using System.Linq;
using dotless.Infrastructure;
using dotless.Tree;
using dotless.Utils;

namespace dotless.Functions
{
  public class RgbaFunction : Function
  {
    protected override Node Evaluate()
    {
      Guard.ExpectNumArguments(Arguments, 4);
      Guard.ExpectNodes<Number>(Arguments);

      var args = Arguments.Cast<Number>();

      var rgb = args.Take(3);

      return new Color(rgb, args.ElementAt(3));
    }
  }
}