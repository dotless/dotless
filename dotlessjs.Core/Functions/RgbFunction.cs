using System.Collections.Generic;
using System.Linq;
using dotless.Infrastructure;
using dotless.Tree;
using dotless.Utils;

namespace dotless.Functions
{
  public class RgbFunction : RgbaFunction
  {
    protected override Node Evaluate()
    {
      Guard.ExpectNumArguments(Arguments, 3);

      Arguments.Add(new Number(1d, ""));

      return base.Evaluate();
    }
  }
}