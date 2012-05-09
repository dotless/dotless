namespace dotless.Core.Parser.Functions
{
    using Infrastructure.Nodes;
    using Tree;

    public class FadeInFunction : ColorFunctionBase
    {
        protected override Node Eval(Color color)
        {
            return new Number(color.Alpha);
        }

        protected override Node EditColor(Color color, Number number)
        {
            var alpha = number.Value;

            if (number.Unit == "%")
                alpha = alpha/100d;

            return new Color(color.R, color.G, color.B, ProcessAlpha( color.Alpha, alpha));
        }

        protected virtual double ProcessAlpha(double originalAlpha, double newAlpha)
        {
            return originalAlpha + newAlpha;
        }
    }

    public class AlphaFunction : FadeInFunction 
    {
        protected override Node EditColor(Color color, Number number)
        {
            WarnNotSupportedByLessJS("alpha(color, number)", "fadein(color, number) or the opposite fadeout(color, number),");

            return base.EditColor(color, number);
        }
    }

    public class FadeOutFunction : AlphaFunction
    {
        protected override Node EditColor(Color color, Number number)
        {
            return base.EditColor(color, -number);
        }
    }

    public class FadeFunction : AlphaFunction
    {
        protected override double  ProcessAlpha(double originalAlpha, double newAlpha)
        {
 	        return newAlpha;
        }
    }
}