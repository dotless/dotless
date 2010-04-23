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
  public static class Parsers {
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
    public static List<Node> Primary(Parser parser)
    {
      Node node;
      var root = new List<Node>();

      while (node = Parsers.Mixin.Definition(parser) || Parsers.Rule(parser) || Parsers.Ruleset(parser) ||
                    Parsers.Mixin.Call(parser) || Parsers.Comment(parser) ||
                    parser.Tokenizer.Match(@"[\n\s]+") || Parsers.Directive(parser))
      {
        root.Add(node);
      }
      return root;
    }

    // We create a Comment node for CSS comments `/* */`,
    // but keep the LeSS comments `//` silent, by just skipping
    // over them.
    public static Node Comment(Parser parser) {
      if (parser.Tokenizer.CurrentChar != '/') 
        return null;

      var comment = parser.Tokenizer.Match(@"\/\*(?:[^*]|\*+[^\/*])*\*+\/\n?");
      if (comment)
        return new Comment(comment.Value);

      return parser.Tokenizer.Match(@"\/\/.*");
    }

    //
    // Entities are tokens which can be found inside an Expression
    //
    public static class Entities {
      //
      // A string, which supports escaping " and '
      //
      //     "milky way" 'he\'s the one!'
      //
      public static Quoted Quoted(Parser parser)
      {
        if (parser.Tokenizer.CurrentChar != '"' && parser.Tokenizer.CurrentChar != '\'')
          return null;

        var str = parser.Tokenizer.Match(@"""((?:[^""\\\r\n]|\\.)*)""|'((?:[^'\\\r\n]|\\.)*)'");
        if (str)
          return new Quoted(str[0], str[1] ?? str[2]);

        return null;
      }

      //
      // A catch-all word, such as:
      //
      //     black border-collapse
      //
      public static Keyword Keyword(Parser parser)
      {
        var k = parser.Tokenizer.Match(@"[A-Za-z-]+");
        if (k)
          return new Keyword(k.Value);

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
      public static Call Call(Parser parser)
      {
        var name = parser.Tokenizer.Match(@"([a-zA-Z0-9_-]+)\(");

        if (!name)
          return null;

        if (name[1].ToLowerInvariant() == "alpha")
        {
          var alpha = Parsers.Alpha(parser);
          if (alpha != null)
            return alpha;
        }

        var args = Parsers.Entities.Arguments(parser);

        if (! parser.Tokenizer.Match(')')) 
          return null;

        return new Call(name[1], args);
      }

      public static NodeList<Expression> Arguments(Parser parser)
      {
        var args = new NodeList<Expression>();
        Expression arg;

        while (arg = Parsers.Expression(parser))
        {
          args.Add(arg);
          if (! parser.Tokenizer.Match(','))
            break;
        }
        return args;
      }

      public static Node Literal(Parser parser)
      {
        return Parsers.Entities.Dimension(parser) ||
               Parsers.Entities.Color(parser) ||
               Parsers.Entities.Quoted(parser);
      }

      //
      // Parse url() tokens
      //
      // We use a specific rule for urls, because they don't really behave like
      // standard function calls. The difference is that the argument doesn't have
      // to be enclosed within a string, so it can't be parsed as an Expression.
      //
      public static Url Url(Parser parser) 
      {
        if (parser.Tokenizer.CurrentChar != 'u' || !parser.Tokenizer.Match(@"url\(")) 
          return null;
      
        var value = Parsers.Entities.Quoted(parser) || parser.Tokenizer.Match(@"[-a-zA-Z0-9_%@$\/.&=:;#+?]+");
      
        if (! parser.Tokenizer.Match(')')) 
          throw new ParsingException("missing closing ) for url()");

        return new Url(value);
      }

      //
      // A Variable entity, such as `@fink`, in
      //
      //     width: @fink + 2px
      //
      // We use a different parser for variable definitions,
      // see `parsers.variable`.
      //
      public static Variable Variable(Parser parser)
      {
        RegexMatchResult name;

        if (parser.Tokenizer.CurrentChar == '@' && (name = parser.Tokenizer.Match(@"@[a-zA-Z0-9_-]+")))
          return new Variable(name.Value);

        return null;
      }

      //
      // A Hexadecimal color
      //
      //     #4F3C2F
      //
      // `rgb` and `hsl` colors are parsed through the `entities.call` parser.
      //
      public static Color Color(Parser parser)
      {
        RegexMatchResult rgb;

        if (parser.Tokenizer.CurrentChar == '#' && (rgb = parser.Tokenizer.Match(@"#([a-fA-F0-9]{6}|[a-fA-F0-9]{3})")))
          return new Color(rgb[1]);

        return null;
      }

      //
      // A Dimension, that is, a number and a unit
      //
      //     0.5em 95%
      //
      public static Number Dimension(Parser parser)
      {
        var c = parser.Tokenizer.CurrentChar;
        if ((c > 57 || c < 45) || c == 47)
          return null;

        var value = parser.Tokenizer.Match(@"(-?[0-9]*\.?[0-9]+)(px|%|em|pc|ex|in|deg|s|ms|pt|cm|mm)?");
        if (value)
          return new Number(value[1], value[2]);

        return null;
      }
    }

    //
    // The variable part of a variable definition. Used in the `rule` parser
    //
    //     @fink:
    //
    public static string Variable(Parser parser)
    {
      RegexMatchResult name;

      if (parser.Tokenizer.CurrentChar == '@' && (name = parser.Tokenizer.Match(@"(@[a-zA-Z0-9_-]+)\s*:")))
        return name[1];

      return null;
    }

    //
    // A font size/line-height shorthand
    //
    //     small/12px
    //
    // We need to peek first, or we'll match on keywords and dimensions
    //
    public static Shorthand Shorthand(Parser parser)
    {
      if (! parser.Tokenizer.Peek(@"[@\w.-]+\/[@\w.-]+"))
        return null;

      Node a = null;
      Node b = null;
      if ((a = Parsers.Entity(parser)) && parser.Tokenizer.Match('/') && (b = Parsers.Entity(parser)))
        return new Shorthand(a, b);

      return null;
    }

    //
    // Mixins
    //
    public static class Mixin {
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
      public static Tree.Mixin.Call Call(Parser parser)
      {
        var elements = new NodeList<Element>();

        RegexMatchResult e;
        Combinator c = null;

        while (e = parser.Tokenizer.Match(@"[#.]?[a-zA-Z0-9_-]+"))
        {
          elements.Add(new Element(c, e.Value));
          var match = parser.Tokenizer.Match('>');

          c = match != null ? new Combinator(match.Value) : null;
        }

        NodeList<Expression> args = null;
        if(parser.Tokenizer.Match('(') && (args = Parsers.Entities.Arguments(parser)) && parser.Tokenizer.Match(')'))
        {
          // arguments optional
        }

        if (elements.Count > 0 && (parser.Tokenizer.Match(';') || parser.Tokenizer.Peek('}')))
          return new Tree.Mixin.Call(elements, args);

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
      public static Tree.Mixin.Definition Definition(Parser parser)
      {
        if (parser.Tokenizer.CurrentChar != '.' || parser.Tokenizer.Peek(@"[^{]*(;|})"))
          return null;

        var match = parser.Tokenizer.Match(@"([#.][a-zA-Z0-9_-]+)\s*\(");
        if (!match)
          return null;
        
        var name = match[1];

        var parameters = new NodeList<Rule>();
        RegexMatchResult param;
        while (param = parser.Tokenizer.Match(@"@[\w-]+"))
        {
          if (parser.Tokenizer.Match(':'))
          {
            var value = Parsers.Expression(parser);
            if (value)
              parameters.Add(new Rule(param.Value, value));
            else
              throw new ParsingException("Expected value");
          }
          else
            parameters.Add(new Rule(param.Value, null));

          if (!parser.Tokenizer.Match(','))
            break;
        }
        if (! parser.Tokenizer.Match(')'))
          throw new ParsingException("Expected ')'");

        var rules = Parsers.Block(parser);

        if (rules != null)
          return new Tree.Mixin.Definition(name, parameters, rules);

        return null;
      }
    }

    //
    // Entities are the smallest recognized token,
    // and can be found inside a rule's value.
    //
    public static Node Entity(Parser parser) {
      return Parsers.Entities.Literal(parser) || Parsers.Entities.Variable(parser) || Parsers.Entities.Url(parser) ||
             Parsers.Entities.Call(parser)    || Parsers.Entities.Keyword(parser);
    }

    //
    // A Rule terminator. Note that we use `Peek()` to check for '}',
    // because the `block` rule will be expecting it, but we still need to make sure
    // it's there, if ';' was ommitted.
    //
    public static bool End(Parser parser) {
      return parser.Tokenizer.Match(';') || parser.Tokenizer.Peek('}');
    }

    //
    // IE's alpha function
    //
    //     alpha(opacity=88)
    //
    public static Alpha Alpha(Parser parser)
    {
      Node value;

      if (! parser.Tokenizer.Match(@"opacity=", true))
        return null;

      if (value = parser.Tokenizer.Match(@"[0-9]+") || Parsers.Entities.Variable(parser))
      {
        if (! parser.Tokenizer.Match(')')) 
          throw new ParsingException("missing closing ) for alpha()");
      
        return new Alpha(value);
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
    public static Element Element(Parser parser)
    {
      var c = Parsers.Combinator(parser);
      var e = parser.Tokenizer.Match(@"[.#:]?[a-zA-Z0-9_-]+") || parser.Tokenizer.Match('*') || Parsers.Attribute(parser) ||
              parser.Tokenizer.Match(@"\([^)@]+\)");

      if (e)
        return new Element(c, e.Value);

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
    public static Combinator Combinator(Parser parser)
    {
      Node match;
      if (match = parser.Tokenizer.Match(@"[+>~]") || parser.Tokenizer.Match('&') || parser.Tokenizer.Match(@"::"))
        return new Combinator(match.ToString());

      return new Combinator(parser.Tokenizer.PreviousChar == ' ' ? " " : null);
    }

    //
    // A CSS Selector
    //
    //     .class > div + h1
    //     li a:hover
    //
    // Selectors are made out of one or more Elements, see above.
    //
    public static Selector Selector(Parser parser)
    {
      Element e;
      var elements = new NodeList<Element>();

      while (e = Parsers.Element(parser))
        elements.Add(e);

      if (elements.Count > 0)
        return new Selector(elements);

      return null;
    }

    public static Node Tag(Parser parser)
    {
      return parser.Tokenizer.Match(@"[a-zA-Z][a-zA-Z-]*[0-9]?") || parser.Tokenizer.Match('*');
    }

    public static TextNode Attribute(Parser parser)
    {
      var attr = "";
      Node key;
      Node val = null;

      if (! parser.Tokenizer.Match('['))
        return null;

      if (key = parser.Tokenizer.Match(@"[a-z]+") || Parsers.Entities.Quoted(parser))
      {
        Node op;
        if ((op = parser.Tokenizer.Match(@"[|~*$^]?=")) &&
            (val = Parsers.Entities.Quoted(parser) || parser.Tokenizer.Match(@"[\w-]+")))
          attr = string.Format("{0}{1}{2}", key, op, val.ToCSS(null));
        else
          attr = key.ToString();
      }

      if (! parser.Tokenizer.Match(']'))
        return null;

      if (!string.IsNullOrEmpty(attr))
        return new TextNode("[" + attr + "]");

      return null;
    }

    //
    // The `block` rule is used by `ruleset` and `mixin.definition`.
    // It's a wrapper around the `primary` rule, with added `{}`.
    //
    public static List<Node> Block(Parser parser)
    {
      if (!parser.Tokenizer.Match('{'))
        return null;

      var content = Parsers.Primary(parser);

      if (content != null && parser.Tokenizer.Match('}'))
        return content;

      if (content != null)
        throw new ParsingException("Expected '}'");

      return null;
    }

    //
    // div, .class, body > p {...}
    //
    public static Ruleset Ruleset(Parser parser)
    {
      var selectors = new NodeList<Selector>();

      var memo = parser.Tokenizer.GetLocation();

      if (parser.Tokenizer.Peek(@"([a-z.#: _-]+)[\s\n]*\{"))
      {
        var match = parser.Tokenizer.Match(@"[a-z.#: _-]+");
        selectors = new NodeList<Selector>(new Selector(new NodeList<Element>(new Element(null, match.Value))));
      }
      else
      {
        Selector s;
        while (s = Parsers.Selector(parser))
        {
          selectors.Add(s);
          if (!parser.Tokenizer.Match(','))
            break;
        }
        if (s) Parsers.Comment(parser);
      }

      List<Node> rules;

      if (selectors.Count > 0 && (rules = Parsers.Block(parser)) != null)
        return new Ruleset(selectors, rules);

      parser.Tokenizer.SetLocation(memo);

      return null;
    }

    public static Rule Rule(Parser parser)
    {
      var memo = parser.Tokenizer.GetLocation();

      var name = Parsers.Property(parser) ?? Parsers.Variable(parser);

      if (!string.IsNullOrEmpty(name))
      {
        Node value;

        if ((name[0] != '@') && (parser.Tokenizer.Peek(@"([^@+\/*(;{}-]*);")))
          value = parser.Tokenizer.Match(@"[^@+\/*(;{}-]*");
        else if (name == "font")
          value = Parsers.Font(parser);
        else
          value = Parsers.Value(parser);

        if (Parsers.End(parser))
          return new Rule(name, value);
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
    public static Import Import(Parser parser)
    {
      Node path = null;

      if (parser.Tokenizer.Match(@"@import\s+") && (path = Parsers.Entities.Quoted(parser) || Parsers.Entities.Url(parser)) && parser.Tokenizer.Match(';'))
      {
        if (path is Quoted)
          return new Import(path as Quoted, parser.Importer);

        if (path is Url)
          return new Import(path as Url, parser.Importer);
      }

      return null;
    }

    //
    // A CSS Directive
    //
    //     @charset "utf-8";
    //
    public static Directive Directive(Parser parser) {

      if (parser.Tokenizer.CurrentChar != '@') 
        return null;

      var import = Parsers.Import(parser);
      if (import)
        return import;

      List<Node> rules;
      var name = parser.Tokenizer.MatchString(@"@media|@page");
      if (!string.IsNullOrEmpty(name))
      {
        var types = parser.Tokenizer.MatchString(@"[a-z:, ]+").Trim();
        rules = Parsers.Block(parser);
        if (rules != null)
          return new Directive(name + " " + types, rules);
      }
      else
      {
        name = parser.Tokenizer.MatchString(@"@[-a-z]+");
        if (name == "@font-face")
        {
          rules = Parsers.Block(parser);
          if (rules != null)
            return new Directive(name, rules);
        }
        else
        {
          Node value;
          if ((value = Parsers.Entity(parser)) && parser.Tokenizer.Match(';'))
            return new Directive(name, value);
        }
      }

      return null;
    }

    public static Value Font(Parser parser)
    {
      var value = new NodeList();
      var expression = new NodeList();
      Node e;

      while (e = Parsers.Shorthand(parser) || Parsers.Entity(parser))
      {
        expression.Add(e);
      }
      value.Add(new Expression(expression));

      if (parser.Tokenizer.Match(','))
      {
        while (e = Parsers.Expression(parser))
        {
          value.Add(e);
          if (! parser.Tokenizer.Match(','))
            break;
        }
      }
      return new Value(value, Parsers.Important(parser));
    }

    //
    // A Value is a comma-delimited list of Expressions
    //
    //     font-family: Baskerville, Georgia, serif;
    //
    // In a Rule, a Value represents everything after the `:`,
    // and before the `;`.
    //
    public static Value Value(Parser parser)
    {
      var expressions = new NodeList();

      Node e;
      while (e = Parsers.Expression(parser))
      {
        expressions.Add(e);
        if (!parser.Tokenizer.Match(','))
          break;
      }
      var important = Parsers.Important(parser);

      if (expressions.Count > 0)
        return new Value(expressions, important);

      return null;
    }

    public static Node Important(Parser parser)
    {
      return parser.Tokenizer.Match(@"!\s*important");
    }

    public static Expression Sub(Parser parser)
    {
      Expression e = null;

      if (parser.Tokenizer.Match('(') && (e = Parsers.Expression(parser)) && parser.Tokenizer.Match(')'))
        return e;

      return null;
    }

    public static Node Multiplication(Parser parser)
    {
      var m = Parsers.Operand(parser);
      if (!m)
        return null;
    
      var op = parser.Tokenizer.Match(@"[\/*]");

      Node a = null;
      if (op && (a = Parsers.Multiplication(parser)))
        return new Operation(op.Value, m, a);

      return m;
    }

    public static Node Addition(Parser parser)
    {
      var m = Parsers.Multiplication(parser);
      if (!m)
        return null;

      var op = parser.Tokenizer.Match(@"[-+]\s+");
      if (!op && parser.Tokenizer.PreviousChar != ' ')
        op = parser.Tokenizer.Match(@"[-+]");

      Node a = null;
      if (op && (a = Parsers.Addition(parser)))
        return new Operation(op.Value, m, a);

      return m;
    }

    //
    // An operand is anything that can be part of an operation,
    // such as a Color, or a Variable
    //
    public static Node Operand(Parser parser) {
      return Parsers.Sub(parser) || 
             Parsers.Entities.Dimension(parser) ||
             Parsers.Entities.Color(parser) || 
             Parsers.Entities.Variable(parser);
    }

    //
    // Expressions either represent mathematical operations,
    // or white-space delimited Entities.
    //
    //     1px solid black
    //     @var * 2
    //
    public static Expression Expression(Parser parser)
    {
      Node e;
      var entities = new NodeList();

      while (e = Parsers.Addition(parser) || Parsers.Entity(parser))
      {
        entities.Add(e);
      }

      if (entities.Count > 0)
        return new Expression(entities);

      return null;
    }

    public static string Property(Parser parser)
    {
      var name = parser.Tokenizer.Match(@"(\*?-?[-a-z]+)\s*:");

      if (name)
        return name[1];

      return null;
    }
  }
}