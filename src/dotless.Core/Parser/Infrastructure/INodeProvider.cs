namespace dotless.Core.Parser.Infrastructure
{
    using System.Collections.Generic;
    using Importers;
    using Nodes;
    using Tree;

    public interface INodeProvider
    {
        Element Element(Combinator combinator, Node Value, int index);
        Combinator Combinator(string value, int index);
        Selector Selector(NodeList<Element> elements, int index);
        Rule Rule(string name, Node value, int index);
        Rule Rule(string name, Node value, bool variadic, int index);
        Ruleset Ruleset(NodeList<Selector> selectors, NodeList rules, int index);

        //entities
        Alpha Alpha(Node value, int index);
        Call Call(string name, NodeList<Node> arguments, int index);
        Color Color(string rgb, int index);
        Keyword Keyword(string value, int index);
        Number Number(string value, string unit, int index);
        Shorthand Shorthand(Node first, Node second, int index);
        Variable Variable(string name, int index);
        Url Url(Node value, IImporter importer, int index);
        Script Script(string script, int index);
        Paren Paren(Node node, int index);

        //mixins
        MixinCall MixinCall(NodeList<Element> elements, List<NamedArgument> arguments, bool important, int index);
        MixinDefinition MixinDefinition(string name, NodeList<Rule> parameters, NodeList rules, Condition condition, bool variadic, int index);
        Condition Condition(Node left, string operation, Node right, bool negate, int index);

        //directives
        Import Import(Url path, IImporter importer, Value features, int index);
        Import Import(Quoted path, IImporter importer, Value features, int index);
        Directive Directive(string name, NodeList rules, Value features, int index);
        Directive Directive(string name, string identifier, NodeList rules, int index);
        Directive Directive(string name, Node value, int index);
        KeyFrame KeyFrame(string identifier, NodeList rules, int index);

        //expressions
        Expression Expression(NodeList expression, int index);
 #if CSS3EXPERIMENTAL
        RepeatEntity RepeatEntity(Node value, Node repeatCount, int index);
#endif
        Value Value(IEnumerable<Node> values, string important, int index);
        Operation Operation(string operation, Node left, Node right, int index);
        Assignment Assignment(string key, Node value);

        //text
        Comment Comment(string value, int index);
        Comment Comment(string value, bool silent, int index);
        TextNode TextNode(string contents, int index);
        Quoted Quoted(string value, string contents, bool escaped, int index);
    }
}