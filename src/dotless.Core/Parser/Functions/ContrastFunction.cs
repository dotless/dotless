namespace dotless.Core.Parser.Functions
{
  using Infrastructure.Nodes;
  using Tree;
  using Utils;

  public class ContrastFunction : HslColorFunctionBase
  {
    protected override Node EvalHsl(HslColor color)
    {
      return (color.Lightness < 0.5d) ? new Color(255d, 255d, 255d) : new Color(0d, 0d, 0d);
    }

    protected override Node EditHsl(HslColor color, Number number)
    {
      return null;
    }
  }
}