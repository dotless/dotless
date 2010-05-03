//
// Here in, the parsing rules/functions
//
// The basic structure of the syntax tree generated is as follows:
//
//   Ruleset ->  Rule -> Value -> Expression -> Entity
//
// Here's some LESS code:
//
//    .class {
//      color: #fff;
//      border: 1px solid #000;
//      width: @w + 4px;
//      > .child {...}
//    }
//
// And here's what the parse tree might look like:
//
//     Ruleset (Selector '.class', [
//         Rule ("color",  Value ([Expression [Color #fff]]))
//         Rule ("border", Value ([Expression [Number 1px][Keyword "solid"][Color #000]]))
//         Rule ("width",  Value ([Expression [Operation "+" [Variable "@w"][Number 4px]]]))
//         Ruleset (Selector [Element '>', '.child'], [...])
//     ])
//
//  In general, most rules will try to parse a token with the `$()` function, and if the return
//  value is truly, will return a new node, of the relevant type. Sometimes, we need to check
//  first, before parsing, that's when we use `peek()`.
//

#pragma warning disable 665
// ReSharper disable RedundantNameQualifier

using System.Collections.Generic;
using dotless.Exceptions;
using dotless.Infrastructure;
using dotless.Tree;

namespace dotless
{
  public class Parsers 
  {
    public INodeProvider NodeProvider { get; set; }

    public Parsers(INodeProvider nodeProvider)
    {
      NodeProvider = nodeProvider;
    }

    //
    // The `primary` rule is the *entry* and *exit* point of the parser.
    // The rules here can appear at any level of the parse tree.
    //
    // The recursive nature of the grammar is an interplay between the `block`
    // rule, which represents `{ ... }`, the `ruleset` rule, and this `primary` rule,
    // as represented by this simplified grammar:
    //
    //     primary  →  (ruleset | rule)+
    //     ruleset  →  selector+ block
    //     block    →  '{' primary '}'
    //
    // Only at one point is the primary rule not called from the
    // block rule: at the root level.
    //
    public List<Node> Primary(Parser parser)
    {
      Node node;
      var root = new List<Node>();

      while (node = MixinDefinition(parser) || Rule(parser) || Ruleset(parser) ||
                    MixinCall(parser) || Comment(parser) ||
                    parser.Tokenizer.Match(@"[\n\s]+") || Directive(parser))
      {
        root.Add(node);
      }
      return root;
    }

    // We create a Comment node for CSS comments `/* */`,
    // but keep the LeSS comments `//` silent, by just skipping
    // over them.
    public Node Comment(Parser parser) {
      if (parser.Tokenizer.CurrentChar != '/') 
        return null;

      var comment = parser.Tokenizer.Match(@"\/\*(?:[^*]|\*+[^\/*])*\*+\/\n?");
      if (comment)
        return NodeProvider.Comment(comment.Value);

      return parser.Tokenizer.Match(@"\/\/.*");
    }

    //
    // Entities are tokens which can be found inside an Expression
    //

