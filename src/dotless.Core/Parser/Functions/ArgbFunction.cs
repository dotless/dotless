namespace dotless.Core.Parser.Functions
{
    using System.Linq;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;
    using Utils;

    public class ArgbFunction : Function
    {
        protected override Node Evaluate(Env env)
        {
            Guard.ExpectNumArguments(1, Arguments.Count, this, Location);
            Guard.ExpectAllNodes<Color>(Arguments, this, Location);

            var color = Arguments[0] as Color;

            return new TextNode(color.ToArgb());
        }
    }
}