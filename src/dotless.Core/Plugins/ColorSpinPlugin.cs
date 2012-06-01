namespace dotless.Core.Plugins
{
    using System;
    using Parser.Infrastructure.Nodes;
    using Parser.Tree;
    using Utils;
    using System.ComponentModel;

    [Description("Automatically spins all colors in a less file"), DisplayName("ColorSpin")]
    public class ColorSpinPlugin : VisitorPlugin
    {
        public double Spin { get; set; }

        public ColorSpinPlugin(double spin)
        {
            Spin = spin;
        }

        public override VisitorPluginType AppliesTo
        {
            get { return VisitorPluginType.AfterEvaluation; }
        }

        public override Node Execute(Node node, out bool visitDeeper)
        {
            visitDeeper = true;

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

            return node;
        }
    }
}