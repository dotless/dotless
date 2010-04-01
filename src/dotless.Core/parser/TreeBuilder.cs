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

    internal class TreeBuilder
    {
        public LessPegRootNode Root { get; set; }
        public string Src { get; set; }

        public TreeBuilder(LessPegRootNode root, string src)
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
            return Primary(Root.Child);
        }

        /// <summary>
        /// Main entry point for the build
        /// </summary>
        /// <returns></returns>
        public ElementBlock Build(ElementBlock tail)
        {
            return tail == null ? Build() : Primary(Root.Child, tail);
        }

        private ElementBlock Primary(LessPegNode node)
        {
            var element = new ElementBlock("");
            return Primary(node, element);
        }

        /// <summary>
        /// primary: (import / declaration / ruleset / comment)* ;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="elementBlock"></param>
        private ElementBlock Primary(LessPegNode node, ElementBlock elementBlock)
        {
            foreach (var nextPrimary in node.Children())
            {
                switch (nextPrimary.Type())
                {
                    case EnLess.import:
                        Import(nextPrimary.Child, elementBlock);
                        //element.Children.AddRange(import);
                        break;
                    case EnLess.insert:
                        Insert(nextPrimary.Child, elementBlock);
                        //element.Children.AddRange(import);
                        break;
                    case EnLess.standard_ruleset:
                        RuleSet(nextPrimary, elementBlock);
                        break;
                    case EnLess.mixin_ruleset:
                        Mixin(nextPrimary,elementBlock);
                        break;
                    case EnLess.declaration:
                        Declaration(nextPrimary.Child, elementBlock);
                        break;
                }
            }
            return elementBlock;
        }

        /// <summary>
        /// import :  ws '@import'  S import_url medias? s ';' ;
        /// </summary>
        /// <param name="node"></param>
        private IEnumerable<INode> Import(LessPegNode node, ElementBlock elementBlock)
        {
            node = (node.Child ?? node);
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


        private IEnumerable<INode> Insert(LessPegNode node, ElementBlock elementBlock)
        {
            node = (node.Child ?? node);
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
        private void Declaration(LessPegNode node, ElementBlock elementBlock)
        {
            var name = node.GetAsString(Src).Replace(" ", "");
            var nextNode = node.Next;

            if(nextNode == null){
                // TODO: emit warning: empty declaration //
                return;
            }
            if (nextNode.Type() == EnLess.comment)
                nextNode = nextNode.Next; 

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
        private IEnumerable<INode> Expressions(LessPegNode node, ElementBlock elementBlock)
        {
            // Expression
            switch (node.Type())
            {
                case EnLess.operation_expressions:
                    return OperationExpressions(node.Child, elementBlock).ToList();
                case EnLess.space_delimited_expressions:
                    return SpaceDelimitedExpressions(node.Child, elementBlock).ToList();
                default:
                    if (node.Child == null) //CatchAll
                        return new List<INode>
                                   {
                                       new Anonymous(node.GetAsString(Src))
                                   };
                    return Expressions(node.Child, elementBlock);
            }
        }

        /// <summary>
        /// operation_expressions:  expression (operator expression)+;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="elementBlock"></param>
        /// <returns></returns>
        private IEnumerable<INode> OperationExpressions(LessPegNode node, ElementBlock elementBlock)
        {
            yield return Expression(node.Child, elementBlock);
            node = node.Next;            
            
            //Tail
            while (node != null)
            {
                switch (node.Type())
                {
                    case EnLess.@operator:
                        yield return new Operator(node.GetAsString(Src), elementBlock);
                        break;
                    case EnLess.expression:
                        yield return Expression(node.Child, elementBlock);
                        break;
                    case EnLess.comment:
                        node.ToString();
                        break;
                }
                node = node.Next;
            }
        }

        /// <summary>
        /// space_delimited_expressions: expression (WS expression)* important? ;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="elementBlock"></param>
        /// <returns></returns>
        private IEnumerable<INode> SpaceDelimitedExpressions(LessPegNode node, ElementBlock elementBlock)
        {
            yield return Expression(node.Child, elementBlock);
            node = node.Next;

            //Tail
            while (node != null)
            {
                switch (node.Type())
                {
                    case EnLess.expression:
                        yield return Expression(node.Child, elementBlock);
                        break;
                    case EnLess.important:
                        yield return new Keyword("!important");
                        break;
                }
                node = node.Next;
            }
        }

        /// <summary>
        /// expression: '(' s expressions s ')' / entity ;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="elementBlock"></param>
        /// <returns></returns>
        private INode Expression(LessPegNode node, ElementBlock elementBlock)
        {
            switch (node.Type())
            {
                case EnLess.expressions:
                    return new Expression(Expressions(node, elementBlock), elementBlock);
                case EnLess.entity:
                    var entity  = Entity(node.Child, elementBlock);
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
        private INode Entity(LessPegNode node, ElementBlock elementBlock)
        {
            switch (node.Type())
            {
                case EnLess.literal:
                    return Entity(node.Child, elementBlock);
                case EnLess.number:
                    return Number(node);
                case EnLess.color:
                    return Color(node);
                case EnLess.variable:
                    return Variable(node);
                case EnLess.accessor:
                    return Accessor(node.Child, elementBlock);
                case EnLess.fonts:
                    return Fonts(node);
                case EnLess.keyword:
                    return Keyword(node);
                case EnLess.function:
                    return Function(node, elementBlock);
                case EnLess.cursors:
                    return Cursors(node);
                case EnLess.@string:
                    return new String(node.GetAsString(Src));
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
        private INode Accessor(LessPegNode node, ElementBlock elementBlock)
        {
            var ident = node.GetAsString(Src);
            var key = node.Next.GetAsString(Src).Replace("'", "");
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
        private INode Function(LessPegNode node, ElementBlock element)
        {
            var funcName = node.Child.GetAsString(Src);
            var arguments = Arguments(node.Child.Next, element);
            return new Function(funcName, arguments.ToList());
        }

        /// <summary>
        /// arguments : '(' s argument s (',' s argument s)* ')';
        /// argument : color / number unit / string / [a-zA-Z]+ '=' dimension / function / expressions / [-a-zA-Z0-9_%$/.&=:;#+?]+ / keyword (S keyword)*;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        private IEnumerable<INode> Arguments(LessPegNode node, ElementBlock element)
        {
            foreach (var argument in node.Children())
            {
                if (argument.Child == null)
                    yield return new Anonymous(argument.GetAsString(Src));
                else
                {
                    switch (argument.Child.Type())
                    {
                        case EnLess.color:
                            yield return Color(argument.Child);
                            break;
                        case EnLess.number:
                            yield return Number(argument.Child);
                            break;
                        case EnLess.function:
                            yield return Function(argument.Child, element);
                            break;
                        case EnLess.expressions:
                            yield return Expression(argument.Child, element);
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
        private INode Keyword(LessPegNode node)
        {
            return new Keyword(node.GetAsString(Src));
        }

        /// <summary>
        /// fonts : font (s ',' s font)+  ;
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private INode Fonts(LessPegNode node)
        {
            var fonts = from childNode in node.Children()
                        select (childNode.Child ?? childNode).GetAsString(Src);
            return new FontFamily(fonts.ToArray());
        }

        /// <summary>
        /// cursor (s ',' s cursor)+  ;
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private INode Cursors(LessPegNode node)
        {
            var set = from childNode in node.Children()
                        select (childNode.Child ?? childNode).GetAsString(Src);
            return new CursorSet(set.ToArray());
        }


        /// <summary>
        /// number: '-'? [0-9]* '.' [0-9]+ / '-'? [0-9]+;
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private INode Number(LessPegNode node)
        {
            var val = float.Parse(node.GetAsString(Src), NumberFormatInfo.InvariantInfo);
            var unit = "";
            node = node.Next;
            if (node != null && node.Type() == EnLess.unit) unit = node.GetAsString(Src);
            return new Number(unit, val);
        }

        /// <summary>
        /// color: '#' rgb;
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private INode Color(LessPegNode node)
        {
            return RGB(node.Child);
        }

        /// <summary>
        /// rgb:(rgb_node)(rgb_node)(rgb_node) / hex hex hex ;
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private INode RGB(LessPegNode node)
        {
            int r = 0, g = 0, b = 0;
            string tmp;

            var rgbNode = node.Child; //Fisrt node;
            if (rgbNode != null)
            {
                tmp = rgbNode.GetAsString(Src);
                r = int.Parse(tmp.Length==1 ? tmp+tmp : tmp, NumberStyles.HexNumber);
                rgbNode = rgbNode.Next;
                if (rgbNode != null)
                {
                    tmp = rgbNode.GetAsString(Src);
                    g = int.Parse(tmp.Length == 1 ? tmp + tmp : tmp, NumberStyles.HexNumber);
                    rgbNode = rgbNode.Next;
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
        private INode Variable(LessPegNode node)
        {
            return new Variable(node.GetAsString(Src));
        }

        /// <summary>
        /// ruleset: selectors [{] ws prsimary ws [}] ws /  ws selectors ';' ws;
        /// </summary>
        /// <param name="node"></param>
        /// <param name="elementBlock"></param>
        private void RuleSet(LessPegNode node, ElementBlock elementBlock)
        {
            foreach (var el in Selectors(node.Child, els => StandardSelectors(elementBlock, els)))
                Primary(node.Child.Next, el);
        }
        /// <summary>
        /// TODO: Added quick fix for multipule mixins, but need to add mixins with variables which will changes things a bit
        /// </summary>
        /// <param name="node"></param>
        /// <param name="elementBlock"></param>
//        private void OldMixin(LessPegNode node, Element element)
//        {
//            var root = element.GetRoot();
//            foreach (var el in Selectors(node.Child, els => els))
//                root = root.Descend(el.Selector, el);
//            if (root.Children != null) element.Children.AddRange(root.Children);
//        }
        private void Mixin(LessPegNode node, ElementBlock elementBlock)
        {
            var root = elementBlock.GetRoot();
            var rules = new List<INode>();
            foreach (var mixins in node.Children())
            {
                var selectors = Selectors(mixins, els => els).ToList();
                if (selectors.Count() > 1)
                {
                    foreach (var el in selectors)
                        root = root.Descend(el.Selector, el);

                    var last = selectors.Last();

                    var children = GetMixinChildren(elementBlock, last, root);

                    if(children != null)
                        rules.AddRange(children);
                }
                else
                {
                    var el = selectors.First();
                    foreach (var mixinElement in root.Nearests(el.Name))
                    {
                        var children = GetMixinChildren(elementBlock, el, mixinElement);

                        if(children != null)
                            rules.AddRange(children);
                    }
                }
                
            }

            elementBlock.Children.AddRange(rules);
        }

        private static IEnumerable<INode> GetMixinChildren(ElementBlock newParentBlock, ElementBlock mixinRule, ElementBlock mixinBlock)
        {

            if (mixinBlock.Children == null)
                return null;

            IEnumerable<INode> children;
            if (mixinBlock is MixinBlock)
            {
                var mixin = ((MixinBlock)mixinBlock);

                List<Variable> arguments = null;
                if (mixinRule is MixinBlock)
                    arguments = (mixinRule as MixinBlock).Arguments;

                children = mixin.GetClonedChildren(newParentBlock, arguments ?? new List<Variable>());
            }
            else
                children = mixinBlock.Children;

            return children;
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
        private IEnumerable<ElementBlock> Selectors(LessPegNode node, Func<IEnumerable<ElementBlock>, IEnumerable<ElementBlock>> action)
        {
            foreach(var selector in node.Children(x => x.Type() == EnLess.selector))
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
        private IEnumerable<ElementBlock> Selector(LessPegNode node)
        {
            var enumerator = node.Children().GetEnumerator();
            while(enumerator.MoveNext())
            {
                ElementBlock block;

                var selector = enumerator.Current.GetAsString(Src).Trim();
                enumerator.MoveNext();
                var name = enumerator.Current.GetAsString(Src);

                var next = enumerator.Current.Next;
                var isMixinWithArgs = next != null && next.Type() == EnLess.arguments;

                if (isMixinWithArgs)
                {
                    var mixinBlock = new MixinBlock(name, selector);
                    block = mixinBlock;

                    var arguments = GetMixinArguments(next, block);
                    enumerator.MoveNext();

                    foreach (var argument in arguments)
                        mixinBlock.Arguments.Add(argument);
                }
                else
                    block = new ElementBlock(name, selector);

                yield return block;
            }
        }


        private IEnumerable<Variable> GetMixinArguments(LessPegNode arguments, ElementBlock block)
        {
            var enumerator = arguments.Children().GetEnumerator();
            var variables = new List<Variable>();
            var position = 0;

            while (enumerator.MoveNext())
            {
                var node = enumerator.Current;

                string name;
                IEnumerable<INode> value;
                if (node.Child.Type() == EnLess.variable) // expect "@variable: expression"
                {
                    name = node.Child.GetAsString(Src);
                    value = Expressions(node.Child.Next, block);
                }
                else  // expect "expresion"
                {
                    // HACK: to make Arguments return as expected.
                    var tmpNode = new LessPegNode(null, (int) EnLess.arguments, Root);
                    tmpNode.Child = new LessPegNode(tmpNode, (int)EnLess.argument, Root);
                    tmpNode.Child.Child = node.Child;

                    name = position.ToString();
                    value = Arguments(tmpNode, block).ToList();
                }

                variables.Add(new Variable(name, value));

                if(node.Next == null || node.Next.Type() != EnLess.argument)
                    break;

                position++;
            }

            return variables;
        }
    }
}