    //
    // A string, which supports escaping " and '
    //
    //     "milky way" 'he\'s the one!'
    //
    public Quoted Quoted(Parser parser)
    {
      if (parser.Tokenizer.CurrentChar != '"' && parser.Tokenizer.CurrentChar != '\'')
        return null;

      var str = parser.Tokenizer.Match(@"""((?:[^""\\\r\n]|\\.)*)""|'((?:[^'\\\r\n]|\\.)*)'");
      if (str)
        return NodeProvider.Quoted(str[0], str[1] ?? str[2]);

      return null;
    }

    //
    // A catch-all word, such as:
    //
    //     black border-collapse
    //
    public Keyword Keyword(Parser parser)
    {
      var k = parser.Tokenizer.Match(@"[A-Za-z-]+");
      if (k)
        return NodeProvider.Keyword(k.Value);

      return null;
    }

    //
    // A function call
    //
    //     rgb(255, 0, 255)
    //
    // We also try to catch IE's `alpha()`, but let the `alpha` parser
    // deal with the details.
    //
    // The arguments are parsed with the `entities.arguments` parser.
    //
    public Call Call(Parser parser)
    {
      var name = parser.Tokenizer.Match(@"([a-zA-Z0-9_-]+)\(");

      if (!name)
        return null;

      if (name[1].ToLowerInvariant() == "alpha")
      {
        var alpha = Alpha(parser);
        if (alpha != null)
          return alpha;
      }

      var args = Arguments(parser);

      if (! parser.Tokenizer.Match(')')) 
        return null;

      return NodeProvider.Call(name[1], args);
    }

    public NodeList<Expression> Arguments(Parser parser)
    {
      var args = new NodeList<Expression>();
      Expression arg;

      while (arg = Expression(parser))
      {
        args.Add(arg);
        if (! parser.Tokenizer.Match(','))
          break;
      }
      return args;
    }

    public Node Literal(Parser parser)
    {
      return Dimension(parser) ||
             Color(parser) ||
             Quoted(parser);
    }

    //
    // Parse url() tokens
    //
    // We use a specific rule for urls, because they don't really behave like
    // standard function calls. The difference is that the argument doesn't have
    // to be enclosed within a string, so it can't be parsed as an Expression.
    //
    public Url Url(Parser parser) 
    {
      if (parser.Tokenizer.CurrentChar != 'u' || !parser.Tokenizer.Match(@"url\(")) 
        return null;
    
      var value = Quoted(parser) || parser.Tokenizer.Match(@"[-a-zA-Z0-9_%@$\/.&=:;#+?]+");
    
      if (! parser.Tokenizer.Match(')')) 
        throw new ParsingException("missing closing ) for url()");

      return NodeProvider.Url(value);
    }

    //
    // A Variable entity, such as `@fink`, in
    //
    //     width: @fink + 2px
    //
    // We use a different parser for variable definitions,
    // see `parsers.variable`.
    //
    public Variable Variable(Parser parser)
    {
      RegexMatchResult name;

      if (parser.Tokenizer.CurrentChar == '@' && (name = parser.Tokenizer.Match(@"@[a-zA-Z0-9_-]+")))
        return NodeProvider.Variable(name.Value);

      return null;
    }

    //
    // A Hexadecimal color
    //
    //     #4F3C2F
    //
    // `rgb` and `hsl` colors are parsed through the `entities.call` parser.
    //
    public Color Color(Parser parser)
    {
      RegexMatchResult rgb;

      if (parser.Tokenizer.CurrentChar == '#' && (rgb = parser.Tokenizer.Match(@"#([a-fA-F0-9]{6}|[a-fA-F0-9]{3})")))
        return NodeProvider.Color(rgb[1]);

      return null;
    }

    //
    // A Dimension, that is, a number and a unit
    //
    //     0.5em 95%
    //
    public Number Dimension(Parser parser)
    {
      var c = parser.Tokenizer.CurrentChar;
      if ((c > 57 || c < 45) || c == 47)
        return null;

      var value = parser.Tokenizer.Match(@"(-?[0-9]*\.?[0-9]+)(px|%|em|pc|ex|in|deg|s|ms|pt|cm|mm)?");
      if (value)
        return NodeProvider.Number(value[1], value[2]);

      return null;
    }
    

    //
    // The variable part of a variable definition. Used in the `rule` parser
    //
    //     @fink:
    //
    public string VariableName(Parser parser)
    {
      var variable = Variable(parser);

      if (variable != null)
        return variable.Name;

      return null;
    }

    //
    // A font size/line-height shorthand
    //
    //     small/12px
    //
    // We need to peek first, or we'll match on keywords and dimensions
    //
    public Shorthand Shorthand(Parser parser)
    {
      if (! parser.Tokenizer.Peek(@"[@\w.-]+\/[@\w.-]+"))
        return null;

      Node a = null;
      Node b = null;
      if ((a = Entity(parser)) && parser.Tokenizer.Match('/') && (b = Entity(parser)))
        return NodeProvider.Shorthand(a, b);

      return null;
    }

    //
    // Mixins
    //

    //
    // A Mixin call, with an optional argument list
    //
    //     #mixins > .square(#fff);
    //     .rounded(4px, black);
    //     .button;
    //
    // The `while` loop is there because mixins can be
    // namespaced, but we only support the child and descendant
    // selector for now.
    //
    public Tree.Mixin.Call MixinCall(Parser parser)
    {
      var elements = new NodeList<Element>();

      RegexMatchResult e;
      Combinator c = null;

      while (e = parser.Tokenizer.Match(@"[#.]?[a-zA-Z0-9_-]+"))
      {
        elements.Add(NodeProvider.Element(c, e.Value));
        var match = parser.Tokenizer.Match('>');

        c = match != null ? NodeProvider.Combinator(match.Value) : null;
      }

      NodeList<Expression> args = null;
      if(parser.Tokenizer.Match('(') && (args = Arguments(parser)) && parser.Tokenizer.Match(')'))
      {
        // arguments optional
      }

      if (elements.Count > 0 && (parser.Tokenizer.Match(';') || parser.Tokenizer.Peek('}')))
        return NodeProvider.MixinCall(elements, args);

      return null;
    }

    //
    // A Mixin definition, with a list of parameters
    //
    //     .rounded (@radius: 2px, @color) {
    //        ...
    //     }
    //
    // Until we have a finer grained state-machine, we have to
    // do a look-ahead, to make sure we don't have a mixin call.
    // See the `rule` function for more information.
    //
    // We start by matching `.rounded (`, and then proceed on to
    // the argument list, which has optional default values.
    // We store the parameters in `params`, with a `value` key,
    // if there is a value, such as in the case of `@radius`.
    //
    // Once we've got our params list, and a closing `)`, we parse
    // the `{...}` block.
    //
    public Tree.Mixin.Definition MixinDefinition(Parser parser)
    {
      if (parser.Tokenizer.CurrentChar != '.' || parser.Tokenizer.Peek(@"[^{]*(;|})"))
        return null;

      var match = parser.Tokenizer.Match(@"([#.][a-zA-Z0-9_-]+)\s*\(");
      if (!match)
        return null;
      
      var name = match[1];

      var parameters = new NodeList<Rule>();
      RegexMatchResult param;
      Node param2 = null;
      while ((param = parser.Tokenizer.Match(@"@[\w-]+")) ||
             (param2 = Literal(parser) ||
                     Keyword(parser)) )
      {
        if (param != null)
        {
          if (parser.Tokenizer.Match(':'))
          {
            var value = Expression(parser);
            if (value)
              parameters.Add(NodeProvider.Rule(param.Value, value));
            else
              throw new ParsingException("Expected value");
          }
          else
            parameters.Add(NodeProvider.Rule(param.Value, null));
        }
        else
        {
          parameters.Add(NodeProvider.Rule(null, param2));
        }

        if (!parser.Tokenizer.Match(','))
          break;
      }
      if (! parser.Tokenizer.Match(')'))
        throw new ParsingException("Expected ')'");

      var rules = Block(parser);

      if (rules != null)
        return NodeProvider.MixinDefinition(name, parameters, rules);

      return null;
    }

    //
    // Entities are the smallest recognized token,
    // and can be found inside a rule's value.
    //
    public Node Entity(Parser parser) {
      return Literal(parser) || Variable(parser) || Url(parser) ||
             Call(parser)    || Keyword(parser);
    }

    //
    // A Rule terminator. Note that we use `Peek()` to check for '}',
    // because the `block` rule will be expecting it, but we still need to make sure
    // it's there, if ';' was ommitted.
    //
    public bool End(Parser parser) {
      return parser.Tokenizer.Match(';') || parser.Tokenizer.Peek('}');
    }

    //
    // IE's alpha function
    //
    //     alpha(opacity=88)
    //
    public Alpha Alpha(Parser parser)
    {
      Node value;

      if (! parser.Tokenizer.Match(@"opacity=", true))
        return null;

      if (value = parser.Tokenizer.Match(@"[0-9]+") || Variable(parser))
      {
        if (! parser.Tokenizer.Match(')')) 
          throw new ParsingException("missing closing ) for alpha()");
      
        return NodeProvider.Alpha(value);
      }

      return null;
    }

    //
    // A Selector Element
    //
    //     div
    //     + h1
    //     #socks
    //     input[type="text"]
    //
    // Elements are the building blocks for Selectors,
    // they are made out of a `Combinator` (see combinator rule),
    // and an element name, such as a tag a class, or `*`.
    //
    public Element Element(Parser parser)
    {
      var c = Combinator(parser);
      var e = parser.Tokenizer.Match(@"[.#:]?[a-zA-Z0-9_-]+") || parser.Tokenizer.Match('*') || Attribute(parser) ||
              parser.Tokenizer.Match(@"\([^)@]+\)");

      if (e)
        return NodeProvider.Element(c, e.Value);

      return null;
    }

    //
    // Combinators combine elements together, in a Selector.
    //
    // Because our parser isn't white-space sensitive, special care
    // has to be taken, when parsing the descendant combinator, ` `,
    // as it's an empty space. We have to check the previous character
    // in the input, to see if it's a ` ` character. More info on how
    // we deal with this in *combinator.js*.
    //
    public Combinator Combinator(Parser parser)
    {
      Node match;
      if (match = parser.Tokenizer.Match(@"[+>~]") || parser.Tokenizer.Match('&') || parser.Tokenizer.Match(@"::"))
        return NodeProvider.Combinator(match.ToString());

      return NodeProvider.Combinator(parser.Tokenizer.PreviousChar == ' ' ? " " : null);
    }

    //
    // A CSS Selector
    //
    //     .class > div + h1
    //     li a:hover
    //
    // Selectors are made out of one or more Elements, see above.
    //
    public Selector Selector(Parser parser)
    {
      Element e;
      var elements = new NodeList<Element>();

      while (e = Element(parser))
        elements.Add(e);

      if (elements.Count > 0)
        return NodeProvider.Selector(elements);

      return null;
    }

    public Node Tag(Parser parser)
    {
      return parser.Tokenizer.Match(@"[a-zA-Z][a-zA-Z-]*[0-9]?") || parser.Tokenizer.Match('*');
    }

    public TextNode Attribute(Parser parser)
    {
      var attr = "";
      Node key;
      Node val = null;

      if (! parser.Tokenizer.Match('['))
        return null;

      if (key = parser.Tokenizer.Match(@"[a-z-]+") || Quoted(parser))
      {
        Node op;
        if ((op = parser.Tokenizer.Match(@"[|~*$^]?=")) &&
            (val = Quoted(parser) || parser.Tokenizer.Match(@"[\w-]+")))
          attr = string.Format("{0}{1}{2}", key, op, val.ToCSS(null));
        else
          attr = key.ToString();
      }

      if (! parser.Tokenizer.Match(']'))
        return null;

      if (!string.IsNullOrEmpty(attr))
        return NodeProvider.TextNode("[" + attr + "]");

      return null;
    }

    //
    // The `block` rule is used by `ruleset` and `mixin.definition`.
    // It's a wrapper around the `primary` rule, with added `{}`.
    //
    public List<Node> Block(Parser parser)
    {
      if (!parser.Tokenizer.Match('{'))
        return null;

      var content = Primary(parser);

      if (content != null && parser.Tokenizer.Match('}'))
        return content;

      if (content != null)
        throw new ParsingException("Expected '}'");

      return null;
    }

    //
    // div, .class, body > p {...}
    //
    public Ruleset Ruleset(Parser parser)
    {
      var selectors = new NodeList<Selector>();

      var memo = parser.Tokenizer.GetLocation();

      if (parser.Tokenizer.Peek(@"([a-z.#: _-]+)[\s\n]*\{"))
      {
        var match = parser.Tokenizer.Match(@"[a-z.#: _-]+");
        selectors = new NodeList<Selector>(NodeProvider.Selector(new NodeList<Element>(NodeProvider.Element(null, match.Value))));
      }
      else
      {
        Selector s;
        while (s = Selector(parser))
        {
          selectors.Add(s);
          if (!parser.Tokenizer.Match(','))
            break;
        }
        if (s) Comment(parser);
      }

      List<Node> rules;

      if (selectors.Count > 0 && (rules = Block(parser)) != null)
        return NodeProvider.Ruleset(selectors, rules);

      parser.Tokenizer.SetLocation(memo);

      return null;
    }

    public Rule Rule(Parser parser)
    {
      var memo = parser.Tokenizer.GetLocation();

      var name = Property(parser) ?? VariableName(parser);

      if (name != null && parser.Tokenizer.Match(':'))
      {
        Node value;

        if ((name[0] != '@') && (parser.Tokenizer.Peek(@"([^@+\/*(;{}-]*);")))
          value = parser.Tokenizer.Match(@"[^@+\/*(;{}-]*");
        else if (name == "font")
          value = Font(parser);
        else
          value = Value(parser);

        if (End(parser))
          return NodeProvider.Rule(name, value);
      }

      parser.Tokenizer.SetLocation(memo);

      return null;
    }

    //
    // An @import directive
    //
    //     @import "lib";
    //
    // Depending on our environemnt, importing is done differently:
    // In the browser, it's an XHR request, in Node, it would be a
    // file-system operation. The function used for importing is
    // stored in `import`, which we pass to the Import constructor.
    //
    public Import Import(Parser parser)
    {
      Node path = null;

      if (parser.Tokenizer.Match(@"@import\s+") && (path = Quoted(parser) || Url(parser)) && parser.Tokenizer.Match(';'))
      {
        if (path is Quoted)
          return NodeProvider.Import(path as Quoted, parser.Importer);

        if (path is Url)
          return NodeProvider.Import(path as Url, parser.Importer);
      }

      return null;
    }

    //
    // A CSS Directive
    //
    //     @charset "utf-8";
    //
    public Directive Directive(Parser parser) {

      if (parser.Tokenizer.CurrentChar != '@') 
        return null;

      var import = Import(parser);
      if (import)
        return import;

      List<Node> rules;
      var name = parser.Tokenizer.MatchString(@"@media|@page");
      if (!string.IsNullOrEmpty(name))
      {
        var types = parser.Tokenizer.MatchString(@"[a-z:, ]+").Trim();
        rules = Block(parser);
        if (rules != null)
          return NodeProvider.Directive(name + " " + types, rules);
      }
      else
      {
        name = parser.Tokenizer.MatchString(@"@[-a-z]+");
        if (name == "@font-face")
        {
          rules = Block(parser);
          if (rules != null)
            return NodeProvider.Directive(name, rules);
        }
        else
        {
          Node value;
          if ((value = Entity(parser)) && parser.Tokenizer.Match(';'))
            return NodeProvider.Directive(name, value);
        }
      }

      return null;
    }

    public Value Font(Parser parser)
    {
      var value = new NodeList();
      var expression = new NodeList();
      Node e;

      while (e = Shorthand(parser) || Entity(parser))
      {
        expression.Add(e);
      }
      value.Add(NodeProvider.Expression(expression));

      if (parser.Tokenizer.Match(','))
      {
        while (e = Expression(parser))
        {
          value.Add(e);
          if (! parser.Tokenizer.Match(','))
            break;
        }
      }
      return NodeProvider.Value(value, Important(parser));
    }

    //
    // A Value is a comma-delimited list of Expressions
    //
    //     font-family: Baskerville, Georgia, serif;
    //
    // In a Rule, a Value represents everything after the `:`,
    // and before the `;`.
    //
    public Value Value(Parser parser)
    {
      var expressions = new NodeList();

      Node e;
      while (e = Expression(parser))
      {
        expressions.Add(e);
        if (!parser.Tokenizer.Match(','))
          break;
      }
      var important = Important(parser);

      if (expressions.Count > 0)
        return NodeProvider.Value(expressions, important);

      return null;
    }

    public Node Important(Parser parser)
    {
      return parser.Tokenizer.Match(@"!\s*important");
    }

    public Expression Sub(Parser parser)
    {
      Expression e = null;

      if (parser.Tokenizer.Match('(') && (e = Expression(parser)) && parser.Tokenizer.Match(')'))
        return e;

      return null;
    }

    public Node Multiplication(Parser parser)
    {
      var m = Operand(parser);
      if (!m)
        return null;

      Operation operation = null;

      while (true)
      {
        var op = parser.Tokenizer.Match(@"[\/*]");

        Node a = null;
        if (op && (a = Operand(parser)))
          operation = NodeProvider.Operation(op.Value, operation ?? m, a);
        else
          break;
      }
      return operation ?? m;
    }

    public Node Addition(Parser parser)
    {
      var m = Multiplication(parser);
      if (!m)
        return null;

      Operation operation = null;
      while (true)
      {
        var op = parser.Tokenizer.Match(@"[-+]\s+");
        if (!op && parser.Tokenizer.PreviousChar != ' ')
          op = parser.Tokenizer.Match(@"[-+]");

        Node a = null;
        if (op && (a = Multiplication(parser)))
          operation = NodeProvider.Operation(op.Value, operation ?? m, a);
        else
          break;
      }
      return operation ?? m;
    }

    //
    // An operand is anything that can be part of an operation,
    // such as a Color, or a Variable
    //
    public Node Operand(Parser parser) {
      return Sub(parser) || 
             Dimension(parser) ||
             Color(parser) || 
             Variable(parser);
    }

    //
    // Expressions either represent mathematical operations,
    // or white-space delimited Entities.
    //
    //     1px solid black
    //     @var * 2
    //
    public Expression Expression(Parser parser)
    {
      Node e;
      var entities = new NodeList();

      while (e = Addition(parser) || Entity(parser))
      {
        entities.Add(e);
      }

      if (entities.Count > 0)
        return NodeProvider.Expression(entities);

      return null;
    }

    public string Property(Parser parser)
    {
      var name = parser.Tokenizer.Match(@"\*?-?[-a-z]+");

      if (name)
        return name.Value;

      return null;
    }
  }
}