#region

using System;
using System.Collections.Generic;
using System.IO;
using nLess;
using nless.Core.engine;
using Peg.Base;

#endregion

namespace nless.Core.parser
{
    internal static class IntParseExtentions
    {
        internal static EnLess ToEnLess(this int i)
        {
            return (EnLess) i;
        }
    }

    public static class ParserWrapper
    {
        //Hack until I can work out how to retrieve return from the parser
        public static Element Env { get; set; }

        public static void Parse(string src, TextWriter errorOut)
        {
            var parser = new nLess.nLess(src, errorOut);
            var bMatches = parser.Parse();

            if (!bMatches)
            {
                Console.WriteLine("FAILURE: Json Parser did not match input file ");
            }
            else
            {
                Console.WriteLine("SUCCESS: Json Parser matched input file");
                var root = parser.GetRoot();
                var walker = new TreeWalker(root, src);
                var nLessRoot = walker.Walk();
                Console.WriteLine(nLessRoot.ToCss());

                //var tw = new StreamWriter(File.OpenWrite("out.txt"));

                var tprint = new TreePrint(Console.Out, src, 60, new NodePrinter(parser).GetNodeName, false);
                tprint.PrintTree(parser.GetRoot(), 0, 0);
            }
        }
    }

    internal class NodePrinter
    {
        private readonly PegBaseParser parser_;

        internal NodePrinter(PegBaseParser parser)
        {
            parser_ = parser;
        }

        internal string GetNodeName(PegNode n)
        {
            return parser_.GetRuleNameFromId(n.id_);
        }
    }

    public class TreeWalker
    {
        public TreeWalker(PegNode root, string src)
        {
            Root = root;
            Src = src;
        }

        public PegNode Root { get; set; }
        public string Src { get; set; }

        public Element Walk()
        {
            var el = new Element("");
            Primary(Root.child_, el);
            return el;
        }

        //(comment/ ruleset /declaration)*
        private void Primary(PegNode node, Element element)
        {
            var nextPrimary = node.child_;
            while (nextPrimary != null)
            {
                switch (nextPrimary.id_.ToEnLess())
                {
                    case EnLess.ruleset:
                        BuildRuleSet(nextPrimary.child_, element);
                        break;
                    case EnLess.declaration:
                        BuildDeclaration(nextPrimary.child_, element);
                        break;
                }
                nextPrimary = nextPrimary.next_;
            }
        }

        private void BuildDeclaration(PegNode node, Element element)
        {
             if(node.id_.ToEnLess() == EnLess.standard_declaration)
             {
                 node = node.child_;
                 var name = node.GetAsString(Src).Replace(" ", "");
                 var property = new Property(name);
                 var experssions = Expressions(node.next_, element);
                 foreach(var expression in experssions)
                     property.Add(expression);
                 element.Add(property);
             }
             else if(node.id_.ToEnLess() == EnLess.catchall_declaration)
             {
                 node = node.child_;
                //TODO: Should I be doing something here?
             }

            /*
           (name.text_value =~ /^@/ ? 
            Node::Variable : Node::Property).new(name.text_value, expressions.build(env), env)
           */
        }

        private IList<INode> Expressions(PegNode node, Element element)
        {
            var lessNodes = new List<INode>();
            node = node.child_; // Expression
            while (node != null){
                switch (node.id_.ToEnLess())
                {
                    case EnLess.operation_expressions:
                        foreach(var exprNode in Expression(node.child_, element))
                            lessNodes.Add(exprNode);
                        node = node.next_;
                        while (node!=null)
                        {
                            if (node.id_.ToEnLess() == EnLess.@operator)
                                lessNodes.Add(new Operator(node.GetAsString(Src)));
                            else if (node.id_.ToEnLess() == EnLess.expressions)
                                Expressions(node, element);
                            node = node.next_;
                        }
                        break;
                    case EnLess.space_delimited_expressions:

                        break;
                }
                node = node.next_;
            }
            
            return lessNodes;
        }

        private IList<INode> Expression(PegNode node, Element element)
        {
            var expression = new List<INode>();
            while (node != null)
            {
                switch (node.id_.ToEnLess())
                {
                    case EnLess.expressions:
                        expression.AddRange(Expressions(node.child_, element));
                        break;
                    case EnLess.entity:
                        expression.Add(new Expression(Entity(node.child_, element)));
                        break;
                }
                node = node.next_;
            }
            return expression;
        }

        private IList<INode> Entity(PegNode node, Element element)
        {
            var nodes = new List<INode>();
            if (node.id_.ToEnLess()==EnLess.literal) node = node.child_;
            switch (node.id_.ToEnLess())
            {
                case EnLess.number:
                    var val = float.Parse(node.GetAsString(Src));
                    var unit = "";
                    node = node.next_;
                    if (node != null && node.id_.ToEnLess() == EnLess.unit) unit = node.GetAsString(Src);
                    nodes.Add(new Number(unit, val));
                    break;
                case EnLess.color:
                    break;
            }

            return nodes;
        }

        //ruleset: selectors [{] ws prsimary ws [}] ws /  ws selectors ';' ws;
        private void BuildRuleSet(PegNode node, Element element)
        {
            if(node.id_.ToEnLess() == EnLess.standard_ruleset)
            {
                node = node.child_;
                var elements = Selectors(node, element, els =>{
                                                                foreach (var el in els)
                                                                    element.Add(el);
                                                                return element.Last;
                                                            });
                foreach (var el in elements)
                    Primary(node.next_, el);
            }
            else if(node.id_.ToEnLess() == EnLess.mixin_ruleset)
            {
                //TODO: Mixins
            }
        }
        private IList<Element> Selectors(PegNode node, Element el, Func<IList<Element>, Element> action)
        {
            var selector = node.child_;
            var elements = new List<Element>();
            while (selector != null && selector.id_.ToEnLess() == EnLess.selector)
            {
                elements.Add(action.Invoke(Selector(selector.child_)));
                selector = selector.next_;
            }
            return elements;
        }
        private IList<Element> Selector(PegNode node)
        {
            var elements = new List<Element>();
            while (node != null)
            {
                if (node.id_.ToEnLess() != EnLess.select)
                    throw new ParsingException("Selectors must be something");
                {
                    var selector = node.GetAsString(Src).Replace(" ", "");
                    node = node.next_;
                    var name = node.GetAsString(Src);
                    elements.Add(new Element(name, selector));
                }

                node = node.next_;
            }
            return elements;
        }



        private void spt(PegNode node)
        {
            if (node != null)
            {
                Console.WriteLine("Node: {0}:{1}", node.id_.ToEnLess(), node.GetAsString(Src));
                if (node.next_ != null) Console.WriteLine("Next: {0}:{1}", node.next_.id_.ToEnLess(), node.GetAsString(Src));
                if (node.child_ != null) Console.WriteLine("Child: {0}:{1}", node.child_.id_.ToEnLess(), node.GetAsString(Src));
            }
        }
    }

    internal class ParsingException : Exception
    {
        public ParsingException(string s) : base(s)
        {
        }
    }
}