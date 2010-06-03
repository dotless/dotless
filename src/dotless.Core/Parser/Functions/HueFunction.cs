using dotless.Infrastructure;
using dotless.Tree;
using dotless.Utils;

namespace dotless.Functions
{
  public class HueFunction : HslColorFunctionBase
  {
    protected override Node EvalHsl(HslColor color)
    {
      return color.GetHueInDegrees();
    }

    protected override Node EditHsl(HslColor color, Number number)
    {
      color.Hue += number.Value / 360d;
      return color.ToRgbColor();
    }
  }
}