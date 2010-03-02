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

using System.Collections.Generic;
using dotless.Core.exceptions;

namespace dotless.Core.engine.LessNodes
{
    public class IfBlock : NodeBlock
    {
        public BoolExpression Expression { get; private set; }
        public IfBlock(BoolExpression expression)
        {
            Expression = expression;
        }
    }

    public class BoolExpression : Expression
    {
        public BoolExpression(IEnumerable<INode> arr) : base(arr)
        {
        }

        public BoolExpression(IEnumerable<INode> arr, INode parent) : base(arr, parent)
        {
        }

        public new Bool Evaluate()
        {
            var value = base.Evaluate();
            if (value.GetType() != typeof(Bool))
                throw new ParsingException("Bool expressions must evauate to true or false");
            return (Bool) value;
        }
    }
}
