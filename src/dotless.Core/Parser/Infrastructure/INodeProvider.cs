namespace dotless.Core.Parser.Infrastructure
{
    using System.Collections.Generic;
    using Importers;
    using Nodes;
    using Tree;

    public interface INodeProvider
  {
    Element Element(Combinator combinator, string value);
    Combinator Combinator(string value);
    Selector Selector(NodeList<Element> elements);
    Rule Rule(string name, Node value);
    Ruleset Ruleset(NodeList<Selector> selectors, List<Node> rules);

    //entities
    Alpha Alpha(Node value);
    Call Call(string name, NodeList<Expression> arguments);
    Color Color(string rgb);
    Keyword Keyword(string value);
    Number Number(string value, string unit);
    Shorthand Shorthand(Node first, Node second);
    Variable Variable(string name);
    Url Url(Node value);

    //mixins
    Mixin.Call MixinCall(NodeList<Element> elements, NodeList<Expression> arguments);
    Mixin.Definition MixinDefinition(string name, NodeList<Rule> parameters, List<Node> rules);

    //directives
    Import Import(Url path, Importer importer);
    Import Import(Quoted path, Importer importer);
    Directive Directive(string name, List<Node> rules);
    Directive Directive(string name, Node value);

    //expressions
    Expression Expression(NodeList expression);
    Value Value(IEnumerable<Node> values, Node important);
    Operation Operation(string operation, Node left, Node right);

    //text
    Comment Comment(string value);
    TextNode TextNode(string contents);
    Quoted Quoted(string value, string contents);
  }
}