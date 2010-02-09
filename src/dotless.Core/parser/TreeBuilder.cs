/* Copyright 2009 dotless project, http://www.dotlesscss.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *     
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License. */

using System.Linq;

namespace dotless.Core.parser
{
    using engine;
    using exceptions;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using nLess;
    using Peg.Base;
    using String=engine.String;

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
        public ElementBlock Build()
        {
            return Primary(Root.child_);
        }

        /// <summary>
        /// Main entry point for the build
        /// </summary>
        /// <returns></returns>
        public ElementBlock Build(ElementBlock tail)
        {
            return tail == null ? Build() : Primary(Root.child_, tail);
        }

        private ElementBlock Primary(PegNode node)
        {
            var element = new ElementBlock("");
            return Primary(node, element);
        }

        /// <summary>
        /// primary: (import / declaration / ruleset / comment)* ;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="elementBlock"></param>
        private ElementBlock Primary(PegNode node, ElementBlock elementBlock)
        {
            foreach (var nextPrimary in node.AsEnumerable())
            {
                switch (nextPrimary.id_.ToEnLess())
                {
                    case EnLess.import:
                        Import(nextPrimary.child_, elementBlock);
                        //element.Children.AddRange(import);
                        break;
                    case EnLess.insert:
                        Insert(nextPrimary.child_, elementBlock);
                        //element.Children.AddRange(import);
                        break;
                    case EnLess.standard_ruleset:
                        RuleSet(nextPrimary, elementBlock);
                        break;
                    case EnLess.mixin_ruleset:
                        Mixin(nextPrimary,elementBlock);
                        break;
                    case EnLess.declaration:
                        Declaration(nextPrimary.child_, elementBlock);
                        break;
                }
            }
            return elementBlock;
        }

        /// <summary>
        /// import :  ws '@import'  S import_url medias? s ';' ;
        /// </summary>
        /// <param name="node"></param>
        private IEnumerable<INode> Import(PegNode node, ElementBlock elementBlock)
        {
            node = (node.child_ ?? node);
//
//            var path = "";
//            if(node.ToEnLess()==EnLess.expressions)
//            {
//                var values = Expressions(node, element);
//                var fakeVariableName = new Variable(string.Format("@{0}", DateTime.Now.Ticks), values);
//                element.Add(fakeVariableName);
//                path = fakeVariableName.ToCss();
//            }
//            else
//            {
//                path = (node).GetAsString(Src)
//                    .Replace("\"", "").Replace("'", "");
//            }

            var path = (node).GetAsString(Src)
                .Replace("\"", "").Replace("'", "");

            if(HttpContext.Current!=null){
                path = HttpContext.Current.Server.MapPath(path);
            }

            if(File.Exists(path))
            {
                var engine = new ExtensibleEngineImpl(File.ReadAllText(path), elementBlock);
                return engine.LessDom.Children;
            }
            return new List<INode>();
        }


        private IEnumerable<INode> Insert(PegNode node, ElementBlock elementBlock)
        {
            node = (node.child_ ?? node);
            var path = (node).GetAsString(Src)
                .Replace("\"", "").Replace("'", "");

            if (HttpContext.Current != null){
                path = HttpContext.Current.Server.MapPath(path);
            }

            if (File.Exists(path)){
                var text = File.ReadAllText(path);
                elementBlock.Add(new Insert(text));
            }
            return new List<INode>();
        }


        /// <summary>
        /// declaration:  standard_declaration / catchall_declaration ;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="elementBlock"></param>
        private void Declaration(PegNode node, ElementBlock elementBlock)
        {
            var name = node.GetAsString(Src).Replace(" ", "");
            var nextNode = node.next_;

            if(nextNode == null){
                // TODO: emit warning: empty declaration //
                return;
            }
            if (nextNode.ToEnLess() == EnLess.comment)
                nextNode = nextNode.next_; 

            var values = Expressions(nextNode, elementBlock);
            var property = name.StartsWith("@") ? new Variable(name, values) : new Property(name, values);
            elementBlock.Add(property);
        }

        /// <summary>
        /// expressions: operation_expressions / space_delimited_expressions / [-a-zA-Z0-9_%*/.&=:,#+? \[\]()]+ ;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="elementBlock"></param>
        /// <returns></returns>
        private IEnumerable<INode> Expressions(PegNode node, ElementBlock elementBlock)
        {
            // Expression
            switch (node.id_.ToEnLess())
            {
                case EnLess.operation_expressions:
                    return OperationExpressions(node.child_, elementBlock).ToList();
                case EnLess.space_delimited_expressions:
                    return SpaceDelimitedExpressions(node.child_, elementBlock).ToList();
                default:
                    if (node.child_ == null) //CatchAll
                        return new List<INode>
                                   {
                                       new Anonymous(node.GetAsString(Src))
                                   };
                    return Expressions(node.child_, elementBlock);
            }
        }

