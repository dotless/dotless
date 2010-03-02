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

using System;

namespace dotless.Core.engine
{
    using System.Collections.Generic;
    public class Property : Entity, INearestResolver, IReferenceableNode
    {
        public INode _eval { get; set; }

        public Property(string key)
            : this(key, new List<INode>(), null)
        {
        }

        public Property(string key, INode value)
            : this(key, value, null)
        {
        }

        public Property(string key, INode value, ElementBlock parent)
            : this(key, new List<INode> {value}, parent)
        {
        }

        public Property(string key, IEnumerable<INode> value)
            : this(key, value, null)
        {
        }

        public Property(string key, IEnumerable<INode> value, ElementBlock parent)
        {
            Key = key;
            foreach (var node in value){
                node.Parent = this;
            }
            Value = new Expression(value, this);
            _eval = null;
            Parent = parent;
        }

        public string Key { get; set; }
        public new Expression Value { get; set; }

        public T ParentAs<T>()
        {
            return (T) Parent;
        }

        private bool Empty
        {
            get { return Value == null; }
        }

        public void Add(INode token)
        {
            token.Parent = this;
            Value.Add(token);
        }

        public void Add(string token)
        {
            var node = new Anonymous(token) {Parent = this};
            Add(node);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Property)) return false;
            return Equals((Property) obj);
        }

        public virtual INode Evaluate()
        {
            _eval = _eval ?? Value.Evaluate();
            return _eval;
        }

        public override string ToCss()
        {
            return string.Format("{0}: {1};", Key, Value.Evaluate().ToCss());
        }

        public override string ToString()
        {
            return string.Format("{0}: {1};", Key, Value);;
        }

        public string Name
        {
            get { return Key; }
        }

        public INode Nearest(string ident)
        {
            return ParentAs<ElementBlock>().Nearest(ident);
        }

        public T NearestAs<T>(string ident)
        {
            return (T)Nearest(ident);
        }

        public bool Equals(Property other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Key, Key);
        }

        public override int GetHashCode()
        {
            return (Key != null ? Key.GetHashCode() : 0);
        }

        public override INode AdoptClone(INode newParent)
        {
            var clone = (Property) base.AdoptClone(newParent);

            clone.Value = (Expression) Value.AdoptClone(clone);

            return clone;
        }
    }
}