namespace dotless.Core.Parser.Tree
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Exceptions;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Utils;

    public class Color : Node, IOperable, IComparable
    {
        private static readonly Dictionary<string, int> Html4Colors = new Dictionary<string, int>
        {
            {"aliceblue", 0xf0f8ff},
            {"antiquewhite", 0xfaebd7},
            {"aqua", 0x00ffff},
            {"aquamarine", 0x7fffd4},
            {"azure", 0xf0ffff},
            {"beige", 0xf5f5dc},
            {"bisque", 0xffe4c4},
            {"black", 0x000000},
            {"blanchedalmond", 0xffebcd},
            {"blue", 0x0000ff},
            {"blueviolet", 0x8a2be2},
            {"brown", 0xa52a2a},
            {"burlywood", 0xdeb887},
            {"cadetblue", 0x5f9ea0},
            {"chartreuse", 0x7fff00},
            {"chocolate", 0xd2691e},
            {"coral", 0xff7f50},
            {"cornflowerblue", 0x6495ed},
            {"cornsilk", 0xfff8dc},
            {"crimson", 0xdc143c},
            {"cyan", 0x00ffff},
            {"darkblue", 0x00008b},
            {"darkcyan", 0x008b8b},
            {"darkgoldenrod", 0xb8860b},
            {"darkgray", 0xa9a9a9},
            {"darkgrey", 0xa9a9a9},
            {"darkgreen", 0x006400},
            {"darkkhaki", 0xbdb76b},
            {"darkmagenta", 0x8b008b},
            {"darkolivegreen", 0x556b2f},
            {"darkorange", 0xff8c00},
            {"darkorchid", 0x9932cc},
            {"darkred", 0x8b0000},
            {"darksalmon", 0xe9967a},
            {"darkseagreen", 0x8fbc8f},
            {"darkslateblue", 0x483d8b},
            {"darkslategray", 0x2f4f4f},
            {"darkslategrey", 0x2f4f4f},
            {"darkturquoise", 0x00ced1},
            {"darkviolet", 0x9400d3},
            {"deeppink", 0xff1493},
            {"deepskyblue", 0x00bfff},
            {"dimgray", 0x696969},
            {"dimgrey", 0x696969},
            {"dodgerblue", 0x1e90ff},
            {"firebrick", 0xb22222},
            {"floralwhite", 0xfffaf0},
            {"forestgreen", 0x228b22},
            {"fuchsia", 0xff00ff},
            {"gainsboro", 0xdcdcdc},
            {"ghostwhite", 0xf8f8ff},
            {"gold", 0xffd700},
            {"goldenrod", 0xdaa520},
            {"gray", 0x808080},
            {"grey", 0x808080},
            {"green", 0x008000},
            {"greenyellow", 0xadff2f},
            {"honeydew", 0xf0fff0},
            {"hotpink", 0xff69b4},
            {"indianred", 0xcd5c5c},
            {"indigo", 0x4b0082},
            {"ivory", 0xfffff0},
            {"khaki", 0xf0e68c},
            {"lavender", 0xe6e6fa},
            {"lavenderblush", 0xfff0f5},
            {"lawngreen", 0x7cfc00},
            {"lemonchiffon", 0xfffacd},
            {"lightblue", 0xadd8e6},
            {"lightcoral", 0xf08080},
            {"lightcyan", 0xe0ffff},
            {"lightgoldenrodyellow", 0xfafad2},
            {"lightgray", 0xd3d3d3},
            {"lightgrey", 0xd3d3d3},
            {"lightgreen", 0x90ee90},
            {"lightpink", 0xffb6c1},
            {"lightsalmon", 0xffa07a},
            {"lightseagreen", 0x20b2aa},
            {"lightskyblue", 0x87cefa},
            {"lightslategray", 0x778899},
            {"lightslategrey", 0x778899},
            {"lightsteelblue", 0xb0c4de},
            {"lightyellow", 0xffffe0},
            {"lime", 0x00ff00},
            {"limegreen", 0x32cd32},
            {"linen", 0xfaf0e6},
            {"magenta", 0xff00ff},
            {"maroon", 0x800000},
            {"mediumaquamarine", 0x66cdaa},
            {"mediumblue", 0x0000cd},
            {"mediumorchid", 0xba55d3},
            {"mediumpurple", 0x9370d8},
            {"mediumseagreen", 0x3cb371},
            {"mediumslateblue", 0x7b68ee},
            {"mediumspringgreen", 0x00fa9a},
            {"mediumturquoise", 0x48d1cc},
            {"mediumvioletred", 0xc71585},
            {"midnightblue", 0x191970},
            {"mintcream", 0xf5fffa},
            {"mistyrose", 0xffe4e1},
            {"moccasin", 0xffe4b5},
            {"navajowhite", 0xffdead},
            {"navy", 0x000080},
            {"oldlace", 0xfdf5e6},
            {"olive", 0x808000},
            {"olivedrab", 0x6b8e23},
            {"orange", 0xffa500},
            {"orangered", 0xff4500},
            {"orchid", 0xda70d6},
            {"palegoldenrod", 0xeee8aa},
            {"palegreen", 0x98fb98},
            {"paleturquoise", 0xafeeee},
            {"palevioletred", 0xd87093},
            {"papayawhip", 0xffefd5},
            {"peachpuff", 0xffdab9},
            {"peru", 0xcd853f},
            {"pink", 0xffc0cb},
            {"plum", 0xdda0dd},
            {"powderblue", 0xb0e0e6},
            {"purple", 0x800080},
            {"red", 0xff0000},
            {"rosybrown", 0xbc8f8f},
            {"royalblue", 0x4169e1},
            {"saddlebrown", 0x8b4513},
            {"salmon", 0xfa8072},
            {"sandybrown", 0xf4a460},
            {"seagreen", 0x2e8b57},
            {"seashell", 0xfff5ee},
            {"sienna", 0xa0522d},
            {"silver", 0xc0c0c0},
            {"skyblue", 0x87ceeb},
            {"slateblue", 0x6a5acd},
            {"slategray", 0x708090},
            {"slategrey", 0x708090},
            {"snow", 0xfffafa},
            {"springgreen", 0x00ff7f},
            {"steelblue", 0x4682b4},
            {"tan", 0xd2b48c},
            {"teal", 0x008080},
            {"thistle", 0xd8bfd8},
            {"tomato", 0xff6347},
            {"turquoise", 0x40e0d0},
            {"violet", 0xee82ee},
            {"wheat", 0xf5deb3},
            {"white", 0xffffff},
            {"whitesmoke", 0xf5f5f5},
            {"yellow", 0xffff00},
            {"yellowgreen", 0x9acd32}
        };

        public static Color From(string keywordOrHex)
        {
            return FromKeyword(keywordOrHex) ?? FromHex(keywordOrHex);
        }

        public static Color FromKeyword(string keyword)
        {
            if (keyword == "transparent")
            {
                return new Color(0x000, 0.0, keyword);
            }

            int hex;
            if (Html4Colors.TryGetValue(keyword, out hex))
            {
                return new Color(hex, 1.0, keyword);
            }

            return null;
        }

        public static Color FromHex(string hex)
        {
            hex = hex.TrimStart('#');
            double[] rgb;
            var alpha = 1.0;
            var text = '#' + hex;

            if (hex.Length == 8)
            {
                rgb = ParseRgb(hex.Substring(2));
                alpha = Parse(hex.Substring(0, 2))/255.0;
            }
            else if (hex.Length == 6)
            {
                rgb = ParseRgb(hex);
            }
            else
            {
                rgb = hex.ToCharArray().Select(c => Parse("" + c + c)).ToArray();
            }

            return new Color(rgb, alpha, text);
        }

        private static double[] ParseRgb(string hex)
        {
            return Enumerable.Range(0, 3)
                .Select(i => hex.Substring(i*2, 2))
                .Select(Parse)
                .ToArray();
        }

        private static double Parse(string hex)
        {
            return int.Parse(hex, NumberStyles.HexNumber);
        }

        private static int ComputeRgb(double[] rgb)
        {
            var c = rgb.Select(x => NumberExtensions.Normalize(x, 255.0))
                .Select(x => (int) Math.Round(x, MidpointRounding.AwayFromZero))
                .ToList();
            return (c[0] << 16) | (c[1] << 8) | c[2];
        }

        private static double ComputeAlpha(double alpha)
        {
            return NumberExtensions.Normalize(alpha, 1.0);
        }

        private readonly int _rgb;
        private readonly string _text;

        private Color(int rgb, double alpha, string text)
        {
            _rgb = rgb;
            RGB = new double[3];
            RGB[0] = rgb >> 16 & 0xFF;
            RGB[1] = rgb >> 8 & 0xFF;
            RGB[2] = rgb & 0xFF;

            Alpha = alpha;
            _text = text;
        }

        public Color(double[] rgb, double alpha = 1.0, string text = null)
            : this (ComputeRgb(rgb), ComputeAlpha(alpha), text)
        {
            RGB = rgb.Select(c => NumberExtensions.Normalize(c, 255.0)).ToArray();
        }

        public Color(double red, double green, double blue, double alpha = 1.0, string text = null)
            : this(new[] {red, green, blue}, alpha, text)
        {
        }

        public double[] RGB { get; private set; }

        public double Alpha { get; private set; }

        public double R
        {
            get { return RGB[0]; }
        }

        public double G
        {
            get { return RGB[1]; }
        }

        public double B
        {
            get { return RGB[2]; }
        }

        /// <summary>
        /// Transforms the linear to SRBG. Formula derivation decscribed <a href="http://en.wikipedia.org/wiki/SRGB#Theory_of_the_transformation">here</a>
        /// </summary>
        /// <param name="linearChannel">The linear channel, for example R/255</param>
        /// <returns>The sRBG value for the given channel</returns>
        private double TransformLinearToSrbg(double linearChannel)
        {
            const double decodingGamma = 2.4;
            const double phi = 12.92;
            const double alpha = .055;
            return (linearChannel <= 0.03928) ? linearChannel / phi : Math.Pow(((linearChannel + alpha) / (1 + alpha)), decodingGamma);
        }

        /// <summary>
        /// Calculates the luma value based on the <a href="http://www.w3.org/TR/2008/REC-WCAG20-20081211/#relativeluminancedef">W3 Standard</a>
        /// </summary>
        /// <value>
        /// The luma value for the current color
        /// </value>
        public double Luma
        {
            get
            {
                var linearR = R / 255; 
                var linearG = G / 255;
                var linearB = B / 255;

                var red = TransformLinearToSrbg(linearR);
                var green = TransformLinearToSrbg(linearG);
                var blue = TransformLinearToSrbg(linearB);
                
                return 0.2126 * red + 0.7152 * green + 0.0722 * blue;
            }
        }

        public override void AppendCSS(Env env)
        {
            if (_text != null)
            {
                env.Output.AppendFormat(CultureInfo.InvariantCulture, _text);
                return;
            }

            var rgb = RGB
                .Select(d => (int) Math.Round(d, MidpointRounding.AwayFromZero))
                .Select(i => i > 255 ? 255 : (i < 0 ? 0 : i))
                .ToArray();

            if (Alpha <= 0.0 && rgb.All(c => c == 0))
            {
                env.Output.AppendFormat(CultureInfo.InvariantCulture, "transparent");
                return;
            }

            if (Alpha < 1.0)
            {
                env.Output.AppendFormat(CultureInfo.InvariantCulture, "rgba({0}, {1}, {2}, {3})", rgb[0], rgb[1], rgb[2], Alpha);
                return;
            }

            var hexString = '#' + rgb
                             .Select(i => i.ToString("X2"))
                             .JoinStrings("")
                             .ToLowerInvariant();

            env.Output.Append(hexString);
        }

        public Node Operate(Operation op, Node other)
        {
            var otherColor = other as Color;

            if (otherColor == null)
            {
                var operable = other as IOperable;
                if(operable == null)
                    throw new ParsingException(string.Format("Unable to convert right hand side of {0} to a color", op.Operator), op.Location);

                otherColor = operable.ToColor();
            }

            var result = new double[3];
            for (var c = 0; c < 3; c++)
            {
                result[c] = Operation.Operate(op.Operator, RGB[c], otherColor.RGB[c]);
            }
            return new Color(result)
                .ReducedFrom<Node>(this, other);
        }

        public Color ToColor()
        {
            return this;
        }

        /// <summary>
        ///  Returns in the IE ARGB format e.g ##FF001122 = rgba(0x00, 0x11, 0x22, 1)
        /// </summary>
        /// <returns></returns>
        public string ToArgb()
        {
            var argb = 
                new double[] { Alpha * 255 }
                .Concat(RGB)
                .Select(d => (int)Math.Round(d, MidpointRounding.AwayFromZero))
                .Select(i => i > 255 ? 255 : (i < 0 ? 0 : i))
                .ToArray();

            return '#' + argb
                 .Select(i => i.ToString("X2"))
                 .JoinStrings("")
                 .ToLowerInvariant();
        }

        public int CompareTo(object obj)
        {
            var col = obj as Color;

            if (col == null)
            {
                return -1;
            }

            if (col.R == R && col.G == G && col.B == B && col.Alpha == Alpha)
            {
                return 0;
            }

            return (((256 * 3) - (col.R + col.G + col.B)) * col.Alpha) < (((256 * 3) - (R + G + B)) * Alpha) ? 1 : -1;
        }

        public static explicit operator System.Drawing.Color(Color color)
        {
            if (color == null)
                throw new ArgumentNullException("color");

            return System.Drawing.Color.FromArgb((int) Math.Round(color.Alpha * 255d), (int) color.R, (int) color.G, (int) color.B);
        }
    }
}