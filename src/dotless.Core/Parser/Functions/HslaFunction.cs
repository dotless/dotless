namespace dotless.Core.Parser.Functions
{
    using System.Linq;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;
    using Utils;

    public class HslaFunction : Function
    {
        protected override Node Evaluate(Env env)
        {
            Guard.ExpectNumArguments(4, Arguments.Count, this, Index);
            Guard.ExpectAllNodes<Number>(Arguments, this, Index);

            var args = Arguments.Cast<Number>().ToArray();

            return new HslColor(args[0], args[1], args[2], args[3]).ToRgbColor();
        }
    }
}