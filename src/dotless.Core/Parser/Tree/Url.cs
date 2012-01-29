namespace dotless.Core.Parser.Tree
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Utils;
    using Exceptions;
    using Plugins;
    using dotless.Core.Importers;

    public class Url : Node
    {
        public Node Value { get; set; }

        public Url(Node value, IImporter importer)
        {
            if (value is TextNode)
            {
                var textValue = value as TextNode;
                if (!Regex.IsMatch(textValue.Value, @"^(([a-zA-Z]+:)|(\/))") && importer.Paths.Any())
                {
                    textValue.Value = importer.Paths.Concat(new[] { textValue.Value }).AggregatePaths(importer.CurrentDirectory);
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
            if (Value is TextNode)
                return (Value as TextNode).Value;

            throw new ParserException("Imports do not allow expressions");
        }

        public override Node Evaluate(Env env)
        {
            return new Url(Value.Evaluate(env));
        }

        public override void AppendCSS(Env env)
        {
            env.Output
                .Append("url(")
                .Append(Value)
                .Append(")");
        }

        public override void Accept(IVisitor visitor)
        {
            Value = VisitAndReplace(Value, visitor);
        }
    }
}