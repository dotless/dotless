namespace dotless.Core.Parser.Infrastructure
{
    using System.Collections.Generic;
    using Importers;
    using Nodes;
    using Tree;

    public interface INodeProvider
    {
        Element Element(Combinator combinator, Node Value, NodeLocation location);
        Combinator Combinator(string value, NodeLocation location);
        Selector Selector(NodeList<Element> elements, NodeLocation location);
        Rule Rule(string name, Node value, NodeLocation location);
        Rule Rule(string name, Node value, bool variadic, NodeLocation location);
        Ruleset Ruleset(NodeList<Selector> selectors, NodeList rules, NodeLocation location);

        //entities
        Alpha Alpha(Node value, NodeLocation location);
        Call Call(string name, NodeList<Node> arguments, NodeLocation location);
        Color Color(string rgb, NodeLocation location);
        Keyword Keyword(string value, NodeLocation location);
        Number Number(string value, string unit, NodeLocation location);
        Shorthand Shorthand(Node first, Node second, NodeLocation location);
        Variable Variable(string name, NodeLocation location);
        Url Url(Node value, IImporter importer, NodeLocation location);
        Script Script(string script, NodeLocation location);
        Paren Paren(Node node, NodeLocation location);

        //mixins
        MixinCall MixinCall(NodeList<Element> elements, List<NamedArgument> arguments, bool important, NodeLocation location);
        MixinDefinition MixinDefinition(string name, NodeList<Rule> parameters, NodeList rules, Condition condition, bool variadic, NodeLocation location);
        Condition Condition(Node left, string operation, Node right, bool negate, NodeLocation location);

        //directives
        Import Import(Url path, IImporter importer, Value features, bool isOnce, NodeLocation location);
        Import Import(Quoted path, IImporter importer, Value features, bool isOnce, NodeLocation location);
        Directive Directive(string name, string identifier, NodeList rules, NodeLocation location);
        Directive Directive(string name, Node value, NodeLocation location);
        Media Media(NodeList rules, Value features, NodeLocation location);
        KeyFrame KeyFrame(NodeList identifier, NodeList rules, NodeLocation location);

        //expressions
        Expression Expression(NodeList expression, NodeLocation location);
 #if CSS3EXPERIMENTAL
        RepeatEntity RepeatEntity(Node value, Node repeatCount, int index);
#endif
        Value Value(IEnumerable<Node> values, string important, NodeLocation location);
        Operation Operation(string operation, Node left, Node right, NodeLocation location);
        Assignment Assignment(string key, Node value, NodeLocation location);

        //text
        Comment Comment(string value, NodeLocation location);
        TextNode TextNode(string contents, NodeLocation location);
        Quoted Quoted(string value, string contents, bool escaped, NodeLocation location);
    }
}