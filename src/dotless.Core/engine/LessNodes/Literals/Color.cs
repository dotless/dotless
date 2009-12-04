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

namespace dotless.Core.engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Color : Literal
    {
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
            R = Normailze(r);
            G = Normailze(g);
            B = Normailze(b);
            A = Normailze(a, 1);
        }


        protected double Normailze(double v) 
        {
            return Normailze(v, 255, 0);
        }

        protected double Normailze(double v, double max) 
        {
            return Normailze(v, max, 0);
        }

        protected double Normailze(double v, double max, double min)
        {
            return new[] { new[] { min, v }.Max(), max }.Min();
        }

        #region operator overrides
        public static Color operator +(Color colour1,  Color colour2)
        {
            return colour1.Operate((i, j) => i + j, colour2);
        }
        public static Color operator +(Color colour1, int colour2)
        {
            return colour1.Operate((i, j) => i + j, colour2);
        }
        public static Color operator +(Color colour1, double colour2)
        {
            return colour1.Operate((i, j) => i + j, (int)colour2);
        }
        public static Color operator -(Color colour1, Color colour2)
        {
            return colour1.Operate((i, j) => i - j, colour2);
        }
        public static Color operator -(Color colour1, int colour2)
        {
            return colour1.Operate((i, j) => i - j, colour2);
        }
        public static Color operator -(Color colour1, double colour2)
        {
            return colour1.Operate((i, j) => i - j, (int)colour2);
        }
        public static Color operator *(Color colour1, Color colour2)
        {
            return colour1.Operate((i, j) => i * j, colour2);
        }
        public static Color operator *(Color colour1, int colour2)
        {
            return colour1.Operate((i, j) => i * j, colour2);
        }
        public static Color operator *(Color colour1, double colour2)
        {
            return colour1.Operate((i, j) => i * j, (int)colour2);
        }
        public static Color operator /(Color colour1, Color colour2)
        {
            return colour1.Operate((i, j) => i / j, colour2);
        }
        public static Color operator /(Color colour1, int colour2)
        {
            return colour1.Operate((i, j) => i / j, colour2);
        }
        public static Color operator /(Color colour1, double colour2)
        {
            return colour1.Operate((i, j) => i / j, (int)colour2);
        }

        //Invert it so (2 * color) works as well as (color * 2)
        public static Color operator -(int colour2, Color colour1)
        {
            return colour1.Operate((i, j) => i - j, colour2);
        }
        public static Color operator +(int colour2, Color colour1)
        {
            return colour1.Operate((i, j) => i + j, colour2);
        }
        public static Color operator *(int colour2, Color colour1)
        {
            return colour1.Operate((i, j) => i * j, colour2);
        }
        public static Color operator /(int colour2, Color colour1)
        {
            return colour1.Operate((i, j) => i / j, colour2);
        }

        //Double too.. This is getting painful
        public static Color operator -(double colour2, Color colour1)
        {
            return colour1.Operate((i, j) => i - j, (int)colour2);
        }
        public static Color operator +(double colour2, Color colour1)
        {
            return colour1.Operate((i, j) => i + j, (int)colour2);
        }
        public static Color operator *(double colour2, Color colour1)
        {
            return colour1.Operate((i, j) => i * j, (int)colour2);
        }
        public static Color operator /(double colour2, Color colour1)
        {
            return colour1.Operate((i, j) => i / j, (int)colour2);
        }

        #endregion


        public Color Operate(Func<double, double, double> action, double other)
        {
            var rgb = RGB;
            //NOTE: Seems like there should be a nice way to do this using lambdas
            for (var i = 0; i < rgb.Count; i++)
                rgb[i] = action.Invoke(rgb[i], other);
            return new Color(rgb[0], rgb[1], rgb[2]);
        }

        public Color Operate(Func<int, int, int> action, int other)
        {
            var rgb = RGB;
            //NOTE: Seems like there should be a nice way to do this using lambdas
            for (var i = 0; i < rgb.Count; i++)
                rgb[i] = action.Invoke((int)rgb[i], other);
            return new Color(rgb[0], rgb[1], rgb[2]);
        }
        public Color Operate(Func<int, int, int> action, Color other)
        {
            var rgb = RGB;
            for (var i = 0; i < rgb.Count; i++)
                rgb[i] = action.Invoke((int)rgb[i], (int)other.RGB[i]);
            return new Color(rgb[0], rgb[1], rgb[2]);
        }

        public List<double> RGB
        {
            get
            {
                return new [] { R,G,B}.ToList();
            }
        }

        public Color Alpha(int a)
        {
            return new Color(R, G, B, a);
        }

        //NOTE: Dont understand this
        //def coerce other
        //  return self, other
        //end

        public override string ToString()
        {
            return (A < 1 ? string.Format("rgba({0},{1},{2}, {3})", (int)R, (int)G, (int)B, (int)A) : string.Format("#{0:X2}{1:X2}{2:X2}", (int)R, (int)G, (int)B)).ToLower();
        }
        public override string ToCss()
        {
            return ToString();
        }
        public override string ToCSharp()
        {
            return string.Format("new Color({0},{1},{2},{3})", R, G, B, A);
        }
        public override string Inspect()
        {
            return (A < 1 ? string.Format("rgba({0},{1},{2}, {3})", R, G, B, A) : string.Format("rgb({0},{1},{2})", (int)R, (int)G, (int)B)).ToLower();
        }
    }
}