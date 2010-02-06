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
using System.Linq;

namespace dotless.Core.engine.Functions
{
    public class AddFunction : FunctionBase
    {
        public override INode Evaluate()
        {
            if(!Arguments.All(x => x is Number))
                throw new InvalidOperationException();
            var args = Arguments.Cast<Number>();
            var result = new Number(0);
            foreach(var arg in args)
                result += arg;
            return result;
        }
    }
}
