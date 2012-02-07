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

namespace dotless.Core.Parser
{
    using System;
    using System.Collections.Generic;
    using Exceptions;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Tree;

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
        public NodeList Primary(Parser parser)
        {
            Node node;
            var root = new NodeList();
            NodeList comments = null;

            GatherComments(parser);

            while (node = MixinDefinition(parser) || Rule(parser) || PullComments() || Ruleset(parser) ||
                          MixinCall(parser) || Directive(parser))
            {
                if (comments = PullComments())
                {
                    root.AddRange(comments);
                }

                comments = node as NodeList;
                if (comments)
                {
                    foreach (Comment c in comments)
                    {
                        c.IsPreSelectorComment = true;
                    }
                    root.AddRange(comments);
                }
                else
                    root.Add(node);

                GatherComments(parser);
            }
            return root;
        }

        private NodeList CurrentComments { get; set; }

        /// <summary>
        ///  Gathers the comments and put them on the stack
        /// </summary>
        private void GatherComments(Parser parser)
        {
            Comment comment;
            while (comment = Comment(parser))
            {
                if (CurrentComments == null)
                {
                    CurrentComments = new NodeList();
                }
                CurrentComments.Add(comment);
            }
        }

        /// <summary>
        ///  Collects comments from the stack retrived when gathering comments
        /// </summary>
        private NodeList PullComments()
        {
            NodeList comments = CurrentComments;
            CurrentComments = null;
            return comments;
        }

        /// <summary>
        ///  The equivalent of gathering any more comments and pulling everything on the stack
        /// </summary>
        private NodeList GatherAndPullComments(Parser parser)
        {
            GatherComments(parser);
            return PullComments();
        }

        private Stack<NodeList> CommentsStack = new Stack<NodeList>();

        /// <summary>
        ///  Pushes comments on to a stack for later use
        /// </summary>
        private void PushComments()
        {
            CommentsStack.Push(PullComments());
        }

        /// <summary>
        ///  Pops the comments stack
        /// </summary>
        private void PopComments()
        {
            CurrentComments = CommentsStack.Pop();
        }

        // We create a Comment node for CSS comments `/* */`,
        // but keep the LeSS comments `//` silent, by just skipping
        // over them.
        public Comment Comment(Parser parser)
        {
            var index = parser.Tokenizer.Location.Index;
            string comment = parser.Tokenizer.GetComment();

            if (comment != null)
            {
                return NodeProvider.Comment(comment, comment.StartsWith("//"), index);
            }

            return null;
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
            var index = parser.Tokenizer.Location.Index;
            var escaped = false;
            var quote = parser.Tokenizer.CurrentChar;

            if (parser.Tokenizer.CurrentChar == '~')
            {
                escaped = true;
                quote = parser.Tokenizer.NextChar;
            }
            if (quote != '"' && quote != '\'')
                return null;

            if (escaped)
                parser.Tokenizer.Match('~');

            string str = parser.Tokenizer.GetQuotedString();

            if (str == null)
                return null;

            return NodeProvider.Quoted(str, str.Substring(1, str.Length - 2), escaped, index);
        }

        //
        // A catch-all word, such as:
        //
        //     black border-collapse
        //
        public Keyword Keyword(Parser parser)
        {
            var index = parser.Tokenizer.Location.Index;

            var k = parser.Tokenizer.Match(@"[A-Za-z-]+");
            if (k)
                return NodeProvider.Keyword(k.Value, index);

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
            var memo = Remember(parser);
            var index = parser.Tokenizer.Location.Index;

            var name = parser.Tokenizer.Match(@"(%|[a-zA-Z0-9_-]+)\(");

            if (!name)
                return null;

            if (name[1].ToLowerInvariant() == "alpha")
            {
                var alpha = Alpha(parser);
                if (alpha != null)
                    return alpha;
            }

            var args = Arguments(parser);

            if (!parser.Tokenizer.Match(')'))
            {
                Recall(parser, memo);
                return null;
            }

            return NodeProvider.Call(name[1], args, index);
        }

