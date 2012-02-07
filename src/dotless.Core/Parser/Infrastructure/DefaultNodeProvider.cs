namespace dotless.Core.Parser.Infrastructure
{
    using System.Collections.Generic;
    using Importers;
    using Nodes;
    using Tree;

    public class DefaultNodeProvider : INodeProvider
    {
        public Element Element(Combinator combinator, string value, int index)
        {
            return new Element(combinator, value) { Index = index };
        }

        public Combinator Combinator(string value, int index)
        {
            return new Combinator(value) { Index = index };
        }

        public Selector Selector(NodeList<Element> elements, int index)
        {
            return new Selector(elements) { Index = index };
        }

        public Rule Rule(string name, Node value, int index)
        {
            return new Rule(name, value) { Index = index };
        }

        public Ruleset Ruleset(NodeList<Selector> selectors, NodeList rules, int index)
        {
            return new Ruleset(selectors, rules) { Index = index };
        }

        public Alpha Alpha(Node value, int index)
        {
            return new Alpha(value) { Index = index };
        }

        public Call Call(string name, NodeList<Expression> arguments, int index)
        {
            return new Call(name, arguments) { Index = index };
        }

        public Color Color(string rgb, int index)
        {
            return new Color(rgb) { Index = index };
        }

        public Keyword Keyword(string value, int index)
        {
            return new Keyword(value) { Index = index };
        }

        public Number Number(string value, string unit, int index)
        {
            return new Number(value, unit) { Index = index };
        }

        public Shorthand Shorthand(Node first, Node second, int index)
        {
            return new Shorthand(first, second) { Index = index };
        }

        public Variable Variable(string name, int index)
        {
            return new Variable(name) { Index = index };
        }

        public Url Url(Node value, IImporter importer, int index)
        {
            return new Url(value, importer) { Index = index };
        }

        public Script Script(string script, int index)
        {
            return new Script(script) { Index = index };
        }

        public MixinCall MixinCall(NodeList<Element> elements, List<NamedArgument> arguments, int index)
        {
            return new MixinCall(elements, arguments) { Index = index };
        }

        public MixinDefinition MixinDefinition(string name, NodeList<Rule> parameters, NodeList rules, int index)
        {
            return new MixinDefinition(name, parameters, rules) { Index = index };
        }

        public Import Import(Url path, IImporter importer, int index)
        {
            return new Import(path, importer) { Index = index };
        }

        public Import Import(Quoted path, IImporter importer, int index)
        {
            return new Import(path, importer) { Index = index };
        }

        public Directive Directive(string name, string identifier, NodeList rules, int index)
        {
            return new Directive(name, identifier, rules) { Index = index };
        }

        public KeyFrame KeyFrame(string identifier, NodeList rules, int index)
        {
            return new KeyFrame(identifier, rules) { Index = index };
        }

        public Directive Directive(string name, Node value, int index)
        {
            return new Directive(name, value) { Index = index };
        }

        public Expression Expression(NodeList expression, int index)
        {
            return new Expression(expression) { Index = index };
        }

        public Value Value(IEnumerable<Node> values, string important, int index)
        {
            return new Value(values, important) { Index = index };
        }

        public Operation Operation(string operation, Node left, Node right, int index)
        {
            return new Operation(operation, left, right) { Index = index };
        }

        public Comment Comment(string value, int index)
        {
            return Comment(value, false, index);
        }

        public Comment Comment(string value, bool silent, int index)
        {
            return new Comment(value, silent) { Index = index };
        }

        public TextNode TextNode(string contents, int index)
        {
            return new TextNode(contents) { Index = index };
        }

        public Quoted Quoted(string value, string contents, bool escaped, int index)
        {
            return new Quoted(value, contents, escaped) { Index = index };
        }
    }
}