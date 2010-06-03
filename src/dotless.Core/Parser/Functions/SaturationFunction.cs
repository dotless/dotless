using dotless.Infrastructure;
using dotless.Tree;
using dotless.Utils;

namespace dotless.Functions
{
  public class SaturationFunction : HslColorFunctionBase
  {
    protected override Node EvalHsl(HslColor color)
    {
      return color.GetSaturation();
    }

    protected override Node EditHsl(HslColor color, Number number)
    {
      color.Saturation += number.Value / 100;
      return color.ToRgbColor();
    }
  }
}