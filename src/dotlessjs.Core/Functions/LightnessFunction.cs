using dotless.Infrastructure;
using dotless.Tree;
using dotless.Utils;

namespace dotless.Functions
{
  public class LightnessFunction : HslColorFunctionBase
  {
    protected override Node EvalHsl(HslColor color)
    {
      return color.GetLightness();
    }

    protected override Node EditHsl(HslColor color, Number number)
    {
      color.Lightness += number.Value / 100;
      return color.ToRgbColor();
    }
  }
}