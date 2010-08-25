namespace dotless.Core.Parser.Tree
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Utils;

    public class Url : Node
    {
        public TextNode Value { get; set; }

        public Url(TextNode value, IEnumerable<string> paths)
        {
            if (!Regex.IsMatch(value.Value, @"^(http:\/)?\/") && paths.Any())
            {
                value.Value = paths.Concat(new[] {value.Value}).AggregatePaths();
            }

            Value = value;
        }

        public string GetUrl()
        {
            return Value.Value;
        }

        public override string ToCSS(Env env)
        {
            return "url(" + Value.ToCSS(env) + ")";
        }
    }
}