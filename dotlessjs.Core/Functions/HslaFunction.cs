using System.Collections.Generic;
using System.Linq;
using dotless.Infrastructure;
using dotless.Tree;
using dotless.Utils;

namespace dotless.Functions
{
  public class HslaFunction : Function
  {
    public override Node Call(IEnumerable<Node> arguments)
    {
      Guard.ExpectNumArguments(arguments, 4);
      Guard.ExpectNodes<Dimension>(arguments);

      var args = arguments.Cast<Dimension>().ToArray();

      return new HslColor(args[0], args[1], args[2], args[3]).ToRgbColor();
    }
  }
}