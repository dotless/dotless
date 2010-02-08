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
    using System.Collections;
    using System.Collections.Generic;

    public class Variable : Property, IEvaluatable
    {
        protected bool Declaration;

        public Variable(string key)
            : this(key, new List<INode>(), null)
        {
        }
        
        public Variable(string key, INode value)
            : this(key, value, null)
        {
        }
        public Variable(string key, INode value, ElementBlock parent)
            :this(key, new List<INode>{value}, parent)
        {
        }
        public Variable(string key, IEnumerable<INode> value)
            : this(key, value, null)
        {

        }
        public Variable(string key, IEnumerable<INode> value, ElementBlock parent)
            : base(key, value, parent)
        {
            Declaration = (value==null || ((IList)value).Count == 0)? false : true;
            Key = key.Replace("@", "");
        }
        public override string  ToString()
        {
            return "@" + Key;
        }

        /// <summary>
        /// Evaluates the variables value i.e. @color: #fff +1;
        /// </summary>
        /// <returns></returns>
        /// <remarks>Only evaluates first time, next time will just return last evaluation</remarks>
        public override INode Evaluate()
        {
            if(Declaration)
                _eval = _eval ?? Value.Evaluate();
            else
                _eval = _eval ?? (ParentAs<INearestResolver>()
                                     .NearestAs<IEvaluatable>(ToString()))
                                     .Evaluate();
            return _eval;
        }
        public override string  ToCss()
        {
            return Evaluate()==null ? "" : Evaluate().ToCss(); ;
        } 
         
    }
}