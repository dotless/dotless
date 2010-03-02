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
using dotless.Core.utils;

namespace dotless.Core.engine.Functions
{
    public class RgbaFunction : FunctionBase
    {
        public override INode Evaluate()
        {
            if (Arguments.Length == 2)
            {
                Guard.ExpectNode<Color>(Arguments[0], this);
                Guard.ExpectNode<Number>(Arguments[1], this);

                return AddAlphaToColor((Color) Arguments[0], (Number) Arguments[1]);
            }

            Guard.ExpectArguments(4, Arguments.Length, this);
            Guard.ExpectAllNodes<Number>(Arguments, this);

            return GetFromComponents();
        }

        private INode AddAlphaToColor(Color color, Number number)
        {
            var alpha = GetAlphaValue(number);

            return color.Alpha(alpha);
        }

        private INode GetFromComponents()
        {
            var colorArgs = Arguments
              .Take(3)
              .Cast<Number>()
              .Select(arg => arg.Unit == "%" ? 255 * arg.Value / 100 : arg.Value)
              .ToArray();

            double alpha = GetAlphaValue(Arguments[3] as Number);

            return new Color(colorArgs[0], colorArgs[1], colorArgs[2], alpha);
        }

        private double GetAlphaValue(Number number)
        {
            return number.Unit == "%" ? number.Value / 100 : number.Value;
        }
    }

    public class RgbFunction : RgbaFunction
    {
        public override INode Evaluate()
        {
            Guard.ExpectArguments(3, Arguments.Length, this);
            Guard.ExpectAllNodes<Number>(Arguments, this);

            Arguments = Arguments.Concat(new[] { new Number(1) }).ToArray();

            return base.Evaluate();
        }
    }
}