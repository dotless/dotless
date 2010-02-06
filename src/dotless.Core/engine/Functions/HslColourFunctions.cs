namespace dotless.Core.engine.Functions
{
    public class HueFunction : HslColorFunctionBase
    {
        protected override INode EvalHsl(HslColor color, INode[] args)
        {
            return color.GetHueInDegrees();
        }

        protected override INode EditHsl(HslColor color, Number number)
        {
            color.Hue += number.Value / 360d;
            return color.ToRgbColor();
        }

        protected override string Name
        {
            get { return "hue"; }
        }
    }

    public class SaturationFunction : HslColorFunctionBase
    {
        protected override INode EvalHsl(HslColor color, INode[] args)
        {
            return color.GetSaturation();
        }

        protected override INode EditHsl(HslColor color, Number number)
        {
            color.Saturation += number.Value / 100;
            return color.ToRgbColor();
        }


        protected override string Name
        {
            get { return "saturation"; }
        }
    }

    public class LightnessFunction : HslColorFunctionBase
    {
        protected override INode EvalHsl(HslColor color, INode[] args)
        {
            return color.GetLightness();
        }

        protected override INode EditHsl(HslColor color, Number number)
        {
            color.Lightness += number.Value / 100;
            return color.ToRgbColor();
        }

        protected override string Name
        {
            get { return "lightness"; }
        }
    }
}