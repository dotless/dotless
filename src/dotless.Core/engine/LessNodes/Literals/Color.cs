/* Copyright 2009 dotless project, http://www.dotlesscss.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *     
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License. */

using dotless.Core.utils;

namespace dotless.Core.engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Color : Literal
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

        public double R { get; set; }
        public double G { get; set; }
        public double B { get; set; }
        public double A { get; set; }

        public Color(double r, double g, double b)
            : this(r, g, b, 1)
        {
        }
        public Color(double r, double g, double b, double a)
        {
            R = NumberExtensions.Normalize(r, 0, 255);
            G = NumberExtensions.Normalize(g, 0, 255);
            B = NumberExtensions.Normalize(b, 0, 255);
            A = NumberExtensions.Normalize(a);
        }

        public Color(int i)
        {
            B = i & 0xff;
            i >>= 8;
            G = i & 0xff;
            i >>= 8;
            R = i & 0xff;
            A = 1;
        }

        #region operator overrides
        public static Color operator +(Color colour1,  Color colour2)
        {
            return colour1.Operate((i, j) => i + j, colour2);
        }
        public static Color operator -(Color colour1, Color colour2)
        {
            return colour1.Operate((i, j) => i - j, colour2);
        }
        public static Color operator *(Color colour1, Color colour2)
        {
            return colour1.Operate((i, j) => i * j, colour2);
        }
        public static Color operator /(Color colour1, Color colour2)
        {
            return colour1.Operate((i, j) => i / j, colour2);
        }
        public static Color operator -(double colour2, Color colour1)
        {
            return colour1.Operate((i, j) => i - j, colour2);
        }
        public static Color operator +(double colour2, Color colour1)
        {
            return colour1.Operate((i, j) => i + j, colour2);
        }
        public static Color operator *(double colour2, Color colour1)
        {
            return colour1.Operate((i, j) => i * j, colour2);
        }
        public static Color operator /(double colour2, Color colour1)
        {
            return colour1.Operate((i, j) => i / j, colour2);
        }
        public static Color operator +(Color colour1, double colour2)
        {
            return colour1.Operate((i, j) => i + j, colour2);
        }
        public static Color operator -(Color colour1, double colour2)
        {
            return colour1.Operate((i, j) => i - j, colour2);
        }
        public static Color operator *(Color colour1, double colour2)
        {
            return colour1.Operate((i, j) => i * j, colour2);
        }
        public static Color operator /(Color colour1, double colour2)
        {
            return colour1.Operate((i, j) => i / j, colour2);
        }
        #endregion

        public Color Operate(Func<double, double, double> action, double other)
        {
            var rgb = RGB.Select(x => action(x, other))
                .ToArray();

            return new Color(rgb[0], rgb[1], rgb[2], A);
        }
        public Color Operate(Func<double, double, double> action, Color other)
        {
            var rgb = RGB.Select((x, i) => action(x, other.RGB[i]))
                .ToArray();

            return new Color(rgb[0], rgb[1], rgb[2], A);
        }
        public List<double> RGB
        {
            get
            {
                return new [] { R,G,B}.ToList();
            }
        }
        public Color Alpha(double a)
        {
            return new Color(R, G, B, a);
        }
        public override string ToString()
        {
            var rgb = RGB.Select(x => (int) Math.Round(x, MidpointRounding.AwayFromZero)).ToArray();

            string result;
            if (A < 1)
            {
                var alpha = new Number(A); // force alpha to print like Number
                result = string.Format("rgba({0}, {1}, {2}, {3})", rgb[0], rgb[1], rgb[2], alpha.ToCss());
            }
            else
            {
                result = GetKeyword();
                if(string.IsNullOrEmpty(result))
                    result = string.Format("#{0:X2}{1:X2}{2:X2}", rgb[0], rgb[1], rgb[2]);
            }

            return result.ToLower();
        }

        public override string ToCss()
        {
            return ToString();
        }

        public string GetKeyword()
        {
            var rgb = RGB.Select(x => (int)Math.Round(x, MidpointRounding.AwayFromZero)).ToArray();

            int color = (rgb[0] << 16) + (rgb[1] << 8) + rgb[2];
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