namespace dotless.Core.Utils
{
    using System;
    using System.Linq;
    using Parser.Tree;

    public class HslColor
    {
        // Note: To avoid converting back and forth between HlsColor and Color, HlsColor should perhaps inherit from color and only cenvert when needed.

        private double _hue;

        public double Hue
        {
            get { return _hue; }
            set { _hue = value%1d; }
        }

        private double _saturation;

        public double Saturation
        {
            get { return _saturation; }
            set { _saturation = NumberExtensions.Normalize(value); }
        }

        private double _lightness;

        public double Lightness
        {
            get { return _lightness; }
            set { _lightness = NumberExtensions.Normalize(value); }
        }

        public double Alpha { get; set; }

        public HslColor(double hue, double saturation, double lightness)
            : this(hue, saturation, lightness, 1)
        {
        }

        public HslColor(double hue, double saturation, double lightness, double alpha)
        {
            Hue = hue;
            Saturation = saturation;
            Lightness = lightness;
            Alpha = alpha;
        }

        public HslColor(Number hue, Number saturation, Number lightness, Number alpha)
        {
            Hue = (hue.ToNumber()/360d)%1d;
            Saturation = saturation.Normalize(100d)/100d;
            Lightness = lightness.Normalize(100d)/100d;
            Alpha = alpha.Normalize();
        }

        public static HslColor FromRgbColor(Color color)
        {
            // Note: this algorithm from http://www.easyrgb.com/index.php?X=MATH&H=18#text18

            const int R = 0;
            const int G = 1;
            const int B = 2;

            var rgb = color.RGB.Select(x => x/255d).ToArray();

            var min = rgb.Min();
            var max = rgb.Max();
            var range = max - min;

            var lightness = (max + min)/2;

            double saturation = 0;
            double hue = 0;

            if (range != 0)
            {
                if (lightness < 0.5)
                    saturation = range/(max + min);
                else
                    saturation = range/(2 - max - min);

                var deltas = rgb.Select(x => (((max - x)/6) + (range/2))/range).ToArray();

                if (rgb[R] == max)
                    hue = deltas[B] - deltas[G];
                else if (rgb[G] == max)
                    hue = (1d/3) + deltas[R] - deltas[B];
                else if (rgb[B] == max)
                    hue = (2d/3) + deltas[G] - deltas[R];

                if (hue < 0) hue += 1;
                if (hue > 1) hue -= 1;
            }

            return new HslColor(hue, saturation, lightness, color.Alpha);
        }


        public Color ToRgbColor()
        {
            // Note: this algorithm from http://www.easyrgb.com/index.php?X=MATH&H=19#text19

            if (Saturation == 0)
            {
                var grey = Math.Round(Lightness*255);
                return new Color(grey, grey, grey, Alpha);
            }

            double q;
            if (Lightness < 0.5)
                q = Lightness*(1 + Saturation);
            else
                q = (Lightness + Saturation) - (Saturation*Lightness);

            var p = 2*Lightness - q;

            var red = 255*Hue_2_RGB(p, q, Hue + (1d/3));
            var green = 255*Hue_2_RGB(p, q, Hue);
            var blue = 255*Hue_2_RGB(p, q, Hue - (1d/3));

            return new Color(red, green, blue, Alpha);
        }

        private static double Hue_2_RGB(double v1, double v2, double vH)
        {
            if (vH < 0) vH += 1;
            if (vH > 1) vH -= 1;
            if ((6*vH) < 1) return (v1 + (v2 - v1)*6*vH);
            if ((2*vH) < 1) return (v2);
            if ((3*vH) < 2) return (v1 + (v2 - v1)*((2d/3) - vH)*6);
            return (v1);
        }

        public Number GetHueInDegrees()
        {
            return new Number(Hue*360, "deg");
        }

        public Number GetSaturation()
        {
            return new Number(Saturation*100, "%");
        }

        public Number GetLightness()
        {
            return new Number(Lightness*100, "%");
        }
    }
}