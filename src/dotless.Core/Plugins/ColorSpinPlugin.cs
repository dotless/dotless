namespace dotless.Core.Plugins
{
    using System;
    using Parser.Infrastructure.Nodes;
    using Parser.Tree;
    using Utils;

    public class ColorSpinPlugin : VisitorPlugin
    {
        public override string Name { get { return "Color Spin Plugin"; } }
        public double Spin { get; set; }

        public ColorSpinPlugin(double spin)
        {
            Spin = spin;
        }

        public override VisitorPluginType AppliesTo
        {
            get { return VisitorPluginType.AfterEvaluation; }
        }

        public override bool Execute(ref Node node)
        {
            if(node is Color)
            {
                var color = node as Color;

                var hslColor = HslColor.FromRgbColor(color);
                hslColor.Hue += Spin/360.0d;
                var newColor = hslColor.ToRgbColor();

                //node = new Color(newColor.R, newColor.G, newColor.B);
                color.R = newColor.R;
                color.G = newColor.G;
                color.B = newColor.B;
            }

            return true;
        }
    }
}