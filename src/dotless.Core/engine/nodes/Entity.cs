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

namespace dotless.Core.engine
{
    using System.Collections.Generic;
    using System.Linq;

    public class Entity : INode
    {
        public string Value { get; set; }
        public INode Parent { get; set; }

        protected Entity(){
        }

        public Entity(string value)
            : this(value, null){
            }

        public Entity(string value, INode parent){
            Value = value;
            Parent = parent;
        }

        public INode Last
        {
            get { return Path(null).Last(); }
        }

        public virtual string ToCss()
        {
            return Value;
        }

        public virtual string ToCSharp()
        {
            return Value;
        }

        /// <summary>
        /// Returns the path from any given node, to the root
        /// </summary>
        /// <param name="node"></param>
        /// <returns>ex: ['color', 'p', '#header', 'body', '*']</returns>
        public IList<INode> Path(INode node)
        {
            var path = new List<INode>();
            if (node == null) node = this;
            while (node != null)
            {
                path.Add(node);
                node = node.Parent;
            }
            return path;
        }
        public IList<INode> Path(){ return Path(this); }
        public virtual string Inspect()
        {
            return Value;
        }
    }
}