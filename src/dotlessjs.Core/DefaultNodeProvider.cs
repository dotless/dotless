using System.Collections.Generic;
using dotless.Infrastructure;
using dotless.Tree;

namespace dotless
{
  public class DefaultNodeProvider : INodeProvider
  {
    public Element Element(Combinator combinator, string value)
    {
      return new Element(combinator, value);
    }

    public Combinator Combinator(string value)
    {
      return new Combinator(value);
    }

    public Selector Selector(NodeList<Element> elements)
    {
      return new Selector(elements);
    }

    public Rule Rule(string name, Node value)
    {
      return new Rule(name, value);
    }

    public Ruleset Ruleset(NodeList<Selector> selectors, List<Node> rules)
    {
      return new Ruleset(selectors, rules);
    }

    public Alpha Alpha(Node value)
    {
      return new Alpha(value);
    }

    public Call Call(string name, NodeList<Expression> arguments)
    {
      return new Call(name, arguments);
    }

    public Color Color(string rgb)
    {
      return new Color(rgb);
    }

    public Keyword Keyword(string value)
    {
      return new Keyword(value);
    }

    public Number Number(string value, string unit)
    {
      return new Number(value, unit);
    }

    public Shorthand Shorthand(Node first, Node second)
    {
      return new Shorthand(first, second);
    }

    public Variable Variable(string name)
    {
      return new Variable(name);
    }

    public Url Url(Node value)
    {
      return new Url(value);
    }

    public Mixin.Call MixinCall(NodeList<Element> elements, NodeList<Expression> arguments)
    {
      return new Mixin.Call(elements, arguments);
    }

    public Mixin.Definition MixinDefinition(string name, NodeList<Rule> parameters, List<Node> rules)
    {
      return new Mixin.Definition(name, parameters, rules);
    }

    public Import Import(Url path, Importer importer)
    {
      return new Import(path, importer);
    }

    public Import Import(Quoted path, Importer importer)
    {
      return new Import(path, importer);
    }

    public Directive Directive(string name, List<Node> rules)
    {
      return new Directive(name, rules);
    }

    public Directive Directive(string name, Node value)
    {
      return new Directive(name, value);
    }

    public Expression Expression(NodeList expression)
    {
      return new Expression(expression);
    }

    public Value Value(IEnumerable<Node> values, Node important)
    {
      return new Value(values, important);
    }

    public Operation Operation(string operation, Node left, Node right)
    {
      return new Operation(operation, left, right);
    }

    public Comment Comment(string value)
    {
      return new Comment(value);
    }

    public TextNode TextNode(string contents)
    {
      return new TextNode(contents);
    }

    public Quoted Quoted(string value, string contents)
    {
      return new Quoted(value, contents);
    }
  }
}