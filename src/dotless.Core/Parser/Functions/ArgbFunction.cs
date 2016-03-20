namespace dotless.Core.Parser.Functions
{
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;
    using Utils;

    public class ArgbFunction : Function
    {
        protected override Node Evaluate(Env env)
        {
            Guard.ExpectNumArguments(1, Arguments.Count, this, Location);
            var color = Guard.ExpectNode<Color>(Arguments[0], this, Location);

            return new TextNode(color.ToArgb());
        }
    }
}