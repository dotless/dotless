using System;
using System.Collections.Generic;
using System.Linq;

namespace nless.Core.engine.nodes.Literals
{
    public class Color : Literal
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public int A { get; set; }

        public Color(object r, object g, object b)
            : this(r, g, b, 1)
        {
        }

        public Color(object r, object g, object b, int a)
        {
            R = Normailze(Convert.ToInt16(r));
            G = Normailze(Convert.ToInt16(g));
            B = Normailze(Convert.ToInt16(b));
            A = Normailze(a, 1);
        }
   

        protected int Normailze(int v) 
        {
            return Normailze(v, 255, 0);
        }

        protected int Normailze(int v, int max) 
        {
            return Normailze(v, max, 0);
        }

        protected int Normailze(int v, int max, int min)
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
            public static Color operator -(Color colour1, Color colour2)
            {
                return colour1.Operate((i, j) => i - j, colour2);
            }
            public static Color operator -(Color colour1, int colour2)
            {
                return colour1.Operate((i, j) => i - j, colour2);
            }
            public static Color operator *(Color colour1, Color colour2)
            {
                return colour1.Operate((i, j) => i * j, colour2);
            }
            public static Color operator *(Color colour1, int colour2)
            {
                return colour1.Operate((i, j) => i * j, colour2);
            }
            public static Color operator /(Color colour1, Color colour2)
            {
                return colour1.Operate((i, j) => i / j, colour2);
            }
            public static Color operator /(Color colour1, int colour2)
            {
                return colour1.Operate((i, j) => i / j, colour2);
            }
        #endregion
        public Color Operate(Func<int, int, int> action, int other)
        {
            var rgb = RGB;
            //NOTE: Seems like there should be a nice way to do this using lambdas
            for (var i = 0; i < rgb.Count; i++)
                rgb[i] = action.Invoke(rgb[i], other);
            return new Color(rgb[0], rgb[1], rgb[2]);
        }

        public Color Operate(Func<int, int, int> action, Color other)
        {
            var rgb = RGB;
            for (var i = 0; i < rgb.Count; i++)
                rgb[i] = action.Invoke(rgb[i], other.RGB[i]);
            return new Color(rgb[0], rgb[1], rgb[2]);
        }

        public List<int> RGB
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
            return A < 1 ? string.Format("rgba({0},{1},{2}, {3})", R, G, B, A) : string.Format("#{0:X2}{1:X2}{2:X2}", R, G, B);
        }
        public override string ToCss()
        {
            return ToString();
        }
        public override string Inspect()
        {
            return A < 1 ? string.Format("rgba({0},{1},{2}, {3})", R, G, B, A) : string.Format("rgb({0},{1},{2})", R, G, B);
        }
    }



}