        public NodeList<Expression> Arguments(Parser parser)
        {
            var args = new NodeList<Expression>();
            Expression arg;

            while (arg = Expression(parser))
            {
                args.Add(arg);
                if (!parser.Tokenizer.Match(','))
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
            var index = parser.Tokenizer.Location.Index;

            if (parser.Tokenizer.CurrentChar != 'u' || !parser.Tokenizer.Match(@"url\("))
                return null;

            GatherComments(parser);

            Node value = Quoted(parser);

            if (!value)
            {
                var memo = Remember(parser);
                value = Expression(parser);

                if (value && !parser.Tokenizer.Peek(')'))
                {
                    value = null;
                    Recall(parser, memo);
                }
            }
            else
            {
                value.PreComments = PullComments();
                value.PostComments = GatherAndPullComments(parser);
            }

            if (!value)
            {
                value = parser.Tokenizer.MatchAny(@"[^\)""']*") || new TextNode("");
            }

            if (!parser.Tokenizer.Match(')'))
                throw new ParsingException("missing closing ) for url()", parser.Tokenizer.Location.Index);

            return NodeProvider.Url(value, parser.Importer, index);
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
            var index = parser.Tokenizer.Location.Index;

            if (parser.Tokenizer.CurrentChar == '@' && (name = parser.Tokenizer.Match(@"@?@[a-zA-Z0-9_-]+")))
                return NodeProvider.Variable(name.Value, index);

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

            var index = parser.Tokenizer.Location.Index;

            if (parser.Tokenizer.CurrentChar == '#' &&
                (rgb = parser.Tokenizer.Match(@"#([a-fA-F0-9]{6}|[a-fA-F0-9]{3})")))
                return NodeProvider.Color(rgb[1], index);

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

            var index = parser.Tokenizer.Location.Index;

            var value = parser.Tokenizer.Match(@"(-?[0-9]*\.?[0-9]+)(px|%|em|pc|ex|in|deg|s|ms|pt|cm|mm|ch|rem|vw|vh|vm|grad|rad|fr|gr|Hz|kHz)?");

            if (value)
                return NodeProvider.Number(value[1], value[2], index);

            return null;
        }

        //
        // C# code to be evaluated
        //
        //     ``
        //
        public Script Script(Parser parser)
        {
            if (parser.Tokenizer.CurrentChar != '`')
                return null;

            var index = parser.Tokenizer.Location.Index;

            var script = parser.Tokenizer.MatchAny(@"`[^`]*`");

            if (!script)
            {
                return null;
            }

            return NodeProvider.Script(script.Value, index);
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
            if (!parser.Tokenizer.Peek(@"[@%\w.-]+\/[@%\w.-]+"))
                return null;

            var index = parser.Tokenizer.Location.Index;

            Node a = null;
            Node b = null;
            if ((a = Entity(parser)) && parser.Tokenizer.Match('/') && (b = Entity(parser)))
                return NodeProvider.Shorthand(a, b, index);

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
        public MixinCall MixinCall(Parser parser)
        {
            var elements = new NodeList<Element>();
            var index = parser.Tokenizer.Location.Index;

            RegexMatchResult e;
            Combinator c = null;

            PushComments();

            for (var i = parser.Tokenizer.Location.Index; e = parser.Tokenizer.Match(@"[#.][a-zA-Z0-9_-]+"); i = parser.Tokenizer.Location.Index)
            {
                elements.Add(NodeProvider.Element(c, e.Value, i));

                i = parser.Tokenizer.Location.Index;
                var match = parser.Tokenizer.Match('>');
                c = match != null ? NodeProvider.Combinator(match.Value, i) : null;
            }

            var args = new List<NamedArgument>();
            if (parser.Tokenizer.Match('('))
            {
                Expression arg;
                while (arg = Expression(parser))
                {
                    var value = arg;
                    string name = null;

                    if (arg.Value.Count == 1 && arg.Value[0] is Variable)
                    {
                        if (parser.Tokenizer.Match(':'))
                        {
                            if (value = Expression(parser))
                                name = (arg.Value[0] as Variable).Name;
                            else
                                throw new ParsingException("Expected value", parser.Tokenizer.Location.Index);
                        }
                    }

                    args.Add(new NamedArgument { Name = name, Value = value });

                    if (!parser.Tokenizer.Match(','))
                        break;
                }
                if (!parser.Tokenizer.Match(')'))
                    throw new ParsingException("Expected ')'", parser.Tokenizer.Location.Index);
            }

            if (elements.Count > 0)
            {
                // if elements then we've picked up chars so don't need to worry about remembering
                var postComments = GatherAndPullComments(parser);

                if (End(parser))
                {
                    var mixinCall = NodeProvider.MixinCall(elements, args, index);
                    mixinCall.PostComments = postComments;
                    PopComments();
                    return mixinCall;
                }
            }

            PopComments();
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
        public MixinDefinition MixinDefinition(Parser parser)
        {
            if ((parser.Tokenizer.CurrentChar != '.' && parser.Tokenizer.CurrentChar != '#') ||
                parser.Tokenizer.Peek(@"[^{]*(;|})"))
                return null;

            var index = parser.Tokenizer.Location.Index;

            var match = parser.Tokenizer.Match(@"([#.](?:[\w-]|\\(?:[a-fA-F0-9]{1,6} ?|[^a-fA-F0-9]))+)\s*\(");
            if (!match)
                return null;

            //mixin definition ignores comments before it - a css hack can't be part of a mixin definition,
            //so it may as well be a rule before the definition
            PushComments();
            GatherAndPullComments(parser); // no store as mixin definition not output

            var name = match[1];

            var parameters = new NodeList<Rule>();
            RegexMatchResult param = null;
            Node param2 = null;
            Func<bool> matchParam = () => (param = parser.Tokenizer.Match(@"@[\w-]+")) ||
                                          (param2 = Literal(parser) ||
                                                    Keyword(parser));
            for (var i = parser.Tokenizer.Location.Index; matchParam(); i = parser.Tokenizer.Location.Index)
            {
                if (param != null)
                {
                    GatherAndPullComments(parser);
                    if (parser.Tokenizer.Match(':'))
                    {
                        GatherComments(parser);
                        var value = Expression(parser);
                        if (value)
                            parameters.Add(NodeProvider.Rule(param.Value, value, i));
                        else
                            throw new ParsingException("Expected value", i);
                    }
                    else
                        parameters.Add(NodeProvider.Rule(param.Value, null, i));
                }
                else
                {
                    parameters.Add(NodeProvider.Rule(null, param2, i));
                }

                GatherAndPullComments(parser);

                if (!parser.Tokenizer.Match(','))
                    break;

                GatherAndPullComments(parser);
            }
            if (!parser.Tokenizer.Match(')'))
                throw new ParsingException("Expected ')'", parser.Tokenizer.Location.Index);

            GatherAndPullComments(parser);

            var rules = Block(parser);

            PopComments();

            if (rules != null)
                return NodeProvider.MixinDefinition(name, parameters, rules, index);

            return null;
        }

        //
        // Entities are the smallest recognized token,
        // and can be found inside a rule's value.
        //
        public Node Entity(Parser parser)
        {
            return Literal(parser) || Variable(parser) || Url(parser) ||
                   Call(parser) || Keyword(parser) || Script(parser);
        }

        //
        // A Rule terminator. Note that we use `Peek()` to check for '}',
        // because the `block` rule will be expecting it, but we still need to make sure
        // it's there, if ';' was ommitted.
        //
        public bool End(Parser parser)
        {
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

            var index = parser.Tokenizer.Location.Index;

            if (!parser.Tokenizer.Match(@"opacity=", true))
                return null;

            if (value = parser.Tokenizer.Match(@"[0-9]+") || Variable(parser))
            {
                if (!parser.Tokenizer.Match(')'))
                    throw new ParsingException("missing closing ) for alpha()", parser.Tokenizer.Location.Index);

                return NodeProvider.Alpha(value, index);
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
            var index = parser.Tokenizer.Location.Index;

            GatherComments(parser);

            Combinator c = Combinator(parser);

            PushComments();
            GatherComments(parser); // to collect, combinator must have picked up something which would require memory anyway
            var e = parser.Tokenizer.Match(@"[.#:]?[a-zA-Z0-9_-]+") || parser.Tokenizer.Match('*') || Attribute(parser) ||
                    parser.Tokenizer.MatchAny(@"\([^)@]+\)");

            bool isCombinatorAnd = !e && c.Value.StartsWith("&");

            if (e || isCombinatorAnd)
            {
                c.PostComments = PullComments();
                PopComments();
                c.PreComments = PullComments();
                return NodeProvider.Element(c, isCombinatorAnd ? null : e.Value, index);
            }

            PopComments();
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
            var index = parser.Tokenizer.Location.Index;

            Node match;
            if (match = parser.Tokenizer.Match(@"[+>~]") || parser.Tokenizer.Match(@"&\s?") || parser.Tokenizer.Match(@"::"))
                return NodeProvider.Combinator(match.ToString(), index);

            return NodeProvider.Combinator(char.IsWhiteSpace(parser.Tokenizer.GetPreviousCharIgnoringComments()) ? " " : null, index);
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
            int realElements = 0;

            var elements = new NodeList<Element>();
            var index = parser.Tokenizer.Location.Index;

            GatherComments(parser);
            PushComments();

            while (true)
            {
                e = Element(parser);
                if (!e)
                    break;

                realElements++;
                elements.Add(e);
            }

            if (realElements > 0)
            {
                var selector = NodeProvider.Selector(elements, index);
                selector.PostComments = GatherAndPullComments(parser);
                PopComments();
                selector.PreComments = PullComments();

                return selector;
            }

            PopComments();
            //We have lost comments we have absorbed here.
            //But comments should be absorbed before selectors...
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

            var index = parser.Tokenizer.Location.Index;

            if (!parser.Tokenizer.Match('['))
                return null;

            if (key = parser.Tokenizer.Match(@"[a-z-]+") || Quoted(parser))
            {
                Node op;
                if ((op = parser.Tokenizer.Match(@"[|~*$^]?=")) &&
                    (val = Quoted(parser) || parser.Tokenizer.Match(@"[\w-]+")))
                    // Would be nice if this wasn't one block - we could make Attribute node
                    // see CommentsInSelectorAttributes in CommentsFixture.cs
                    attr = string.Format("{0}{1}{2}", key, op, val.ToCSS(new Env())); 
                else
                    attr = key.ToString();
            }

            if (!parser.Tokenizer.Match(']'))
                throw new ParsingException("Excpected ']'", parser.Tokenizer.Location.Index);

            if (!string.IsNullOrEmpty(attr))
                return NodeProvider.TextNode("[" + attr + "]", index);

            return null;
        }

        //
        // The `block` rule is used by `ruleset` and `mixin.definition`.
        // It's a wrapper around the `primary` rule, with added `{}`.
        //
        public NodeList Block(Parser parser)
        {
            if (!parser.Tokenizer.Match('{'))
                return null;

            var content = Primary(parser);

            if (content != null && parser.Tokenizer.Match('}'))
                return content;

            throw new ParsingException("Expected '}'", parser.Tokenizer.Location.Index);
        }

        //
        // div, .class, body > p {...}
        //
        public Ruleset Ruleset(Parser parser)
        {
            var selectors = new NodeList<Selector>();

            var memo = Remember(parser);
            var index = memo.TokenizerLocation.Index;

            if (parser.Tokenizer.Peek(@"([a-z.#: _-]+)[\s\n]*\{")) //simple case with no comments
            {
                var match = parser.Tokenizer.Match(@"[a-z.#: _-]+");
                selectors =
                    new NodeList<Selector>(
                        NodeProvider.Selector(new NodeList<Element>(NodeProvider.Element(null, match.Value, index)), index));
            }
            else
            {
                Selector s;
                while (s = Selector(parser))
                {
                    selectors.Add(s);
                    if (!parser.Tokenizer.Match(','))
                        break;

                    GatherComments(parser);
                }
            }

            NodeList rules;

            if (selectors.Count > 0 && (rules = Block(parser)) != null)
                return NodeProvider.Ruleset(selectors, rules, index);

            Recall(parser, memo);

            return null;
        }

        public Rule Rule(Parser parser)
        {
            var memo = Remember(parser);
            PushComments();

            var name = Property(parser) ?? VariableName(parser);

            var postNameComments = GatherAndPullComments(parser);

            if (name != null && parser.Tokenizer.Match(':'))
            {
                Node value;

                var preValueComments = GatherAndPullComments(parser);

                if ((name[0] != '@') && (parser.Tokenizer.Peek(@"([^@+\/*`(;{}'""-]*);")))
                    value = parser.Tokenizer.Match(@"[^@+\/*`(;{}'""-]*");
                else if (name == "font")
                    value = Font(parser);
                else
                    value = Value(parser);

                var postValueComments = GatherAndPullComments(parser);

                if (End(parser))
                {
                    value.PreComments = preValueComments;
                    value.PostComments = postValueComments;

                    var rule = NodeProvider.Rule(name, value, memo.TokenizerLocation.Index);
                    rule.PostNameComments = postNameComments;
                    PopComments();
                    return rule;
                }
            }

            PopComments();
            Recall(parser, memo);

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

            var index = parser.Tokenizer.Location.Index;

            if (parser.Tokenizer.Match(@"@import\s+") && (path = Quoted(parser) || Url(parser)))
            {
                if (!parser.Tokenizer.Match(';'))
                    throw new ParsingException("Expected ';'", parser.Tokenizer.Location.Index);

                if (path is Quoted)
                    return NodeProvider.Import(path as Quoted, parser.Importer, index);

                if (path is Url)
                    return NodeProvider.Import(path as Url, parser.Importer, index);
            }

            return null;
        }

        //
        // A CSS Directive
        //
        //     @charset "utf-8";
        //
        public Directive Directive(Parser parser)
        {
            if (parser.Tokenizer.CurrentChar != '@')
                return null;

            var import = Import(parser);
            if (import)
                return import;

            GatherComments(parser);

            var index = parser.Tokenizer.Location.Index;

            var name = parser.Tokenizer.MatchString(@"@[-a-z]+");
            bool hasIdentifier = false, hasBlock = false, isKeyFrame = false;
            NodeList rules, preRulesComments = null, preComments = null;
            string identifierRegEx = @"[^{]+";
            string nonVendorSpecificName = name;

            if (name.StartsWith("@-") && name.IndexOf('-', 2) > 0)
            {
                nonVendorSpecificName = "@" + name.Substring(name.IndexOf('-', 2) + 1);
            }

            switch (nonVendorSpecificName)
            {
                case "@font-face":
                    hasBlock = true;
                    break;
                case "@page":
                case "@document":
                case "@media":
                case "@supports":
                    hasBlock = true;
                    hasIdentifier = true;
                    break;
                case "@top-left":
                case "@top-left-corner":
                case "@top-center":
                case "@top-right":
                case "@top-right-corner":
                case "@bottom-left":
                case "@bottom-left-corner":
                case "@bottom-center":
                case "@bottom-right":
                case "@bottom-right-corner":
                case "@left-top":
                case "@left-middle":
                case "@left-bottom":
                case "@right-top":
                case "@right-middle":
                case "@right-bottom":
                    hasBlock = true;
                    break;
                case "@keyframes":
                    isKeyFrame = true;
                    hasIdentifier = true;
                    break;
            }

            string identifier = "";

            preComments = PullComments();

            if (hasIdentifier)
            {
                GatherComments(parser);

                var identifierRegResult = parser.Tokenizer.MatchAny(identifierRegEx);
                if (identifierRegResult != null)
                {
                    identifier = identifierRegResult.Value.Trim();
                }
            }

            preRulesComments = GatherAndPullComments(parser);

            if (hasBlock)
            {
                rules = Block(parser);

                if (rules != null) {
                    rules.PreComments = preRulesComments;
                    return NodeProvider.Directive(name, identifier, rules, index);
                }
            }
            else if (isKeyFrame)
            {
                var keyframeblock = KeyFrameBlock(parser, name, identifier, index);
                keyframeblock.PreComments = preRulesComments;
                return keyframeblock;
            }
            else
            {
                Node value;
                if (value = Expression(parser)) {
                    value.PreComments = preRulesComments;
                    value.PostComments = GatherAndPullComments(parser);
                    if (parser.Tokenizer.Match(';')) {
                        var directive = NodeProvider.Directive(name, value, index);
                        directive.PreComments = preComments;
                        return directive;
                    }
                }
            }

            return null;
        }

        public Directive KeyFrameBlock(Parser parser, string name, string identifier, int index)
        {
            if (!parser.Tokenizer.Match('{'))
                return null;

            NodeList keyFrames = new NodeList();
            const string identifierRegEx = "from|to|([0-9]+%)";

            while (true)
            {
                GatherComments(parser);

                string keyFrameIdentifier;
                var keyFrameIdentifier1 = parser.Tokenizer.Match(identifierRegEx);

                if (!keyFrameIdentifier1)
                    break;

                keyFrameIdentifier = keyFrameIdentifier1.Value;

                if (parser.Tokenizer.Match(","))
                {
                    var keyFrameIdentifier2 = parser.Tokenizer.Match(identifierRegEx);

                    if (!keyFrameIdentifier2)
                        throw new ParsingException("Comma in @keyframe followed by unknown identifier", parser.Tokenizer.Location.Index);

                    keyFrameIdentifier += "," + keyFrameIdentifier2;
                }

                var preComments = GatherAndPullComments(parser);

                var block = Block(parser);

                if (block == null)
                    throw new ParsingException("Expected css block after key frame identifier", parser.Tokenizer.Location.Index);

                block.PreComments = preComments;
                block.PostComments = GatherAndPullComments(parser);

                keyFrames.Add(NodeProvider.KeyFrame(keyFrameIdentifier, block, parser.Tokenizer.Location.Index));
            }

            if (!parser.Tokenizer.Match('}'))
                throw new ParsingException("Expected start, finish, % or '}'", parser.Tokenizer.Location.Index);

            return NodeProvider.Directive(name, identifier, keyFrames, index);
        }

        public Value Font(Parser parser)
        {
            var value = new NodeList();
            var expression = new NodeList();
            Node e;

            var index = parser.Tokenizer.Location.Index;

            while (e = Shorthand(parser) || Entity(parser))
            {
                expression.Add(e);
            }
            value.Add(NodeProvider.Expression(expression, index));

            if (parser.Tokenizer.Match(','))
            {
                while (e = Expression(parser))
                {
                    value.Add(e);
                    if (!parser.Tokenizer.Match(','))
                        break;
                }
            }
            return NodeProvider.Value(value, Important(parser), index);
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

            var index = parser.Tokenizer.Location.Index;

            Node e;
            while (e = Expression(parser))
            {
                expressions.Add(e);
                if (!parser.Tokenizer.Match(','))
                    break;
            }

            GatherComments(parser);

            var important = Important(parser);

            if (expressions.Count > 0)
            {
                var value = NodeProvider.Value(expressions, important, index);

                if (!string.IsNullOrEmpty(important))
                {
                    value.PreImportantComments = PullComments();
                }

                return value;
            }

            return null;
        }

        public string Important(Parser parser)
        {
            var important = parser.Tokenizer.Match(@"!\s*important");

            return important == null ? "" : important.Value;
        }

        public Expression Sub(Parser parser)
        {
            if (!parser.Tokenizer.Match('('))
                return null;

            var e = Expression(parser);
            if (e != null && parser.Tokenizer.Match(')'))
                return e;

            return null; // might be an attribute selector or something else
        }

        public Node Multiplication(Parser parser)
        {
            GatherComments(parser);

            var m = Operand(parser);
            if (!m)
                return null;

            Node operation = m;

            while (true)
            {
                GatherComments(parser); // after left operand

                var index = parser.Tokenizer.Location.Index;
                var op = parser.Tokenizer.Match(@"[\/*]");

                GatherComments(parser); // after operation

                Node a = null;
                if (op && (a = Operand(parser)))
                    operation = NodeProvider.Operation(op.Value, operation, a, index);
                else
                    break;
            }
            return operation;
        }

        public Node Addition(Parser parser)
        {
            var m = Multiplication(parser);
            if (!m)
                return null;

            Operation operation = null;
            while (true)
            {
                GatherComments(parser);

                var index = parser.Tokenizer.Location.Index;
                var op = parser.Tokenizer.Match(@"[-+]\s+");
                if (!op && !char.IsWhiteSpace(parser.Tokenizer.GetPreviousCharIgnoringComments()))
                    op = parser.Tokenizer.Match(@"[-+]");

                Node a = null;
                if (op && (a = Multiplication(parser)))
                    operation = NodeProvider.Operation(op.Value, operation ?? m, a, index);
                else
                    break;
            }
            return operation ?? m;
        }

        //
        // An operand is anything that can be part of an operation,
        // such as a Color, or a Variable
        //
        public Node Operand(Parser parser)
        {
            CharMatchResult negate = null;

            if (parser.Tokenizer.CurrentChar == '-' && parser.Tokenizer.Peek(@"-[@\(]"))
            {
                negate = parser.Tokenizer.Match('-');
                GatherComments(parser);
            }

            var operand = Sub(parser) ??
                          Dimension(parser) ??
                          Color(parser) ??
                          (Node)Variable(parser);

            if (operand != null)
            {
                return negate ?
                    NodeProvider.Operation("*", NodeProvider.Number("-1", "", negate.Index), operand, negate.Index) :
                    operand;
            }

            if (parser.Tokenizer.CurrentChar == 'u' && parser.Tokenizer.Peek(@"url\("))
                return null;

            return Call(parser) || Keyword(parser);
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

            var index = parser.Tokenizer.Location.Index;

            while (e = Addition(parser) || Entity(parser))
            {
                e.PostComments = PullComments();
                entities.Add(e);
            }

            if (entities.Count > 0)
                return NodeProvider.Expression(entities, index);

            return null;
        }

        public string Property(Parser parser)
        {
            var name = parser.Tokenizer.Match(@"\*?-?[-_a-zA-Z][-_a-z0-9A-Z]*");

            if (name)
                return name.Value;

            return null;
        }

        public class ParserLocation
        {
            public NodeList Comments { get; set; }
            public Location TokenizerLocation { get; set; }
        }

        public ParserLocation Remember(Parser parser)
        {
            return new ParserLocation() { Comments = CurrentComments, TokenizerLocation = parser.Tokenizer.Location };
        }

        public void Recall(Parser parser, ParserLocation location)
        {
            CurrentComments = location.Comments;
            parser.Tokenizer.Location = location.TokenizerLocation;
        }
    }
}