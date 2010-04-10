using System.Collections.Generic;
using System.Linq;
using dotless.Infrastructure;
using dotless.Tree;
using dotless.Utils;

namespace dotless.Functions
{
  public class HslaFunction : Function
  {
    protected override Node Evaluate()
    {
      Guard.ExpectNumArguments(Arguments, 4);
      Guard.ExpectNodes<Number>(Arguments);

      var args = Arguments.Cast<Number>().ToArray();

      return new HslColor(args[0], args[1], args[2], args[3]).ToRgbColor();
    }
  }
}