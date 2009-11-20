﻿/* Copyright 2009 dotless project, http://www.dotlesscss.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.sz\
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *     
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License. */

namespace dotless.Core.engine
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using exceptions;
    using utils;

    public class Element : INode, INearestResolver
    {

        public INode Parent { get; set; }
        public List<INode> Rules { get; set; }
        public List<Element> Set { get; set; }
        public string Name { get; set; }
        public Selector Selector { get; set; }

        public Element(string name, string selector)
        {
            Rules = new List<INode>();
            Set = new List<Element>(); 
            Name = name;
            Selector = selector!=null ? Selector.Get(selector) : Selector.Get("");
        }

        public Element(string name) : this(name, null)
        {
        }

        public Element() : this(string.Empty)
        {
        }
       
        public Element Last
        {
            get
            {
                return Elements.LastOrDefault();
            }
        }
        public Element First
        {
            get
            {
                return Elements.FirstOrDefault();
            }
        }
        public bool IsTag
        {
            get { return !IsId || !IsClass || !IsUniversal; }
        }

        public void Add(INode token)
        {
            token.Parent = this;
            Rules.Add(token);
        }

        public bool IsClass
        {
            get { return Name.StartsWith("."); }
        }

        public bool IsId
        {
            get { return Name.StartsWith("#"); }
        }

        public bool IsUniversal
        {
            get { return Name == "*"; }
        }

        public bool IsRoot
        {
            get { return Parent == null; }
        }

        public bool IsEmpty
        {
            get { return Rules.Count() == 0; }
        }

        public bool IsLeaf
        {
            get { return Elements.Count() == 0; }
        }

        public IList<Property> Identifiers
        {
            get
            {
                return Rules.Where(r => r is Property)
                    .Select(r => (Property) r).ToList();
            }
        }

        public IList<Property> Properties
        {
            get
            {
                return Rules.Where(r => r.GetType() == typeof(Property))
                    .Select(r => (Property) r).ToList();
            }
        }

        public IList<Variable> Variables
        {
            get
            {
                return Rules.Where(r => r.GetType() == typeof(Variable))
                    .Select(r => (Variable) r).ToList();
            }
        }

        public IList<Element> Elements
        {
            get
            {
                return Rules.Where(r => r.GetType() == typeof(Element))
                    .Select(r => (Element) r).ToList();
            }
        }

        /// <summary>
        /// Gets the Trees Root Element
        /// </summary>
        /// <returns></returns>
        public Element GetRoot()
        {
            var els = this;
            while (!els.IsRoot)
                els = (Element)els.Parent;
            return els;
        }

        private Element GetLeaf()
        {
            var els = this;
            while (!els.IsLeaf)
                els = els.First;
            return els;
        }

        /// <summary>
        /// Gets a specific node based upon its ToString value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public INode Get(string key){
            var rules = All().SelectMany(e => e.Rules);
            foreach(var rule in rules){
                if (rule.ToString() == key)
                    return rule;
            }
            return null;
        }
        public T GetAs<T>(string key) { return (T)Get(key); }

        /// <summary>
        /// Gets a specific node based upon its Equality value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public INode Get(INode key){
            return Rules.Where(r => r.Equals(key)).FirstOrDefault();
        }
        public T GetAs<T>(INode key) { return (T)Get(key); }

        public override string ToString()
        {
            return IsRoot ? "*" : Name;
        }

        public virtual string ToCSharp()
        {
            return "";
        }

        /// <summary>
        /// Path from node up the tree i.e. node-->parent-->parent-->parent-->root
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public IList<INode> Path(INode node)
        {
            var path = new List<INode>();
            if (node == null) node = this;
            while (node != null){
                path.Add(node);
                node = node.Parent;
            }
            return path;
        }
        public IList<INode> Path()
        {
            return Path(this);
        }

        /// <summary>
        /// All Elements down the tree from current one
        /// </summary>
        /// <returns></returns>
        public IList<Element> All()
        {
            var path = new List<Element>();
            if(!path.Contains(this)) path.Add(this);
            foreach(var element in Elements){
                path.Add(element);
                path.AddRange(element.All());
            }
            return path;
        }

        /// <summary>
        /// Main entry point for the CSS conversion from the Tree
        /// </summary>
        /// <returns></returns>
        public virtual string ToCss()
        {
            return ToCss(new List<string>());
        }

        /// <summary>
        /// Recursive call appending to the last generated CSS
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal string ToCss(ICollection<string> path)
        {
            if (!IsRoot){
                path.Add(Selector.ToCss());
                path.Add(Name);
            }
            var properties = new StringBuilder();
            var singular = Properties.Count < 2;
            foreach (var prop in Properties){
                if (singular)
                    properties.Append(string.Format(" {0} ", prop.ToCss()));
                else
                    properties.Append(string.Format("\r\n  {0}", prop.ToCss()));
            }
            if (!singular) properties.Append("\r\n");

            var setArray = Set.Select(s => s.Name).ToArray();
            var pathContent =  string.Join(string.Empty, path.Where(p => !string.IsNullOrEmpty(p)).ToArray());
            pathContent = pathContent.StartsWith(" ") ? pathContent.Substring(1) : pathContent;
            var setContent = new StringBuilder(pathContent);


            foreach (var setItem in setArray)
                setContent.AppendFormat(", {0}", setItem);

            var propContent = string.Format(" {{{0}}}", properties);
            var ruleset = properties.Length != 0 ? (setContent + propContent + "\r\n") : "";
            return ruleset + GetChildCss(path);
        }

        private string GetChildCss(IEnumerable<string> path)
        {
            var css = new StringBuilder();
            foreach(var element in Elements){
                css.Append(element.ToCss(new List<string>(path)));
            }
            return css.ToString();
        }


        /// <summary>
        /// Group together simmilar elements
        /// </summary>
        /// <returns></returns>
        /// <remarks>TODO: This code was horrible in the Ruby version and its horrible here. Cant even remember how it works</remarks>
        public Element Group()
        {
            var matched = false;
            var stack = new Stack<Element>(Elements.Reverse());
            var result = new List<Element>();

            if(Elements.Count() == 0) return this;

            foreach (var element in Elements){
                var e = stack.First();
                
                if(!matched) result.Add(e);
                matched = true;

                if (stack.Count() > 1){
                    for (var i = 1; i < stack.Count; i++ ){
                        var ee = stack.ToArray()[i];
                        if (ee.IsEquiv(e) && e.Elements.Count == 0){
                            GetAs<Element>(e).Set.Add(ee);
                            stack.Pop();
                        }
                        else{
                            stack.Pop();
                            matched = false;
                            break;
                        }
                    }
                }
            }
            var grpEls = Elements.Where(e => !result.Contains(e)).ToList();
            if (grpEls.Count > 0){
                foreach (var gpEl in grpEls)
                    Rules.Remove(gpEl);
            }
            return this;
        }


        /// <summary>
        /// Compares two elements 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        private bool IsEquiv(Element other)
        {
            var equiv = Rules.Count() == other.Rules.Count();
            var differentToCss =
                Rules.SelectMany(a => other.Rules, (a, b) => new {a, b})
                    .Where(@t => @t.a.ToCss() != @t.b.ToCss())
                    .Select(@t => @t.a);
            return equiv && differentToCss.Count() == 0;
        }


        /// <summary>
        /// Decend through
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        internal Element Descend(Selector selector, Element element)
        {
            Selector s;
            if (selector is Child)
            {
                s = GetAs<Element>(element.Name).Selector;
                if (s is Child || s is Descendant) return GetAs<Element>(element.Name);
            }
            else if(selector is Descendant)
                return GetAs<Element>(element.Name);
            else
            {
                s = GetAs<Element>(element.Name).Selector;
                if (s.GetType() == selector.GetType()) return GetAs<Element>(element.Name);
            }
            return null;
        }

        /// <summary>
        /// Nearest node to this element (used for nested variable identification).
        /// </summary>
        /// <param name="ident"></param>
        /// <returns></returns>
        public INode Nearest(string ident)
        {
            INode node = null; 
            foreach(var el in Path().Where(n => n is Element).Select(n => (Element)n)){
                var ary = ident.IsIdent() ? el.Elements.Select(n => (INode)n).ToList() : el.Variables.Select(n => (INode)n).ToList();
                node = ary.Where(i => i.ToString() == ident).FirstOrDefault();
                if(node!=null) break;
            }
            if (node == null)  throw new VariableNameException(ident);
            return node;
        }
        public IList<Element> Nearests(string ident)
        {
            IList<Element> nodes = null;
            foreach (var el in Path().Where(n => n is Element).Select(n => (Element)n))
            {
                var ary = ident.IsIdent() ? el.Elements.Select(n => (INode)n).ToList() : el.Variables.Select(n => (INode)n).ToList();
                nodes = ary.Where(i => i.ToString() == ident).Select(e=>(Element)e).ToList();
            }
            //if (nodes == null || nodes.Count==0) throw new VariableNameException(ident);
            return nodes;
        }

        public T NearestAs<T>(string ident) { return (T) Nearest(ident); }
    }
}