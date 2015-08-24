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
        private static readonly Dictionary<string, Color> Html4Colors2 = new Dictionary<string, Color>
        {
            {"transparent", new Color(0x000000, 0.0, "transparent")}, // Note that transparent is not a color
            {"aliceblue", new Color(0xf0f8ff, 1.0, "aliceblue")},
            {"antiquewhite", new Color(0xfaebd7, 1.0, "antiquewhite")},
            {"aqua", new Color(0x00ffff, 1.0, "aqua")},
            {"aquamarine", new Color(0x7fffd4, 1.0, "aquamarine")},
            {"azure", new Color(0xf0ffff, 1.0, "azure")},
            {"beige", new Color(0xf5f5dc, 1.0, "beige")},
            {"bisque", new Color(0xffe4c4, 1.0, "bisque")},
            {"black", new Color(0x000000, 1.0, "black")},
            {"blanchedalmond", new Color(0xffebcd, 1.0, "blanchedalmond")},
            {"blue", new Color(0x0000ff, 1.0, "blue")},
            {"blueviolet", new Color(0x8a2be2, 1.0, "blueviolet")},
            {"brown", new Color(0xa52a2a, 1.0, "brown")},
            {"burlywood", new Color(0xdeb887, 1.0, "burlywood")},
            {"cadetblue", new Color(0x5f9ea0, 1.0, "cadetblue")},
            {"chartreuse", new Color(0x7fff00, 1.0, "chartreuse")},
            {"chocolate", new Color(0xd2691e, 1.0, "chocolate")},
            {"coral", new Color(0xff7f50, 1.0, "coral")},
            {"cornflowerblue", new Color(0x6495ed, 1.0, "cornflowerblue")},
            {"cornsilk", new Color(0xfff8dc, 1.0, "cornsilk")},
            {"crimson", new Color(0xdc143c, 1.0, "crimson")},
            {"cyan", new Color(0x00ffff, 1.0, "cyan")},
            {"darkblue", new Color(0x00008b, 1.0, "darkblue")},
            {"darkcyan", new Color(0x008b8b, 1.0, "darkcyan")},
            {"darkgoldenrod", new Color(0xb8860b, 1.0, "darkgoldenrod")},
            {"darkgray", new Color(0xa9a9a9, 1.0, "darkgray")},
            {"darkgrey", new Color(0xa9a9a9, 1.0, "darkgrey")},
            {"darkgreen", new Color(0x006400, 1.0, "darkgreen")},
            {"darkkhaki", new Color(0xbdb76b, 1.0, "darkkhaki")},
            {"darkmagenta", new Color(0x8b008b, 1.0, "darkmagenta")},
            {"darkolivegreen", new Color(0x556b2f, 1.0, "darkolivegreen")},
            {"darkorange", new Color(0xff8c00, 1.0, "darkorange")},
            {"darkorchid", new Color(0x9932cc, 1.0, "darkorchid")},
            {"darkred", new Color(0x8b0000, 1.0, "darkred")},
            {"darksalmon", new Color(0xe9967a, 1.0, "darksalmon")},
            {"darkseagreen", new Color(0x8fbc8f, 1.0, "darkseagreen")},
            {"darkslateblue", new Color(0x483d8b, 1.0, "darkslateblue")},
            {"darkslategray", new Color(0x2f4f4f, 1.0, "darkslategray")},
            {"darkslategrey", new Color(0x2f4f4f, 1.0, "darkslategrey")},
            {"darkturquoise", new Color(0x00ced1, 1.0, "darkturquoise")},
            {"darkviolet", new Color(0x9400d3, 1.0, "darkviolet")},
            {"deeppink", new Color(0xff1493, 1.0, "deeppink")},
            {"deepskyblue", new Color(0x00bfff, 1.0, "deepskyblue")},
            {"dimgray", new Color(0x696969, 1.0, "dimgray")},
            {"dimgrey", new Color(0x696969, 1.0, "dimgrey")},
            {"dodgerblue", new Color(0x1e90ff, 1.0, "dodgerblue")},
            {"firebrick", new Color(0xb22222, 1.0, "firebrick")},
            {"floralwhite", new Color(0xfffaf0, 1.0, "floralwhite")},
            {"forestgreen", new Color(0x228b22, 1.0, "forestgreen")},
            {"fuchsia", new Color(0xff00ff, 1.0, "fuchsia")},
            {"gainsboro", new Color(0xdcdcdc, 1.0, "gainsboro")},
            {"ghostwhite", new Color(0xf8f8ff, 1.0, "ghostwhite")},
            {"gold", new Color(0xffd700, 1.0, "gold")},
            {"goldenrod", new Color(0xdaa520, 1.0, "goldenrod")},
            {"gray", new Color(0x808080, 1.0, "gray")},
            {"grey", new Color(0x808080, 1.0, "grey")},
            {"green", new Color(0x008000, 1.0, "green")},
            {"greenyellow", new Color(0xadff2f, 1.0, "greenyellow")},
            {"honeydew", new Color(0xf0fff0, 1.0, "honeydew")},
            {"hotpink", new Color(0xff69b4, 1.0, "hotpink")},
            {"indianred", new Color(0xcd5c5c, 1.0, "indianred")},
            {"indigo", new Color(0x4b0082, 1.0, "indigo")},
            {"ivory", new Color(0xfffff0, 1.0, "ivory")},
            {"khaki", new Color(0xf0e68c, 1.0, "khaki")},
            {"lavender", new Color(0xe6e6fa, 1.0, "lavender")},
            {"lavenderblush", new Color(0xfff0f5, 1.0, "lavenderblush")},
            {"lawngreen", new Color(0x7cfc00, 1.0, "lawngreen")},
            {"lemonchiffon", new Color(0xfffacd, 1.0, "lemonchiffon")},
            {"lightblue", new Color(0xadd8e6, 1.0, "lightblue")},
            {"lightcoral", new Color(0xf08080, 1.0, "lightcoral")},
            {"lightcyan", new Color(0xe0ffff, 1.0, "lightcyan")},
            {"lightgoldenrodyellow", new Color(0xfafad2, 1.0, "lightgoldenrodyellow")},
            {"lightgray", new Color(0xd3d3d3, 1.0, "lightgray")},
            {"lightgrey", new Color(0xd3d3d3, 1.0, "lightgrey")},
            {"lightgreen", new Color(0x90ee90, 1.0, "lightgreen")},
            {"lightpink", new Color(0xffb6c1, 1.0, "lightpink")},
            {"lightsalmon", new Color(0xffa07a, 1.0, "lightsalmon")},
            {"lightseagreen", new Color(0x20b2aa, 1.0, "lightseagreen")},
            {"lightskyblue", new Color(0x87cefa, 1.0, "lightskyblue")},
            {"lightslategray", new Color(0x778899, 1.0, "lightslategray")},
            {"lightslategrey", new Color(0x778899, 1.0, "lightslategrey")},
            {"lightsteelblue", new Color(0xb0c4de, 1.0, "lightsteelblue")},
            {"lightyellow", new Color(0xffffe0, 1.0, "lightyellow")},
            {"lime", new Color(0x00ff00, 1.0, "lime")},
            {"limegreen", new Color(0x32cd32, 1.0, "limegreen")},
            {"linen", new Color(0xfaf0e6, 1.0, "linen")},
            {"magenta", new Color(0xff00ff, 1.0, "magenta")},
            {"maroon", new Color(0x800000, 1.0, "maroon")},
            {"mediumaquamarine", new Color(0x66cdaa, 1.0, "mediumaquamarine")},
            {"mediumblue", new Color(0x0000cd, 1.0, "mediumblue")},
            {"mediumorchid", new Color(0xba55d3, 1.0, "mediumorchid")},
            {"mediumpurple", new Color(0x9370d8, 1.0, "mediumpurple")},
            {"mediumseagreen", new Color(0x3cb371, 1.0, "mediumseagreen")},
            {"mediumslateblue", new Color(0x7b68ee, 1.0, "mediumslateblue")},
            {"mediumspringgreen", new Color(0x00fa9a, 1.0, "mediumspringgreen")},
            {"mediumturquoise", new Color(0x48d1cc, 1.0, "mediumturquoise")},
            {"mediumvioletred", new Color(0xc71585, 1.0, "mediumvioletred")},
            {"midnightblue", new Color(0x191970, 1.0, "midnightblue")},
            {"mintcream", new Color(0xf5fffa, 1.0, "mintcream")},
            {"mistyrose", new Color(0xffe4e1, 1.0, "mistyrose")},
            {"moccasin", new Color(0xffe4b5, 1.0, "moccasin")},
            {"navajowhite", new Color(0xffdead, 1.0, "navajowhite")},
            {"navy", new Color(0x000080, 1.0, "navy")},
            {"oldlace", new Color(0xfdf5e6, 1.0, "oldlace")},
            {"olive", new Color(0x808000, 1.0, "olive")},
            {"olivedrab", new Color(0x6b8e23, 1.0, "olivedrab")},
            {"orange", new Color(0xffa500, 1.0, "orange")},
            {"orangered", new Color(0xff4500, 1.0, "orangered")},
            {"orchid", new Color(0xda70d6, 1.0, "orchid")},
            {"palegoldenrod", new Color(0xeee8aa, 1.0, "palegoldenrod")},
            {"palegreen", new Color(0x98fb98, 1.0, "palegreen")},
            {"paleturquoise", new Color(0xafeeee, 1.0, "paleturquoise")},
            {"palevioletred", new Color(0xd87093, 1.0, "palevioletred")},
            {"papayawhip", new Color(0xffefd5, 1.0, "papayawhip")},
            {"peachpuff", new Color(0xffdab9, 1.0, "peachpuff")},
            {"peru", new Color(0xcd853f, 1.0, "peru")},
            {"pink", new Color(0xffc0cb, 1.0, "pink")},
            {"plum", new Color(0xdda0dd, 1.0, "plum")},
            {"powderblue", new Color(0xb0e0e6, 1.0, "powderblue")},
            {"purple", new Color(0x800080, 1.0, "purple")},
            {"red", new Color(0xff0000, 1.0, "red")},
            {"rosybrown", new Color(0xbc8f8f, 1.0, "rosybrown")},
            {"royalblue", new Color(0x4169e1, 1.0, "royalblue")},
            {"saddlebrown", new Color(0x8b4513, 1.0, "saddlebrown")},
            {"salmon", new Color(0xfa8072, 1.0, "salmon")},
            {"sandybrown", new Color(0xf4a460, 1.0, "sandybrown")},
            {"seagreen", new Color(0x2e8b57, 1.0, "seagreen")},
            {"seashell", new Color(0xfff5ee, 1.0, "seashell")},
            {"sienna", new Color(0xa0522d, 1.0, "sienna")},
            {"silver", new Color(0xc0c0c0, 1.0, "silver")},
            {"skyblue", new Color(0x87ceeb, 1.0, "skyblue")},
            {"slateblue", new Color(0x6a5acd, 1.0, "slateblue")},
            {"slategray", new Color(0x708090, 1.0, "slategray")},
            {"slategrey", new Color(0x708090, 1.0, "slategrey")},
            {"snow", new Color(0xfffafa, 1.0, "snow")},
            {"springgreen", new Color(0x00ff7f, 1.0, "springgreen")},
            {"steelblue", new Color(0x4682b4, 1.0, "steelblue")},
            {"tan", new Color(0xd2b48c, 1.0, "tan")},
            {"teal", new Color(0x008080, 1.0, "teal")},
            {"thistle", new Color(0xd8bfd8, 1.0, "thistle")},
            {"tomato", new Color(0xff6347, 1.0, "tomato")},
            {"turquoise", new Color(0x40e0d0, 1.0, "turquoise")},
            {"violet", new Color(0xee82ee, 1.0, "violet")},
            {"wheat", new Color(0xf5deb3, 1.0, "wheat")},
            {"white", new Color(0xffffff, 1.0, "white")},
            {"whitesmoke", new Color(0xf5f5f5, 1.0, "whitesmoke")},
            {"yellow", new Color(0xffff00, 1.0, "yellow")},
            {"yellowgreen", new Color(0x9acd32, 1.0, "yellowgreen")}
        };

        private static readonly Dictionary<int, string> Html4ColorsReverse;

        private static readonly Dictionary<string, int> Html4Colors =
            new Dictionary<string, int>
                {
                    { "aliceblue", 0xf0f8ff},
                    { "antiquewhite", 0xfaebd7},
                    { "aqua", 0x00ffff},
                    { "aquamarine", 0x7fffd4},
                    { "azure", 0xf0ffff},
                    { "beige", 0xf5f5dc},
                    { "bisque", 0xffe4c4},
                    { "black", 0x000000},
                    { "blanchedalmond", 0xffebcd},
                    { "blue", 0x0000ff},
                    { "blueviolet", 0x8a2be2},
                    { "brown", 0xa52a2a},
                    { "burlywood", 0xdeb887},
                    { "cadetblue", 0x5f9ea0},
                    { "chartreuse", 0x7fff00},
                    { "chocolate", 0xd2691e},
                    { "coral", 0xff7f50},
                    { "cornflowerblue", 0x6495ed},
                    { "cornsilk", 0xfff8dc},
                    { "crimson", 0xdc143c},
                    { "cyan", 0x00ffff},
                    { "darkblue", 0x00008b},
                    { "darkcyan", 0x008b8b},
                    { "darkgoldenrod", 0xb8860b},
                    { "darkgray", 0xa9a9a9},
                    { "darkgrey", 0xa9a9a9},
                    { "darkgreen", 0x006400},
                    { "darkkhaki", 0xbdb76b},
                    { "darkmagenta", 0x8b008b},
                    { "darkolivegreen", 0x556b2f},
                    { "darkorange", 0xff8c00},
                    { "darkorchid", 0x9932cc},
                    { "darkred", 0x8b0000},
                    { "darksalmon", 0xe9967a},
                    { "darkseagreen", 0x8fbc8f},
                    { "darkslateblue", 0x483d8b},
                    { "darkslategray", 0x2f4f4f},
                    { "darkslategrey", 0x2f4f4f},
                    { "darkturquoise", 0x00ced1},
                    { "darkviolet", 0x9400d3},
                    { "deeppink", 0xff1493},
                    { "deepskyblue", 0x00bfff},
                    { "dimgray", 0x696969},
                    { "dimgrey", 0x696969},
                    { "dodgerblue", 0x1e90ff},
                    { "firebrick", 0xb22222},
                    { "floralwhite", 0xfffaf0},
                    { "forestgreen", 0x228b22},
                    { "fuchsia", 0xff00ff},
                    { "gainsboro", 0xdcdcdc},
                    { "ghostwhite", 0xf8f8ff},
                    { "gold", 0xffd700},
                    { "goldenrod", 0xdaa520},
                    { "gray", 0x808080},
                    { "grey", 0x808080},
                    { "green", 0x008000},
                    { "greenyellow", 0xadff2f},
                    { "honeydew", 0xf0fff0},
                    { "hotpink", 0xff69b4},
                    { "indianred", 0xcd5c5c},
                    { "indigo", 0x4b0082},
                    { "ivory", 0xfffff0},
                    { "khaki", 0xf0e68c},
                    { "lavender", 0xe6e6fa},
                    { "lavenderblush", 0xfff0f5},
                    { "lawngreen", 0x7cfc00},
                    { "lemonchiffon", 0xfffacd},
                    { "lightblue", 0xadd8e6},
                    { "lightcoral", 0xf08080},
                    { "lightcyan", 0xe0ffff},
                    { "lightgoldenrodyellow", 0xfafad2},
                    { "lightgray", 0xd3d3d3},
                    { "lightgrey", 0xd3d3d3},
                    { "lightgreen", 0x90ee90},
                    { "lightpink", 0xffb6c1},
                    { "lightsalmon", 0xffa07a},
                    { "lightseagreen", 0x20b2aa},
                    { "lightskyblue", 0x87cefa},
                    { "lightslategray", 0x778899},
                    { "lightslategrey", 0x778899},
                    { "lightsteelblue", 0xb0c4de},
                    { "lightyellow", 0xffffe0},
                    { "lime", 0x00ff00},
                    { "limegreen", 0x32cd32},
                    { "linen", 0xfaf0e6},
                    { "magenta", 0xff00ff},
                    { "maroon", 0x800000},
                    { "mediumaquamarine", 0x66cdaa},
                    { "mediumblue", 0x0000cd},
                    { "mediumorchid", 0xba55d3},
                    { "mediumpurple", 0x9370d8},
                    { "mediumseagreen", 0x3cb371},
                    { "mediumslateblue", 0x7b68ee},
                    { "mediumspringgreen", 0x00fa9a},
                    { "mediumturquoise", 0x48d1cc},
                    { "mediumvioletred", 0xc71585},
                    { "midnightblue", 0x191970},
                    { "mintcream", 0xf5fffa},
                    { "mistyrose", 0xffe4e1},
                    { "moccasin", 0xffe4b5},
                    { "navajowhite", 0xffdead},
                    { "navy", 0x000080},
                    { "oldlace", 0xfdf5e6},
                    { "olive", 0x808000},
                    { "olivedrab", 0x6b8e23},
                    { "orange", 0xffa500},
                    { "orangered", 0xff4500},
                    { "orchid", 0xda70d6},
                    { "palegoldenrod", 0xeee8aa},
                    { "palegreen", 0x98fb98},
                    { "paleturquoise", 0xafeeee},
                    { "palevioletred", 0xd87093},
                    { "papayawhip", 0xffefd5},
                    { "peachpuff", 0xffdab9},
                    { "peru", 0xcd853f},
                    { "pink", 0xffc0cb},
                    { "plum", 0xdda0dd},
                    { "powderblue", 0xb0e0e6},
                    { "purple", 0x800080},
                    { "red", 0xff0000},
                    { "rosybrown", 0xbc8f8f},
                    { "royalblue", 0x4169e1},
                    { "saddlebrown", 0x8b4513},
                    { "salmon", 0xfa8072},
                    { "sandybrown", 0xf4a460},
                    { "seagreen", 0x2e8b57},
                    { "seashell", 0xfff5ee},
                    { "sienna", 0xa0522d},
                    { "silver", 0xc0c0c0},
                    { "skyblue", 0x87ceeb},
                    { "slateblue", 0x6a5acd},
                    { "slategray", 0x708090},
                    { "slategrey", 0x708090},
                    { "snow", 0xfffafa},
                    { "springgreen", 0x00ff7f},
                    { "steelblue", 0x4682b4},
                    { "tan", 0xd2b48c},
                    { "teal", 0x008080},
                    { "thistle", 0xd8bfd8},
                    { "tomato", 0xff6347},
                    { "turquoise", 0x40e0d0},
                    { "violet", 0xee82ee},
                    { "wheat", 0xf5deb3},
                    { "white", 0xffffff},
                    { "whitesmoke", 0xf5f5f5},
                    { "yellow", 0xffff00},
                    { "yellowgreen", 0x9acd32 }
                };

        static Color()
        {
            Html4ColorsReverse = new Dictionary<int, string>();

            foreach (KeyValuePair<string, int> color in Html4Colors)
            {
                if (Html4ColorsReverse.ContainsKey(color.Value))
                {
                    if (Html4ColorsReverse[color.Value].Length <= color.Key.Length)
                    {
                        continue;
                    }
                }

                Html4ColorsReverse[color.Value] = color.Key;
            }
        }

        public static Color From(string keywordOrHex)
        {
            return FromKeyword(keywordOrHex) ?? FromHex(keywordOrHex);
        }

        // TODO(yln): Dictionary should be instance of Color, color should be immutable!
        public static Color FromKeyword(string keyword)
        {
            if (keyword == "transparent")
            {
                return new Color(0, 0, 0, 0);
            }

            int color;
            if (Html4Colors.TryGetValue(keyword, out color))
            {
                var b = color & 0xff;
                color >>= 8;
                var g = color & 0xff;
                color >>= 8;
                var r = color & 0xff;

                return new Color(r, g, b);
            }

            return null;
        }

        public static Color FromHex(string hex)
        {
            hex = hex.TrimStart('#');
            var isArgb = false;
            var alpha = 1.0;
            double[] rgb;
            
            if (hex.Length == 8)
            {
                isArgb = true;
                rgb = Enumerable.Range(1, 3)
                    .Select(i => hex.Substring(i * 2, 2))
                    .Select(s => (double)int.Parse(s, NumberStyles.HexNumber))
                    .ToArray();
                alpha = (double)int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber) / 255d;
            }
            else if (hex.Length == 6)
            {
                rgb = Enumerable.Range(0, 3)
                    .Select(i => hex.Substring(i * 2, 2))
                    .Select(s => (double)int.Parse(s, NumberStyles.HexNumber))
                    .ToArray();
            }
            else
            {
                rgb = hex.ToCharArray()
                    .Select(c => (double)int.Parse("" + c + c, NumberStyles.HexNumber))
                    .ToArray();
            }

            return new Color(rgb, alpha) {isArgb = isArgb};
        }

        private bool isArgb = false;

        public readonly double[] RGB;
        public readonly double Alpha;

        private readonly int _rgb;
        private readonly string _text;

        public Color(int rgb, double alpha = 1.0, string text = null)
        {
            _rgb = rgb;
            Alpha = alpha;
            _text = text;
        }

        public Color(double[] rgb, double alpha = 1.0, string text = null)
            : this (((int) rgb[0] << 16) & ((int) rgb[1] << 8) & ((int) rgb[2]), alpha)
        {
            RGB = rgb.Select(c => NumberExtensions.Normalize(c, 255.0)).ToArray();
            Alpha = NumberExtensions.Normalize(alpha, 1.0);
            _text = text;
        }

        public Color(double red, double green, double blue, double alpha = 1.0, string text = null)
            : this(new[] {red, green, blue}, alpha, text)
        {
        }

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
                env.Output.Append(_text);
                return;
            }

            var rgb = RGB
                .Select(d => (int) Math.Round(d, MidpointRounding.AwayFromZero))
                .Select(i => i > 255 ? 255 : (i < 0 ? 0 : i))
                .ToArray();

            if (Alpha == 0 && rgb[0] == 0 && rgb[1] == 0 && rgb[2] == 0)
            {
                env.Output.AppendFormat(CultureInfo.InvariantCulture, "transparent");
                return;
            }

            if (isArgb)
            {
                env.Output.Append(ToArgb());
                return;
            }

            if (Alpha < 1.0)
            {
                env.Output.AppendFormat(CultureInfo.InvariantCulture, "rgba({0}, {1}, {2}, {3})", rgb[0], rgb[1], rgb[2], Alpha);
                return;
            }

            var keyword = GetKeyword(rgb);

            var hexString = '#' + rgb
                             .Select(i => i.ToString("X2"))
                             .JoinStrings("")
                             .ToLowerInvariant();

            if (env.Compress && !env.DisableColorCompression)
            {
                hexString = Regex.Replace(hexString, @"#(.)\1(.)\2(.)\3", "#$1$2$3");
                env.Output.Append(string.IsNullOrEmpty(keyword) || hexString.Length < keyword.Length ? hexString : keyword);
                return;
            }

            env.Output.Append(!string.IsNullOrEmpty(keyword) ? keyword : hexString);
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

            if (keyword == "transparent")
            {
                return new Color(0, 0, 0, 0);
            }

            if (Html4Colors.TryGetValue(keyword, out color))
            {
                var b = color & 0xff;
                color >>= 8;
                var g = color & 0xff;
                color >>= 8;
                var r = color & 0xff;

                return new Color(r, g, b);
            }

            return null;
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