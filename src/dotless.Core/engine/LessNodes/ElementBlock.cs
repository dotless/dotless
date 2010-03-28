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
    using exceptions;
    using utils;
    using LessNodes;

    public class ElementBlock : NodeBlock, INearestResolver, IReferenceableNode
    {
        public string Name { get; set; }
        public Selector Selector { get; set; }

        public ElementBlock(string name, string selector)
        {
            Name = name;
            Selector = selector!=null ? Selector.Get(selector) : Selector.Get("");
        }

        public ElementBlock(string name) : this(name, null)
        {
        }




        /// <summary>
        /// Gets the Trees Root Element
        /// </summary>
        /// <returns></returns>
        public ElementBlock GetRoot()
        {
            var els = this;
            while (!els.IsRoot())
                els = (ElementBlock)els.Parent;
            return els;
        }

        /// <summary>
        /// Gets a specific node based upon its Name value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private INode Get(string key){
            var rules = All()
                .SelectMany(e => e.Children)
                .Where( e => e is IReferenceableNode )
                .Cast<IReferenceableNode>();
            
            foreach(var rule in rules)
            {
                if (rule.Name == key)
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
        private INode Get(INode key){
            return Children.Where(r => r.Equals(key)).FirstOrDefault();
        }
        public T GetAs<T>(INode key) { return (T)Get(key); }

        /// <summary>
        /// All Elements down the tree from current one
        /// </summary>
        /// <returns></returns>
        public IList<ElementBlock> All()
        {
            var path = new List<ElementBlock>();
            if(!path.Contains(this)) path.Add(this);
            foreach(var element in Elements){
                path.Add(element);
                path.AddRange(element.All());
            }
            return path;
        }

        /// <summary>
        /// Decend through
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="elementBlock"></param>
        /// <returns></returns>
        internal ElementBlock Descend(Selector selector, ElementBlock elementBlock)
        {
            Selector s;
            if (selector is Child)
            {
                s = GetAs<ElementBlock>(elementBlock.Name).Selector;
                if (s is Child || s is Descendant) return GetAs<ElementBlock>(elementBlock.Name);
            }
            else if(selector is Descendant)
                return GetAs<ElementBlock>(elementBlock.Name);
            else
            {
                s = GetAs<ElementBlock>(elementBlock.Name).Selector;
                if (s.GetType() == selector.GetType()) return GetAs<ElementBlock>(elementBlock.Name);
            }
            return null;
        }


        public override string ToString()
        {
            return IsRoot() ? "*" : Name;
        }

        public bool IsTag
        {
            get { return !IsId || !IsClass || !IsUniversal; }
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

        /// <summary>
        /// Nearest node to this element (used for nested variable identification).
        /// </summary>
        /// <param name="ident"></param>
        /// <returns></returns>
        public INode Nearest(string ident)
        {
            INode node = null;
            foreach (var el in Path().Where(n => n is ElementBlock).Cast<ElementBlock>())
            {   
                node = GetNodesByIdent(ident, el).FirstOrDefault();
                if (node != null) break;
            }
            if (node == null) 
                throw new VariableNameException(ident);
            return node;
        }

        public IList<ElementBlock> Nearests(string ident)
        {
            IList<ElementBlock> nodes = null;
            foreach (var el in Path().Where(n => n is ElementBlock).Cast<ElementBlock>())
            {
                nodes = GetNodesByIdent(ident, el).Cast<ElementBlock>().ToList();
            }
            if (nodes == null || nodes.Count == 0) throw new VariableNameException(ident);
            return nodes;
        }
        public T NearestAs<T>(string ident) { return (T)Nearest(ident); } 
        
        private IEnumerable<INode> GetNodesByIdent(string ident, ElementBlock el)
        {
            IEnumerable<IReferenceableNode> ary;
            if (ident.IsVariable())
                ary = el.Variables.Cast<IReferenceableNode>();
            else
                ary = el.Elements.Cast<IReferenceableNode>();

            return ary.Where(i => i.Name == ident).Cast<INode>();
        }
    }
}