using System.Linq;
using dotless.Infrastructure;
using dotless.Tree;

namespace dotless.Functions
{
  public class GrayscaleFunction : ColorFunctionBase
  {
    protected override Node Eval(Color color)
    {
      var grey = (color.RGB.Max() + color.RGB.Min()) / 2;

      return new Color(grey, grey, grey);
    }
  }
}