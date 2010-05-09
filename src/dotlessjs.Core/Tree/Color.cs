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
    private static readonly Dictionary<int, string> Html4ColorsReverse;

    private static readonly Dictionary<string, int> Html4Colors =
        new Dictionary<string, int>
                {
                    {"black",   0x000000},
                    {"silver",  0xc0c0c0},
                    {"gray",    0x808080},
                    {"white",   0xffffff},
                    {"maroon",  0x800000},
                    {"red",     0xff0000},
                    {"purple",  0x800080},
                    {"fuchsia", 0xff00ff},
                    {"green",   0x008000},
                    {"lime",    0x00ff00},
                    {"olive",   0x808000},
                    {"yellow",  0xffff00},
                    {"navy",    0x000080},
                    {"blue",    0x0000ff},
                    {"teal",    0x008080},
                    {"aqua",    0x00ffff}
                };
    static Color()
    {
      Html4ColorsReverse = Html4Colors.ToDictionary(x => x.Value, x => x.Key);
    }

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

    public Color(double red, double green, double blue)
      : this(red, green, blue, 1d)
    {
    }

    public Color(int color)
    {
      RGB = new double[3];

      B = color & 0xff;
      color >>= 8;
      G = color & 0xff;
      color >>= 8;
      R = color & 0xff;
      Alpha = 1;
    }

    public double R
    {
      get { return RGB[0]; }
      set { RGB[0] = value; }
    }

    public double G
    {
      get { return RGB[1]; }
      set { RGB[1] = value; }
    }

    public double B
    {
      get { return RGB[2]; }
      set { RGB[2] = value; }
    }

    public override string ToCSS()
    {
      var rgb = RGB
        .Select(d => (int) Math.Round(d, MidpointRounding.AwayFromZero))
        .Select(i => i > 255 ? 255 : (i < 0 ? 0 : i))
        .ToArray();

      if (Alpha < 1.0)
        return string.Format("rgba({0}, {1}, {2}, {3})", rgb[0], rgb[1], rgb[2], Alpha);

      var keyword = GetKeyword(rgb);

      if (!string.IsNullOrEmpty(keyword))
        return keyword;

      return '#' + rgb
                     .Select(i => i.ToString("X2"))
                     .JoinStrings("")
                     .ToLowerInvariant();
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

    public string GetKeyword(int[] rgb)
    {
      var color = (rgb[0] << 16) + (rgb[1] << 8) + rgb[2];
      string keyword;

      if (Html4ColorsReverse.TryGetValue(color, out keyword))
        return keyword;

      return null;
    }

    public static Color GetColorFromKeyword(string keyword)
    {
      int color;

      if (Html4Colors.TryGetValue(keyword, out color))
        return new Color(color);

      return null;
    }
  }
}