using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using nLess;
using nless.Core.engine;
using nless.Core.engine.nodes.Literals;
using nless.Core.Exceptions;
using Peg.Base;
using String=nless.Core.engine.String;

namespace nless.Core.parser
{
    public class TreeBuilder
    {
        public PegNode Root { get; set; }
        public string Src { get; set; }
        public TreeBuilder(PegNode root, string src)
        {
            Root = root;
            Src = src;
        }
        
        /// <summary>
        /// Main entry point for the build
        /// </summary>
        /// <returns></returns>
        public Element Build()
        {
            var el = new Element("");
            Primary(Root.child_, el);
            return el;
        }

        /// <summary>
        /// primary: (import / declaration / ruleset / comment)* ;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="element"></param>
        private void Primary(PegNode node, Element element)
        {
            var nextPrimary = node.child_;
            if (nextPrimary == null && node.next_ != null) Primary(node.next_, element);
            else
            {
                while (nextPrimary != null)
                {
                    switch (nextPrimary.id_.ToEnLess())
                    {
                        case EnLess.import:
                            Import(nextPrimary.child_, element);
                            break;
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
        }

        /// <summary>
        /// import :  ws '@import'  S import_url medias? s ';' ;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="element"></param>
        private void Import(PegNode node, Element element)
        {
            var path = node.GetAsString(Src);
            if(node.child_ != null){
                path = node.child_.GetAsString(Src); 
            }
            path = path.Replace("\"", "").Replace("'", "");
           //TODO: Fuck around with pah to make it absolute relative

            if(HttpContext.Current!=null)
            {
                path = HttpContext.Current.Server.MapPath(path);
            }

           if(File.Exists(path))
           {
               var engine = new Engine(File.ReadAllText(path), null);
               element.Rules.AddRange(engine.Parse().Root.Rules); 
           }
        }

        /// <summary>
        /// declaration:  standard_declaration / catchall_declaration ;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="element"></param>
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
/*                node = node.child_;
                var name = node.GetAsString(Src).Replace(" ", "");
                element.Add(new Property(name));*/
                
                //TODO: Should I be doing something here?
            }
        }

        /// <summary>
        /// expressions: operation_expressions / space_delimited_expressions / [-a-zA-Z0-9_%*/.&=:,#+? \[\]()]+ ;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="element"></param>
        /// <returns></returns>
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

        /// <summary>
        /// operation_expressions:  expression (operator expression)+;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private IList<INode> OperationExpressions(PegNode node, Element element)
        {
            var lessNodes = new List<INode> {Expression(node.child_, element)};
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
                        break;
                }
                node = node.next_;
            }

            return lessNodes;
        }

