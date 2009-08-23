using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using nLess;
using nless.Core.engine;
using nless.Core.engine.nodes.Literals;
using Peg.Base;
using String=nless.Core.engine.String;

namespace nless.Core.parser
{
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
                        RuleSet(nextPrimary.child_, element);
                        break;
                    case EnLess.declaration:
                        Declaration(nextPrimary.child_, element);
                        break;
                }
                nextPrimary = nextPrimary.next_;
            }
        }

        private void Declaration(PegNode node, Element element)
        {
            if (node.id_.ToEnLess() == EnLess.standard_declaration)
            {
                node = node.child_;
                var name = node.GetAsString(Src).Replace(" ", "");

                if (name.Substring(0, 1) == "@")
                {
                    var property = new Variable(name, Expressions(node.next_, element));
                    element.Add(property);
                }
                else
                {
                    var property = new Property(name, Expressions(node.next_, element));
                    element.Add(property);
                }
            }
            else if (node.id_.ToEnLess() == EnLess.catchall_declaration)
            {
                node = node.child_;
                //TODO: Should I be doing something here?
            }
        }

        private IList<INode> Expressions(PegNode node, Element element)
        {
            // Expression
            switch (node.id_.ToEnLess())
            {
                case EnLess.operation_expressions:
                    return OperationExpressions(node.child_, element);
                case EnLess.space_delimited_expressions:
                    return SpaceDelimitedExpressions(node.child_, element);
                default:
                    if (node.child_ == null) //CatchAll
                        return new List<INode>
                                   {
                                       new Anonymous(node.GetAsString(Src))
                                   };
                    return Expressions(node.child_, element);
            }
        }

        //expression tail:(operator expression)+ 
        private IList<INode> OperationExpressions(PegNode node, Element element)
        {
            var lessNodes = new List<INode>();
            lessNodes.Add(Expression(node.child_, element)); //First expression
            node = node.next_;

            //Tail
            while (node != null)
            {
                switch (node.id_.ToEnLess())
                {
                    case EnLess.@operator:
                        lessNodes.Add(new Operator(node.GetAsString(Src)));
                        break;
                    case EnLess.expression:
                        lessNodes.Add(Expression(node.child_, element));
                        ;
                        break;
                }
                node = node.next_;
            }

            return lessNodes;
        }

        //expression (WS expression)* important?;
        private IList<INode> SpaceDelimitedExpressions(PegNode node, Element element)
        {
            var lessNodes = new List<INode> {Expression(node.child_, element)};
            node = node.next_;

            //Tail
            while (node != null)
            {
                switch (node.id_.ToEnLess())
                {
                    case EnLess.expression:
                        lessNodes.Add(Expression(node.child_, element));;
                        break;
                    case EnLess.important:
                        lessNodes.Add(Keyword(node, element));
                        break;
                }
                node = node.next_;
            }

            return lessNodes;
        }

        private INode Expression(PegNode node, Element element)
        {
            switch (node.id_.ToEnLess())
            {
                case EnLess.expressions:
                    return new Expression(Expressions(node, element));
                    break;
                case EnLess.entity:
                    return Entity(node.child_, element);
                    break;
            }
            throw new ParsingException("Expression should either be child expressions or an entity");
        }

        //function / fonts / keyword  / variable / literal ;
        private INode Entity(PegNode node, Element element)
        {
            var nodes = new List<INode>();
            switch (node.id_.ToEnLess())
            {
                case EnLess.literal:
                    return Entity(node.child_, element);
                case EnLess.number:
                    return Number(node, element);
                case EnLess.color:
                    return Color(node, element);
                case EnLess.variable:
                    return Variable(node, element);
                case EnLess.fonts:
                    return Fonts(node.child_, element);
                case EnLess.keyword:
                    return Keyword(node, element);
                case EnLess.function:
                    return Function(node.child_, element);
            }

            return new Anonymous(node.GetAsString(Src));
        }

        private INode Function(PegNode node, Element element)
        {
            var funcName = node.GetAsString(Src);
            var arguments = Arguments(node.next_, element);
            return new Function(funcName, arguments);
        }

        private IList<INode> Arguments(PegNode node, Element element)
        {
            var args = new List<INode>();
            while (node != null)
            {
                args.Add(Entity(node, element));
                node = node.next_;
            }
            return args;
        }

        private INode Keyword(PegNode node, Element element)
        {
            return new Keyword(node.GetAsString(Src));
        }

        private INode Fonts(PegNode node, Element element)
        {
            var fonts = new List<Literal>();
            while (node!=null)
            {
                if (node.child_ != null) fonts.Add(new String(node.child_.GetAsString(Src)));
                else fonts.Add(new Keyword(node.GetAsString(Src)));
                node = node.next_;
            }
            return new FontFamily(fonts.ToArray());
        }

        private INode Number(PegNode node, Element element)
        {
            var val = float.Parse(node.GetAsString(Src));
            var unit = "";
            node = node.next_;
            if (node != null && node.id_.ToEnLess() == EnLess.unit) unit = node.GetAsString(Src);
            return new Number(unit, val);
        }

        private INode Color(PegNode node, Element element)
        {
            return RGB(node.child_, element);
        }

        private INode RGB(PegNode node, Element element)
        {
            int R = 0, G = 0, B = 0;
            var rgbNode = node.child_; //Fisrt node;
            if (rgbNode != null)
            {
                R = int.Parse(rgbNode.GetAsString(Src), NumberStyles.HexNumber);
                rgbNode = rgbNode.next_;
                if (rgbNode != null)
                {
                    G = int.Parse(rgbNode.GetAsString(Src), NumberStyles.HexNumber);
                    rgbNode = rgbNode.next_;
                    if (rgbNode != null)
                    {
                        B = int.Parse(rgbNode.GetAsString(Src), NumberStyles.HexNumber);
                    }
                }
            }
            return new Color(R, G, B);
        }


        private INode Variable(PegNode node, Element element)
        {
            return new Variable(node.GetAsString(Src));
        }

        //ruleset: selectors [{] ws prsimary ws [}] ws /  ws selectors ';' ws;
        private void RuleSet(PegNode node, Element element)
        {
            if (node.id_.ToEnLess() == EnLess.standard_ruleset)
            {
                node = node.child_;
                var elements = Selectors(node, element, els =>
                                                            {
                                                                foreach (var el in els)
                                                                    element.Add(el);
                                                                return element.Last;
                                                            });
                foreach (var el in elements)
                    Primary(node.next_, el);
            }
            else if (node.id_.ToEnLess() == EnLess.mixin_ruleset)
            {
                node = node.child_;
                var elements = Selectors(node, element, els =>
                                                            {
                                                                return els.First(); //TODO: This must be wrong
                                                            });
                foreach (var el in elements){
                    var root = element.GetRoot();
                    var rules = root.Descend(el.Selector, el).Rules;
                    element.Rules.AddRange(rules);
                }
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
                if (node.next_ != null)
                    Console.WriteLine("Next: {0}:{1}", node.next_.id_.ToEnLess(), node.GetAsString(Src));
                if (node.child_ != null)
                    Console.WriteLine("Child: {0}:{1}", node.child_.id_.ToEnLess(), node.GetAsString(Src));
            }
        }
    }
}