        /// <summary>
        /// operation_expressions:  expression (operator expression)+;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="elementBlock"></param>
        /// <returns></returns>
        private IEnumerable<INode> OperationExpressions(PegNode node, ElementBlock elementBlock)
        {
            yield return Expression(node.child_, elementBlock);
            node = node.next_;            
            
            //Tail
            while (node != null)
            {
                switch (node.id_.ToEnLess())
                {
                    case EnLess.@operator:
                        yield return new Operator(node.GetAsString(Src), elementBlock);
                        break;
                    case EnLess.expression:
                        yield return Expression(node.child_, elementBlock);
                        break;
                    case EnLess.comment:
                        node.ToString();
                        break;
                }
                node = node.next_;
            }
        }

        /// <summary>
        /// space_delimited_expressions: expression (WS expression)* important? ;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="elementBlock"></param>
        /// <returns></returns>
        private IEnumerable<INode> SpaceDelimitedExpressions(PegNode node, ElementBlock elementBlock)
        {
            yield return Expression(node.child_, elementBlock);
            node = node.next_;

            //Tail
            while (node != null)
            {
                switch (node.id_.ToEnLess())
                {
                    case EnLess.expression:
                        yield return Expression(node.child_, elementBlock);
                        break;
                    case EnLess.important:
                        yield return new Keyword("!important");
                        break;
                }
                node = node.next_;
            }
        }

        /// <summary>
        /// expression: '(' s expressions s ')' / entity ;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="elementBlock"></param>
        /// <returns></returns>
        private INode Expression(PegNode node, ElementBlock elementBlock)
        {
            switch (node.id_.ToEnLess())
            {
                case EnLess.expressions:
                    return new Expression(Expressions(node, elementBlock), elementBlock);
                case EnLess.entity:
                    var entity  = Entity(node.child_, elementBlock);
                    entity.Parent = elementBlock;
                    return entity;
                default:
                    throw new ParsingException("Expression should either be child expressions or an entity");
            }
        }

        /// <summary>
        /// entity :  function / fonts / accessor / keyword  / variable / literal  ;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="elementBlock"></param>
        /// <returns></returns>
        private INode Entity(PegNode node, ElementBlock elementBlock)
        {
            switch (node.id_.ToEnLess())
            {
                case EnLess.literal:
                    return Entity(node.child_, elementBlock);
                case EnLess.number:
                    return Number(node);
                case EnLess.color:
                    return Color(node);
                case EnLess.variable:
                    return Variable(node);
                case EnLess.accessor:
                    return Accessor(node.child_, elementBlock);
                case EnLess.fonts:
                    return Fonts(node);
                case EnLess.keyword:
                    return Keyword(node);
                case EnLess.function:
                    return Function(node, elementBlock);
                case EnLess.cursors:
                    return Cursors(node);
                default:
                    return new Anonymous(node.GetAsString(Src));
            }
        }



        /// <summary>
        /// accessor: accessor_name '[' accessor_key ']'; 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="elementBlock"></param>
        /// <returns></returns>
        private INode Accessor(PegNode node, ElementBlock elementBlock)
        {
            var ident = node.GetAsString(Src);
            var key = node.next_.GetAsString(Src).Replace("'", "");
            var el = elementBlock.NearestAs<ElementBlock>(ident);
            if (el != null)
            {
                var prop = el.GetAs<Property>(key);
                if (prop != null) return prop.Value;
            }
            return new Anonymous("");
        }

        /// <summary>
        /// function: function_name arguments ;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private INode Function(PegNode node, ElementBlock element)
        {
            var funcName = node.child_.GetAsString(Src);
            var arguments = Arguments(node, element);
            return new Function(funcName, arguments.ToList());
        }

