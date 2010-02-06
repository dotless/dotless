using System;
using System.Linq;

namespace dotless.Core.engine.Functions
{
    public class RgbaFunction : FunctionBase
    {
        public override INode Evaluate()
        {
            if (Arguments.All(arg => arg is Number) && Arguments.Length == 4)
                return GetFromComponents();

            if (Arguments.Length == 2 && Arguments[0] is Color && Arguments[1] is Number)
                return AddAlphaToColor(Arguments[0] as Color, Arguments[1] as Number);

            throw new exceptions.ParsingException("Expected 4 numeric arguments for RGBA color.");
        }

        private INode AddAlphaToColor(Color color, Number number)
        {
            var alpha = GetAlphaValue(number);

            return color.Alpha(alpha);
        }

        private INode GetFromComponents()
        {
            var colorArgs = Arguments
              .Take(3)
              .Cast<Number>()
              .Select(arg => arg.Unit == "%" ? 255 * arg.Value / 100 : arg.Value)
              .ToArray();

            double alpha = GetAlphaValue(Arguments[3] as Number);

            return new Color(colorArgs[0], colorArgs[1], colorArgs[2], alpha);
        }

        private double GetAlphaValue(Number number)
        {
            return number.Unit == "%" ? number.Value / 100 : number.Value;
        }
    }

    public class RgbFunction : RgbaFunction
    {
        public override INode Evaluate()
        {
            if (!Arguments.All(arg => arg is Number) || !(Arguments.Length == 3))
            {
                throw new exceptions.ParsingException("Expected 3 numeric arguments for RGB color.");
            }

            Arguments = Arguments.Concat(new[] { new Number(1) }).ToArray();

            return base.Evaluate();
        }
    }
}