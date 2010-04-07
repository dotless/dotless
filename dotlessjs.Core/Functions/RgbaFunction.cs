using System.Collections.Generic;
using System.Linq;
using dotless.Infrastructure;
using dotless.Tree;
using dotless.Utils;

namespace dotless.Functions
{
  public class RgbaFunction : Function
  {
    public override Node Call(IEnumerable<Node> arguments)
    {
      Guard.ExpectNumArguments(arguments, 4);
      Guard.ExpectNodes<Dimension>(arguments);

      var args = arguments.Cast<Dimension>();

      var rgb = args.Take(3);

      return new Color(rgb, args.ElementAt(3));
    }
  }
}