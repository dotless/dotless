using System.Linq;

namespace dotless.Core.engine.Functions
{
    public class RgbFunction : FunctionBase
    {
        public override INode Evaluate()
        {
            if (!Arguments.All(arg => arg is Number) || !(Arguments.Length == 3))
            {
                throw new exceptions.ParsingException("Expected 3 numeric arguments for RGB color.");
            }
            var colorArgs = Arguments.Cast<Number>().Select(arg => arg.Unit == "%" ? 255 * arg.Value / 100 : arg.Value).ToArray();
            return new Color(colorArgs[0], colorArgs[1], colorArgs[2]);
        }
    }
}