        /// <summary>
        /// space_delimited_expressions: expression (WS expression)* important? ;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="element"></param>
        /// <returns></returns>
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
                        lessNodes.Add(new Keyword("!important"));
                        break;
                }
                node = node.next_;
            }

            return lessNodes;
        }

        /// <summary>
        /// expression: '(' s expressions s ')' / entity ;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private INode Expression(PegNode node, Element element)
        {
            switch (node.id_.ToEnLess())
            {
                case EnLess.expressions:
                    return new Expression(Expressions(node, element));
                case EnLess.entity:
                    return Entity(node.child_, element);
            }
            throw new ParsingException("Expression should either be child expressions or an entity");
        }

        /// <summary>
        /// entity :  function / fonts / accessor / keyword  / variable / literal  ;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private INode Entity(PegNode node, Element element)
        {
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
                case EnLess.accessor:
                    return Accessor(node.child_, element);
                case EnLess.fonts:
                    return Fonts(node.child_, element);
                case EnLess.keyword:
                    return Keyword(node, element);
                case EnLess.function:
                    return Function(node.child_, element);
            }

            return new Anonymous(node.GetAsString(Src));
        }

        /// <summary>
        /// accessor: accessor_name '[' accessor_key ']'; 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private INode Accessor(PegNode node, Element element)
        {
            var ident = node.GetAsString(Src);
            var key = node.next_.GetAsString(Src).Replace("'", "");
            var el = element.NearestAs<Element>(ident);
            if (el!=null)
            {
                var prop = el.GetAs<Property>(key);
                if (((INode)prop) != null) return prop.Value;
            }
            return new Anonymous("");
        }

        /// <summary>
        /// function: function_name arguments ;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private INode Function(PegNode node, Element element)
        {
            var funcName = node.GetAsString(Src);
            var arguments = Arguments(node.next_, element);
            return new Function(funcName, arguments);
        }

        /// <summary>
        /// arguments : '(' s argument s (',' s argument s)* ')';
        /// </summary>
        /// <param name="node"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private IList<INode> Arguments(PegNode node, Element element)
        {
            var args = new List<INode>();
            while (node != null)
            {
                switch (node.id_.ToEnLess())
                {
                    case EnLess.color:
                        args.Add(Color(node, element));
                        break;
                    case EnLess.number:
                        args.Add(Number(node, element));
                        break;
                    case EnLess.@string:
                        args.Add(new String(node.GetAsString(Src)));
                        break;
                    case EnLess.keyword:
                        args.Add(new Keyword(node.GetAsString(Src)));
                        break;
                    default:
                        args.Add(new Anonymous(node.GetAsString(Src)));
                        break;
                }
                node = node.next_;
            }
            return args;
        }

        /// <summary>
        /// keyword: [-a-zA-Z]+ !ns;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private INode Keyword(PegNode node, Element element)
        {
            return new Keyword(node.GetAsString(Src));
        }

        /// <summary>
        /// fonts : font (s ',' s font)+  ;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="element"></param>
        /// <returns></returns>
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

        /// <summary>
        /// number: '-'? [0-9]* '.' [0-9]+ / '-'? [0-9]+;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private INode Number(PegNode node, Element element)
        {
            var val = float.Parse(node.GetAsString(Src), NumberFormatInfo.InvariantInfo);
            var unit = "";
            node = node.next_;
            if (node != null && node.id_.ToEnLess() == EnLess.unit) unit = node.GetAsString(Src);
            return new Number(unit, val);
        }

        /// <summary>
        /// color: '#' rgb;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private INode Color(PegNode node, Element element)
        {
            return RGB(node.child_, element);
        }

        /// <summary>
        /// rgb:(rgb_node)(rgb_node)(rgb_node) / hex hex hex ;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private INode RGB(PegNode node, Element element)
        {
            int R = 0, G = 0, B = 0;
            string tmp;

            var rgbNode = node.child_; //Fisrt node;
            if (rgbNode != null)
            {
                tmp = rgbNode.GetAsString(Src);
                R = int.Parse(tmp.Length==1 ? tmp+tmp : tmp, NumberStyles.HexNumber);
                rgbNode = rgbNode.next_;
                if (rgbNode != null)
                {
                    tmp = rgbNode.GetAsString(Src);
                    G = int.Parse(tmp.Length == 1 ? tmp + tmp : tmp, NumberStyles.HexNumber);
                    rgbNode = rgbNode.next_;
                    if (rgbNode != null)
                    {
                        tmp = rgbNode.GetAsString(Src);
                        B = int.Parse(tmp.Length == 1 ? tmp + tmp : tmp, NumberStyles.HexNumber);
                    }
                }
            }
            return new Color(R, G, B);
        }

        /// <summary>
        /// variable: '@' [-_a-zA-Z0-9]+; 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private INode Variable(PegNode node, Element element)
        {
            return new Variable(node.GetAsString(Src));
        }

        /// <summary>
        /// ruleset: selectors [{] ws prsimary ws [}] ws /  ws selectors ';' ws;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="element"></param>
        private void RuleSet(PegNode node, Element element)
        {
            if (node.id_.ToEnLess() == EnLess.standard_ruleset)
            {
                node = node.child_;
                var elements = Selectors(node, element, els => StandardSelectors(element, els));
                foreach (var el in elements)
                    Primary(node.next_, el);
            }
            else if (node.id_.ToEnLess() == EnLess.mixin_ruleset)
            {
                node = node.child_;
                var elements = Selectors(node, element, els => els);
                foreach (var el in elements){
                    var root = element.GetRoot();
                    var rules = root.Descend(el.Selector, el).Rules;
                    element.Rules.AddRange(rules);
                }
            }
        }

        /// <summary>
        /// standard_ruleset: ws selectors [{] ws primary ws [}] ws;
        /// </summary>
        /// <param name="element"></param>
        /// <param name="els"></param>
        /// <returns></returns>
        private static IList<Element> StandardSelectors(Element element, IEnumerable<Element> els)
        {
            foreach (var el in els){
                element.Add(el);
                element = element.Last;
            }
            return new List<Element> { element};
        }

        /// <summary>
        /// selectors :  ws selector (s ',' ws selector)* ws ;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="el"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private IList<Element> Selectors(PegNode node, Element el, Func<IList<Element>, IList<Element>> action)
        {
            var selector = node.child_;
            var elements = new List<Element>();
            while (selector != null && selector.id_.ToEnLess() == EnLess.selector)
            {
                elements.AddRange(action.Invoke(Selector(selector.child_)));
                selector = selector.next_;
            }
            return elements;
        }

        /// <summary>
        /// selector : (s select element s)+ arguments? ;
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
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
    }
}