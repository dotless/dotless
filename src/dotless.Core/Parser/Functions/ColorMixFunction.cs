namespace dotless.Core.Parser.Functions
{
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;
    using Utils;

    public abstract class ColorMixFunction : Function
    {
        protected override Node Evaluate(Env env)
        {
            Guard.ExpectNumArguments(2, Arguments.Count, this, Location);
            Guard.ExpectAllNodes<Color>(Arguments, this, Location);

            var color1 = Arguments[0] as Color;
            var color2 = Arguments[1] as Color;

            return new Color(
                Operate(color1.R, color2.R),
                Operate(color1.G, color2.G), 
                Operate(color1.B, color2.B));
        }

        protected abstract double Operate(double a, double b);
    }
}