namespace dotless.Core.Parser.Tree
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Utils;
	using dotless.Core.Exceptions;

    public class Url : Node
    {
        public Node Value { get; set; }

        public Url(Node value, IEnumerable<string> paths)
        {
            TextNode textValue = value as TextNode;
            if (textValue != null)
            {
                if (!Regex.IsMatch(textValue.Value, @"^(http:\/)?\/") && paths.Any())
                {
                    textValue.Value = paths.Concat(new[] { textValue.Value }).AggregatePaths();
                }
            }

            Value = value;
        }

		public Url(Node value)
		{
			Value = value;
		}

        public string GetUrl()
        {
			TextNode textValue = Value as TextNode;
			if (textValue != null)
			{
				return textValue.Value;
			}
			throw new ParserException("Imports do not allow expressions");
        }

		public override Node Evaluate(Env env)
		{
			return new Url(Value.Evaluate(env));
		}

        public override string ToCSS(Env env)
        {
            return "url(" + Value.ToCSS(env) + ")";
        }
    }
}