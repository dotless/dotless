namespace dotless.Core.Parser.Functions
{
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;
    using Utils;
    using System;
    using Exceptions;

    public class ColorFunction : Function
    {
        protected override Node Evaluate(Env env)
        {
            Guard.ExpectNumArguments(1, Arguments.Count, this, Location);
            Guard.ExpectNode<TextNode>(Arguments[0], this, Location);

            var argument = ((TextNode) Arguments[0]);

            string rgb;
            if (!argument.Value.StartsWith("#")) {
                var color = Color.GetColorFromKeyword(argument.Value);
                if (color != null) {
                    return color;
                }

                rgb = argument.Value;
            } else {
                rgb = argument.Value.TrimStart('#');
            }

            try
            {
                return new Color(rgb);
            }
            catch (FormatException ex)
            {
                throw new ParsingException(string.Format("Invalid RGB color string '{0}'", rgb), ex, Location, null);
            }
        }
    }
}