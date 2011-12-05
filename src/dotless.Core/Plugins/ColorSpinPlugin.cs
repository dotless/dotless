namespace dotless.Core.Plugins
{
    using System;
    using Parser.Infrastructure.Nodes;
    using Parser.Tree;
    using Utils;

    public class ColorSpinPlugin : VisitorPlugin
    {
        public double Spin { get; set; }

        public ColorSpinPlugin(double spin)
        {
            Spin = spin;
        }

        public override PluginType AppliesTo
        {
            get { return PluginType.AfterEvaluation; }
        }

        public override bool Execute(Node node)
        {
            if(node is Color)
            {
                var color = node as Color;

                var hslColor = HslColor.FromRgbColor(color);
                hslColor.Hue += Spin/360.0d;
                var newColor = hslColor.ToRgbColor();

                color.R = newColor.R;
                color.G = newColor.G;
                color.B = newColor.B;
            }

            return true;
        }
    }
}