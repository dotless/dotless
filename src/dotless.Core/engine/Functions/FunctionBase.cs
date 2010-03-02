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
    public abstract class FunctionBase
    {
        protected INode[] Arguments { get; set; }

        public abstract INode Evaluate();

        public void SetArguments(INode[] arguments)
        {
            Arguments = arguments;
        }

        public override string ToString()
        {
            return string.Format("function '{0}'", Name);
        }

        public string Name { get; set; }
    }

    public abstract class ColorFunctionBase : FunctionBase
    {
        public override INode Evaluate()
        {
            Guard.ExpectMinArguments(1, Arguments.Length, this);
            Guard.ExpectNode<Color>(Arguments[0], this);

            var color = Arguments[0] as Color;

            if (Arguments.Length == 2)
            {
                Guard.ExpectNode<Number>(Arguments[1], this);

                var number = Arguments[1] as Number;
                var edit = EditColor(color, number);

                if (edit != null)
                    return edit;
            }
          
            return Eval(color);
        }

        protected abstract INode Eval(Color color);

        protected virtual INode EditColor(Color color, Number number)
        {
            return null;
        }
    }

    public abstract class HslColorFunctionBase : ColorFunctionBase
    {
        protected override INode Eval(Color color)
        {
            var hsl = HslColor.FromRgbColor(color);

            return EvalHsl(hsl);
        }

        protected override INode EditColor(Color color, Number number)
        {
            var hsl = HslColor.FromRgbColor(color);

            return EditHsl(hsl, number);
        }

        protected abstract INode EvalHsl(HslColor color);

        protected abstract INode EditHsl(HslColor color, Number number);
    }

    public abstract class NumberFunctionBase : FunctionBase
    {
        public override INode Evaluate()
        {
            Guard.ExpectMinArguments(1, Arguments.Length, this);
            Guard.ExpectNode<Number>(Arguments[0], this);

            var number = Arguments[0] as Number;
            var args = Arguments.Skip(1).ToArray();

            return Eval(number, args);
        }

        protected abstract INode Eval(Number number, INode[] args);
    }

}