        /// <summary>
        /// arguments : '(' s argument s (',' s argument s)* ')';
        /// argument : color / number unit / string / [a-zA-Z]+ '=' dimension / function / expressions / [-a-zA-Z0-9_%$/.&=:;#+?]+ / keyword (S keyword)*;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private IEnumerable<INode> Arguments(PegNode node, ElementBlock element)
        {
            foreach (var argument in node.AsEnumerable().Skip(1))
            {
                if (argument.child_ == null)
                    yield return new Anonymous(argument.GetAsString(Src));
                else
                {
                    switch (argument.child_.id_.ToEnLess())
                    {
                        case EnLess.color:
                            yield return Color(argument.child_);
                            break;
                        case EnLess.number:
                            yield return Number(argument.child_);
                            break;
                        case EnLess.function:
                            yield return Function(argument.child_, element);
                            break;
                        case EnLess.expressions:
                            yield return Expression(argument.child_, element);
                            break;
                        case EnLess.@string:
                            yield return new String(argument.GetAsString(Src));
                            break;
                        case EnLess.keyword:
                            yield return new Keyword(argument.GetAsString(Src));
                            break;
                        default:
                            yield return new Anonymous(argument.GetAsString(Src));
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// keyword: [-a-zA-Z]+ !ns;
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private INode Keyword(PegNode node)
        {
            return new Keyword(node.GetAsString(Src));
        }

        /// <summary>
        /// fonts : font (s ',' s font)+  ;
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private INode Fonts(PegNode node)
        {
            var fonts = from childNode in node.AsEnumerable()
                        select (childNode.child_ ?? childNode).GetAsString(Src);
            return new FontFamily(fonts.ToArray());
        }

        /// <summary>
        /// cursor (s ',' s cursor)+  ;
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private INode Cursors(PegNode node)
        {
            var set = from childNode in node.AsEnumerable()
                        select (childNode.child_ ?? childNode).GetAsString(Src);
            return new CursorSet(set.ToArray());
        }


        /// <summary>
        /// number: '-'? [0-9]* '.' [0-9]+ / '-'? [0-9]+;
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private INode Number(PegNode node)
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
        /// <returns></returns>
        private INode Color(PegNode node)
        {
            return RGB(node.child_);
        }

        /// <summary>
        /// rgb:(rgb_node)(rgb_node)(rgb_node) / hex hex hex ;
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private INode RGB(PegNode node)
        {
            int r = 0, g = 0, b = 0;
            string tmp;

            var rgbNode = node.child_; //Fisrt node;
            if (rgbNode != null)
            {
                tmp = rgbNode.GetAsString(Src);
                r = int.Parse(tmp.Length==1 ? tmp+tmp : tmp, NumberStyles.HexNumber);
                rgbNode = rgbNode.next_;
                if (rgbNode != null)
                {
                    tmp = rgbNode.GetAsString(Src);
                    g = int.Parse(tmp.Length == 1 ? tmp + tmp : tmp, NumberStyles.HexNumber);
                    rgbNode = rgbNode.next_;
                    if (rgbNode != null)
                    {
                        tmp = rgbNode.GetAsString(Src);
                        b = int.Parse(tmp.Length == 1 ? tmp + tmp : tmp, NumberStyles.HexNumber);
                    }
                }
            }
            return new Color(r, g, b);
        }

        /// <summary>
        /// variable: '@' [-_a-zA-Z0-9]+; 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private INode Variable(PegNode node)
        {
            return new Variable(node.GetAsString(Src));
        }

        /// <summary>
        /// ruleset: selectors [{] ws prsimary ws [}] ws /  ws selectors ';' ws;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="elementBlock"></param>
        private void RuleSet(PegNode node, ElementBlock elementBlock)
        {
            foreach (var el in Selectors(node.child_, els => StandardSelectors(elementBlock, els)))
                Primary(node.child_.next_, el);
        }
        /// <summary>
        /// TODO: Added quick fix for multipule mixins, but need to add mixins with variables which will changes things a bit
        /// </summary>
        /// <param name="node"></param>
        /// <param name="elementBlock"></param>
//        private void OldMixin(PegNode node, Element element)
//        {
//            var root = element.GetRoot();
//            foreach (var el in Selectors(node.child_, els => els))
//                root = root.Descend(el.Selector, el);
//            if (root.Children != null) element.Children.AddRange(root.Children);
//        }
        private void Mixin(PegNode node, ElementBlock elementBlock)
        {
            var root = elementBlock.GetRoot();
            var rules = new List<INode>();
            foreach (var mixins in node.AsEnumerable())
            {
                var selectors = Selectors(mixins, els => els);
                if (selectors.Count() > 1)
                {
                    foreach (var el in selectors)
                        root = root.Descend(el.Selector, el);
                    if (root.Children != null) elementBlock.Children.AddRange(root.Children); 
                }
                else
                {
                    var el = selectors.First();
                    foreach (var mixinElement in root.Nearests(el.Name))
                    {
                        if (mixinElement.Children != null) rules.AddRange(mixinElement.Children);
                    }
                }
                
            }

            element.Rules.AddRange(rules);
        }

        /// <summary>
        /// standard_ruleset: ws selectors [{] ws primary ws [}] ws;
        /// </summary>
        /// <param name="elementBlock"></param>
        /// <param name="els"></param>
        /// <returns></returns>
        private static IEnumerable<ElementBlock> StandardSelectors(ElementBlock elementBlock, IEnumerable<ElementBlock> els)
        {
            foreach (var el in els)
            {
                elementBlock.Add(el);
                elementBlock = elementBlock.Last;
            }
            yield return elementBlock;
        }

        /// <summary>
        /// selectors :  ws selector (s ',' ws selector)* ws ;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private IEnumerable<ElementBlock> Selectors(PegNode node, Func<IEnumerable<ElementBlock>, IEnumerable<ElementBlock>> action)
        {
            foreach(var selector in node.AsEnumerable(x => x.id_.ToEnLess() == EnLess.selector))
            {
                var selectors = Selector(selector);
                foreach(var s in action(selectors)) yield return s;
            }
        }

        /// <summary>
        /// selector : (s select element s)+ arguments? ;
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private IEnumerable<ElementBlock> Selector(PegNode node)
        {
            var enumerator = node.AsEnumerable().GetEnumerator();
            while(enumerator.MoveNext())
            {
                var selector = enumerator.Current.GetAsString(Src).Trim();
                enumerator.MoveNext();
                var name = enumerator.Current.GetAsString(Src);
                yield return new ElementBlock(name, selector);
            }
        }
    }
}