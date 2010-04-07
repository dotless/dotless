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
//         Rule ("border", Value ([Expression [Dimension 1px][Keyword "solid"][Color #000]]))
//         Rule ("width",  Value ([Expression [Operation "+" [Variable "@w"][Dimension 4px]]]))
//         Ruleset (Selector [Element '>', '.child'], [...])
//     ])
//
//  In general, most rules will try to parse a token with the `$()` function, and if the return
//  value is truly, will return a new node, of the relevant type. Sometimes, we need to check
//  first, before parsing, that's when we use `peek()`.
//

#pragma warning disable 665
// ReSharper disable InconsistentNaming
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
    public static NodeList primary(Parser parser)
    {
      Node node;
      var root = new NodeList();

      while (node = Parsers.Mixin.definition(parser) || Parsers.ruleset(parser) || Parsers.rule(parser) ||
                    Parsers.Mixin.call(parser) || Parsers.comment(parser) ||
                    parser.Match(@"[\n\s]+") || Parsers.directive(parser))
      {
        root.Add(node);
      }
      return root;
    }

    // We create a Comment node for CSS comments `/* */`,
    // but keep the LeSS comments `//` silent, by just skipping
    // over them.
    public static Node comment(Parser parser) {
      if (parser.CurrentChar != '/') 
        return null;

      var comment = parser.Match(@"\/\*(?:[^*]|\*+[^\/*])*\*+\/\n?");
      if (comment)
        return new Comment(comment.Value);

      return parser.Match(@"\/\/.*");
    }

    //
    // Entities are tokens which can be found inside an Expression
    //
    public static class entities {
      //
      // A string, which supports escaping " and '
      //
      //     "milky way" 'he\'s the one!'
      //
      public static Quoted quoted(Parser parser)
      {
        if (parser.CurrentChar != '"' && parser.CurrentChar != '\'')
          return null;

        var str = parser.Match(@"""((?:[^""\\\r\n]|\\.)*)""|'((?:[^'\\\r\n]|\\.)*)'");
        if (str)
          return new Quoted(str[0], str[1] ?? str[2]);

        return null;
      }

      //
      // A catch-all word, such as:
      //
      //     black border-collapse
      //
      public static Node keyword(Parser parser)
      {
        var k = parser.Match(@"[A-Za-z-]+");
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
      public static Node call(Parser parser)
      {
        var name = parser.Match(@"([a-zA-Z0-9_-]+)\(");

        if (!name)
          return null;

        if (name[1].ToLowerInvariant() == "alpha")
          return Parsers.alpha(parser);

        var args = Parsers.entities.arguments(parser);

        if (! parser.Match(')')) 
          return null;

        return new Call(name[1], args);
      }

      public static NodeList arguments(Parser parser)
      {
        var args = new NodeList();
        Node arg;

        while (arg = Parsers.expression(parser))
        {
          args.Add(arg);
          if (! parser.Match(','))
            break;
        }
        return args;
      }

      public static Node literal(Parser parser)
      {
        return Parsers.entities.dimension(parser) ||
               Parsers.entities.color(parser) ||
               Parsers.entities.quoted(parser);
      }

      //
      // Parse url() tokens
      //
      // We use a specific rule for urls, because they don't really behave like
      // standard function calls. The difference is that the argument doesn't have
      // to be enclosed within a string, so it can't be parsed as an Expression.
      //
      public static Url url(Parser parser) 
      {
        if (parser.CurrentChar != 'u' || !parser.Match(@"url\(")) 
          return null;
      
        var value = Parsers.entities.quoted(parser) || parser.Match(@"[-a-zA-Z0-9_%@$\/.&=:;#+?]+");
      
        if (! parser.Match(')')) 
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
      public static Node variable(Parser parser)
      {
        RegexMatchResult name;

        if (parser.CurrentChar == '@' && (name = parser.Match(@"@[a-zA-Z0-9_-]+")))
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
      public static Node color(Parser parser)
      {
        RegexMatchResult rgb;

        if (parser.CurrentChar == '#' && (rgb = parser.Match(@"#([a-fA-F0-9]{6}|[a-fA-F0-9]{3})")))
          return new Color(rgb[1]);

        return null;
      }

      //
      // A Dimension, that is, a number and a unit
      //
      //     0.5em 95%
      //
      public static Node dimension(Parser parser)
      {
        var c = parser.CurrentChar;
        if ((c > 57 || c < 45) || c == 47)
          return null;

        var value = parser.Match(@"(-?[0-9]*\.?[0-9]+)(px|%|em|pc|ex|in|deg|s|ms|pt|cm|mm)?");
        if (value)
          return new Dimension(value[1], value[2]);

        return null;
      }
    }

    //
    // The variable part of a variable definition. Used in the `rule` parser
    //
    //     @fink:
    //
    public static string variable(Parser parser)
    {
      RegexMatchResult name;

      if (parser.CurrentChar == '@' && (name = parser.Match(@"(@[a-zA-Z0-9_-]+)\s*:")))
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
    public static Node shorthand(Parser parser)
    {

      if (! parser.Peek(@"[@\w.-]+\/[@\w.-]+"))
        return null;

      Node a = null;
      Node b = null;
      if ((a = Parsers.entity(parser)) && parser.Match('/') && (b = Parsers.entity(parser)))
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
      public static Node call(Parser parser)
      {
        var elements = new NodeList<Element>();

        RegexMatchResult e;
        Combinator c = null;

        while (e = parser.Match(@"[#.]?[a-zA-Z0-9_-]+"))
        {
          elements.Add(new Element(c, e.Value));
          var match = parser.Match('>');

          c = match != null ? new Combinator(match.Value) : null;
        }

        NodeList args = null;
        if(parser.Match('(') && (args = Parsers.entities.arguments(parser)) && parser.Match(')'))
        {
          // arguments optional
        }

        if (elements.Count > 0 && (parser.Match(';') || parser.Peek('}')))
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
      public static Node definition(Parser parser) {
        var @params = new Dictionary<string, Node>();

        if (parser.CurrentChar != '.' || parser.Peek(@"[^{]*(;|})"))
          return null;

        var match = parser.Match(@"([#.][a-zA-Z0-9_-]+)\s*\(");
        if (match) {
          var name = match[1];

          RegexMatchResult param;
          while (param = parser.Match(@"@[\w-]+"))
          {
            if (parser.Match(':'))
            {
              var value = Parsers.expression(parser);
              if (value)
                @params.Add(param.Value, value);
              else
                throw new ParsingException("Expected value");
            }
            else
              @params.Add(param.Value, null);

            if (! parser.Match(','))
              break;
          }
          if (! parser.Match(')')) 
            throw new ParsingException("Expected )");

          var rules = Parsers.block(parser);

          if (rules)
            return new Tree.Mixin.Definition(name, @params, rules);
        }

        return null;
      }
    }

    //
    // Entities are the smallest recognized token,
    // and can be found inside a rule's value.
    //
    public static Node entity(Parser parser) {
      return Parsers.entities.literal(parser) || Parsers.entities.variable(parser) || Parsers.entities.url(parser) ||
             Parsers.entities.call(parser)    || Parsers.entities.keyword(parser);
    }

    //
    // A Rule terminator. Note that we use `Peek()` to check for '}',
    // because the `block` rule will be expecting it, but we still need to make sure
    // it's there, if ';' was ommitted.
    //
    public static bool end(Parser parser) {
      return parser.Match(';') || parser.Peek('}');
    }

    //
    // IE's alpha function
    //
    //     alpha(opacity=88)
    //
    public static Node alpha(Parser parser)
    {
      Node value;

      if (! parser.Match(@"opacity=", true))
        return null;

      if (value = parser.Match(@"[0-9]+") || Parsers.entities.variable(parser))
      {
        if (! parser.Match(')')) 
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
    public static Element element(Parser parser)
    {
      var c = Parsers.combinator(parser);
      var e = parser.Match(@"[.#:]?[a-zA-Z0-9_-]+") || parser.Match('*') || Parsers.attribute(parser) ||
              parser.Match(@"\([^)@]+\)");

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
    public static Combinator combinator(Parser parser)
    {
      Node match;
      if (match = parser.Match(@"[+>~]") || parser.Match('&') || parser.Match(@"::"))
        return new Combinator(match.ToString());

      return new Combinator(parser.PreviousChar == ' ' ? " " : null);
    }

    //
    // A CSS Selector
    //
    //     .class > div + h1
    //     li a:hover
    //
    // Selectors are made out of one or more Elements, see above.
    //
    public static Selector selector(Parser parser)
    {
      Element e;
      var elements = new NodeList<Element>();

      while (e = Parsers.element(parser))
        elements.Add(e);

      if (elements.Count > 0)
        return new Selector(elements);

      return null;
    }

    public static Node tag(Parser parser)
    {
      return parser.Match(@"[a-zA-Z][a-zA-Z-]*[0-9]?") || parser.Match('*');
    }

    public static TextNode attribute(Parser parser)
    {
      var attr = "";
      Node key;
      Node val = null;

      if (! parser.Match('['))
        return null;

      if (key = parser.Match(@"[a-z]+") || Parsers.entities.quoted(parser))
      {
        Node op;
        if ((op = parser.Match(@"[|~*$^]?=")) &&
            (val = Parsers.entities.quoted(parser) || parser.Match(@"[\w-]+")))
          attr = string.Format("{0}{1}{2}", key, op, val.ToCSS(null));
        else
          attr = key.ToString();
      }

      if (! parser.Match(']'))
        return null;

      if (!string.IsNullOrEmpty(attr))
        return new TextNode("[" + attr + "]");

      return null;
    }

    //
    // The `block` rule is used by `ruleset` and `mixin.definition`.
    // It's a wrapper around the `primary` rule, with added `{}`.
    //
    public static NodeList block(Parser parser)
    {
      NodeList content = null;

      if (parser.Match('{') && (content = Parsers.primary(parser)) && parser.Match('}'))
        return content;

      return null;
    }

    //
    // div, .class, body > p {...}
    //
    public static Node ruleset(Parser parser)
    {
      var selectors = new NodeList<Selector>();

      if (parser.Peek(@"[^{]+[@;}]"))
        return null;

      if (parser.Peek(@"([a-z.#: _-]+)[\s\n]*\{"))
      {
        var match = parser.Match(@"[a-z.#: _-]+");
        selectors = new NodeList<Selector>(new Selector(new NodeList<Element>(new Element(null, match.Value))));
      }
      else
      {
        Selector s;
        while (s = Parsers.selector(parser))
        {
          selectors.Add(s);
          if (! parser.Match(','))
            break;
        }
        if (s) Parsers.comment(parser);
      }

      var rules = Parsers.block(parser);

      if (selectors.Count > 0 && rules)
        return new Ruleset(selectors, rules);

      return null;
    }

    public static Node rule(Parser parser)
    {
      var name = Parsers.property(parser) ?? Parsers.variable(parser);

      if (!string.IsNullOrEmpty(name))
      {
        Node value;

        if ((name[0] != '@') && (parser.Peek(@"([^@+\/*(;{}-]*);")))
          value = parser.Match(@"[^@+\/*(;{}-]*");
        else if (name == "font")
          value = Parsers.font(parser);
        else
          value = Parsers.value(parser);

        if (Parsers.end(parser))
          return new Rule(name, value);
      }

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
    public static Node import(Parser parser)
    {
      Node path = null;

      if (parser.Match(@"@import\s+") && (path = Parsers.entities.quoted(parser) || Parsers.entities.url(parser)) && parser.Match(';'))
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
    public static Node directive(Parser parser) {

      if (parser.CurrentChar != '@') 
        return null;

      Node value;
      if (value = Parsers.import(parser))
        return value;

      NodeList rules;
      var name = parser.MatchString(@"@media|@page");
      if (!string.IsNullOrEmpty(name))
      {
        var types = parser.MatchString(@"[a-z:, ]+").Trim();
        if (rules = Parsers.block(parser))
          return new Directive(name + " " + types, rules);
      }
      else
      {
        name = parser.MatchString(@"@[-a-z]+");
        if (name == "@font-face")
        {
          if (rules = Parsers.block(parser))
            return new Directive(name, rules);
        }
        else if ((value = Parsers.entity(parser)) && parser.Match(';'))
          return new Directive(name, value);
      }

      return null;
    }

    public static Node font(Parser parser)
    {
      var value = new NodeList();
      var expression = new NodeList();
      Node e;

      while (e = Parsers.shorthand(parser) || Parsers.entity(parser))
      {
        expression.Add(e);
      }
      value.Add(new Expression(expression));

      if (parser.Match(','))
      {
        while (e = Parsers.expression(parser))
        {
          value.Add(e);
          if (! parser.Match(','))
            break;
        }
      }
      return new Value(value, Parsers.important(parser));
    }

    //
    // A Value is a comma-delimited list of Expressions
    //
    //     font-family: Baskerville, Georgia, serif;
    //
    // In a Rule, a Value represents everything after the `:`,
    // and before the `;`.
    //
    public static Node value(Parser parser)
    {
      var expressions = new NodeList();

      Node e;
      while (e = Parsers.expression(parser))
      {
        expressions.Add(e);
        if (!parser.Match(','))
          break;
      }
      var important = Parsers.important(parser);

      if (expressions.Count > 0)
        return new Value(expressions, important);

      return null;
    }

    public static Node important(Parser parser)
    {
      return parser.Match(@"!\s*important");
    }

    public static Node sub(Parser parser)
    {
      Node e = null;

      if (parser.Match('(') && (e = Parsers.expression(parser)) && parser.Match(')'))
        return e;

      return null;
    }

    public static Node multiplication(Parser parser)
    {
      var m = Parsers.operand(parser);
      if (!m)
        return null;
    
      var op = parser.Match(@"[\/*]");

      Node a = null;
      if (op && (a = Parsers.multiplication(parser)))
        return new Operation(op.Value, m, a);

      return m;
    }

    public static Node addition(Parser parser)
    {
      var m = Parsers.multiplication(parser);
      if (!m)
        return null;

      var op = parser.Match(@"[-+]\s+");
      if (!op && parser.PreviousChar != ' ')
        op = parser.Match(@"[-+]");

      Node a = null;
      if (op && (a = Parsers.addition(parser)))
        return new Operation(op.Value, m, a);

      return m;
    }

    //
    // An operand is anything that can be part of an operation,
    // such as a Color, or a Variable
    //
    public static Node operand(Parser parser) {
      return Parsers.sub(parser) || 
             Parsers.entities.dimension(parser) ||
             Parsers.entities.color(parser) || 
             Parsers.entities.variable(parser);
    }

    //
    // Expressions either represent mathematical operations,
    // or white-space delimited Entities.
    //
    //     1px solid black
    //     @var * 2
    //
    public static Expression expression(Parser parser)
    {
      Node e;
      var entities = new NodeList();

      while (e = Parsers.addition(parser) || Parsers.entity(parser))
      {
        entities.Add(e);
      }

      if (entities.Count > 0)
        return new Expression(entities);

      return null;
    }

    public static string property(Parser parser)
    {
      var name = parser.Match(@"(\*?-?[-a-z]+)\s*:");

      if (name)
        return name[1];

      return null;
    }
  }
}