using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using dotless.Infrastructure;
using dotless.Utils;

namespace dotless.Tree
{
  public class Color : Node, IOperable
  {
    public readonly double[] RGB;
    public readonly double Alpha;

    public Color(double[] rgb) : this(rgb, 1)
    {
    }

    public Color(double[] rgb, double alpha)
    {
      RGB = rgb;
      Alpha = alpha;
    }

    public Color(IEnumerable<Number> rgb, Number alpha)
    {
      RGB = rgb
        .Select(d => d.Normalize(255d))
        .ToArray();

      Alpha = alpha.Normalize();
    }

    public Color(string rgb)
    {
      if (rgb.Length == 6)
      {
        RGB = Enumerable.Range(0, 3)
          .Select(i => rgb.Substring(i*2, 2))
          .Select(s => (double)int.Parse(s, NumberStyles.HexNumber))
          .ToArray();
      }
      else
      {
        RGB = rgb.ToCharArray()
          .Select(c => (double)int.Parse("" + c + c, NumberStyles.HexNumber))
          .ToArray();
      }
      Alpha = 1;
    }

    public Color(double red, double green, double blue, double alpha)
    {
      RGB = new[]
              {
                NumberExtensions.Normalize(red, 255d),
                NumberExtensions.Normalize(green, 255d),
                NumberExtensions.Normalize(blue, 255d)
              };

      Alpha = NumberExtensions.Normalize(alpha);
    }

    public override string ToCSS(Env env)
    {
      if (Alpha > 0.0 && Alpha < 1.0)
        return string.Format("rgba({0}, {1}, {2}, {3})", RGB[0], RGB[1], RGB[2], Alpha);

      return '#' + RGB
                     .Select(d => (int) Math.Round(d, MidpointRounding.AwayFromZero))
                     .Select(i => i > 255 ? 255 : (i < 0 ? 0 : i))
                     .Select(i => i.ToString("X2").ToLowerInvariant())
                     .JoinStrings("");
    }

    public Node Operate(string op, Node other)
    {
      var otherColor = other as Color;

      if (otherColor == null)
        otherColor = ((IOperable)other).ToColor();

      var result = new double[3];
      for (var c = 0; c < 3; c++)
      {
        result[c] = Operation.Operate(op, RGB[c], otherColor.RGB[c]);
      }
      return new Color(result);
    }

    public Color ToColor()
    {
      return this;
    }
  }
}