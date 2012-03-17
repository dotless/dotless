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
        public List<string> ImportPaths { get; set; }
        public IImporter Importer { get; set; }

        public Url(Node value, IImporter importer)
        {
            Importer = importer;
            ImportPaths = importer.GetCurrentPathsClone();

            Value = value;
        }

        public Url(Node value)
        {
            Value = value;
        }

        public string GetUrl()
        {
            var textValue = Value as TextNode;
            if (textValue != null)
            {
                return AdjustUrlPath(textValue).Value;
            }

            throw new ParserException("Imports do not allow expressions");
        }

        private Node AdjustUrlPath(Node value)
        {
            var textValue = value as TextNode;
            if (textValue != null)
                return AdjustUrlPath(textValue);
            return value;
        }

        private TextNode AdjustUrlPath(TextNode textValue)
        {
            if (Importer != null && !Regex.IsMatch(textValue.Value, @"^(([a-zA-Z]+:)|(\/))"))
            {
                textValue.Value = Importer.AlterUrl(textValue.Value, ImportPaths);
            }
            return textValue;
        }

        public override Node Evaluate(Env env)
        {
            return new Url(AdjustUrlPath(Value.Evaluate(env)));
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