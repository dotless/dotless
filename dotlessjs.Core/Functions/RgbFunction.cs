using System.Collections.Generic;
using System.Linq;
using dotless.Infrastructure;
using dotless.Tree;
using dotless.Utils;

namespace dotless.Functions
{
  public class RgbFunction : RgbaFunction
  {
    public override Node Call(IEnumerable<Node> arguments)
    {
      Guard.ExpectNumArguments(arguments, 3);

      return base.Call(arguments.Concat(new[] {new Dimension(1d, "")}));
    }
  }
}