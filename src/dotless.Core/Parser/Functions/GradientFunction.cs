namespace dotless.Core.Parser.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using Exceptions;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;
    using Utils;
    using Color = Tree.Color;

    /// <summary>
    /// Outputs <see cref="Url"/> node with gradient image implemented as described in RFC-2397 (The "data" URL scheme, http://tools.ietf.org/html/rfc2397)
    /// </summary>
    /// <remarks>
    /// Acepts color point definitions and outputs image data URL. Usage:
    /// <c>gradient(#color1, #color2[, position2][, #color3[, position3]]...)</c>
    /// Position is a zero-based offset of the color stop. If omitted, it is increased by 50 (px) or by the previous offset, if any.
    /// 
    /// Currently only vertical 1px width gradients with many color stops are supported.
    /// Image data URLs are not supported by IE7 and below. IE8 restricts the size of URL to 32k.
    /// See details here: http://en.wikipedia.org/wiki/Data_URI_scheme.
    /// <example>
    /// The following example shows how to render a gradient (.less file):
    /// <code>
    /// .ui-widget-header { background: @headerBgColor gradient(@headerBgColor, desaturate(lighten(@headerBgColor, 30), 30)) 50% 50% repeat-x; }
    /// </code>
    /// </example>
    /// </remarks>
    public class GradientFunction : Function
    {
        /*
         * TODO: 
         * 1. Add URI length check for IE8: max 0x8000
         * 2. Add fallback for IE7 and lower - dump the image to disk and refer it as ordinal url(...) - needs access to config and Context.Request.Browser + disk write permissions.
         * 3. Implement horisontal gradients (1px height)
         * 
         * Open questions:
         * 1. PNG 32bpp - is it ok for all cases? 
         * 2. Is it required to cache images?
         */

        private class ColorPoint
        {
            public ColorPoint(System.Drawing.Color color, int position)
            {
                Color = color;
                Position = position;
            }

            public static string Stringify(IEnumerable<ColorPoint> points)
            {
                return points.Aggregate("",
                                 (s, point) =>
                                 string.Format("{0}{1}#{2:X}{3:X}{4:X}{5:X},{6}", s, s == "" ? "" : ",",
                                               point.Color.A, point.Color.R, point.Color.G, point.Color.B, point.Position));
            }

            public System.Drawing.Color Color { get; private set; }
            public int Position { get; private set; }
        }

        private class CacheItem
        {
            private readonly string _def;
            private readonly string _url;

            public CacheItem(string def, string url)
            {
                _def = def;
                _url = url;
            }
        }

        public const int DEFAULT_COLOR_OFFSET = 50;


        protected override Node Evaluate(Env env)
        {
            ColorPoint[] points = GetColorPoints();

            WarnNotSupportedByLessJS("gradient(color, color[, position])");

            string colorDefs = ColorPoint.Stringify(points);
            string imageUrl = GetFromCache(colorDefs);
            if (imageUrl == null)
            {
                imageUrl = "data:image/png;base64," + Convert.ToBase64String(GetImageData(points));
                AddToCache(colorDefs, imageUrl);
            }
            
            return new Url(new TextNode(imageUrl));
        }

        private byte[] GetImageData(ColorPoint[] points)
        {
            ColorPoint last = points.Last();
            int size = last.Position + 1;

            using (var ms = new MemoryStream())
            {
                using (var bmp = new Bitmap(1, size, PixelFormat.Format32bppArgb))
                {
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        for (int i = 1; i < points.Length; i++)
                        {
                            var rect = new Rectangle(0, points[i - 1].Position, 1, points[i].Position);
                            var brush = new LinearGradientBrush(
                                rect,
                                points[i - 1].Color,
                                points[i].Color,
                                LinearGradientMode.Vertical);
                            g.FillRectangle(brush, rect);
                        }

                        bmp.SetPixel(0, last.Position, last.Color);
                        bmp.Save(ms, ImageFormat.Png);
                    }
                }
                return ms.ToArray();
            }
        }

        private ColorPoint[] GetColorPoints()
        {
            int argCount = Arguments.Count;
            Guard.ExpectMinArguments(2, argCount, this, Location);
            Guard.ExpectAllNodes<Color>(Arguments.Take(2), this, Location);
            var first = (Color) Arguments[0];
            var points = new List<ColorPoint>
                {
                    new ColorPoint((System.Drawing.Color) first, 0)
                };

            int prevPos = 0;
            int prevOffset = DEFAULT_COLOR_OFFSET;

            for (int i = 1; i < argCount; i++)
            {
                Node arg = Arguments[i];
                Guard.ExpectNode<Color>(arg, this, Location);
                var color = arg as Color;
                int pos = prevPos + prevOffset;
                if (i < argCount - 1)
                {
                    var numberArg = Arguments[i + 1] as Number;
                    if (numberArg)
                    {
                        pos = Convert.ToInt32(numberArg.Value);
                        if (pos <= prevPos)
                        {
                            throw new ParsingException(
                                string.Format("Incrementing color point position expected, at least {0}, found {1}", prevPos + 1, numberArg.Value), Location);
                        }
                        prevOffset = pos - prevPos;
                        i++;
                    }
                }
                points.Add(new ColorPoint((System.Drawing.Color) color, pos));
                prevPos = pos;
            }

            return points.ToArray();
        }

        private static string GetFromCache(string colorDefs)
        {
            return null;
        }

        private static void AddToCache(string colorDefs, string imageUrl)
        {
            var item = new CacheItem(colorDefs, imageUrl);
        }
    }
}