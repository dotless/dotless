namespace dotless.Core.Parser.Functions
{
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;
    using Utils;

    public class ColorFunction : Function
    {
        protected override Node Evaluate(Env env)
        {
            Guard.ExpectNumArguments(1, Arguments.Count, this, Location);
            Guard.ExpectNode<TextNode>(Arguments[0], this, Location);

            return new Color(((TextNode)Arguments[0]).Value.TrimStart('#'));
        }
    }
}