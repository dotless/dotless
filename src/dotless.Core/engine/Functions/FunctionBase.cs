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
    }

    public abstract class ColorFunctionBase : FunctionBase
    {
        public override INode Evaluate()
        {
            if (Arguments.Length < 1)
                throw new exceptions.ParsingException(string.Format("Expected at least 1 color in function '{0}'.", Name));

            var arg = Arguments[0];
            if (!(arg is Color))
                throw new exceptions.ParsingException(string.Format("Expected a color in function '{0}', found {1}.", Name, arg));

            var color = arg as Color;
            var args = Arguments.Skip(1).ToArray();

            if (args.Length == 1)
            {
                var node = args[0];
                if (node is Number)
                {
                    var number = (node as Number);
                    var edit = EditColor(color, number);
                    if (edit != null)
                        return edit;
                }
            }
          
            return Eval(color, args);
        }

        protected abstract INode Eval(Color color, INode[] args);

        protected virtual INode EditColor(Color color, Number number)
        {
            return null;
        }

        protected abstract string Name { get; }
    }

    public abstract class HslColorFunctionBase : ColorFunctionBase
    {
        protected override INode Eval(Color color, INode[] args)
        {
            var hsl = HslColor.FromRgbColor(color);

            if (args.Length == 1)
            {
                var arg = args[0];
                if (arg is Number)
                {
                    var number = (arg as Number);
                    var edit = EditHsl(hsl, number);
                    if (edit != null)
                        return edit;
                }
            }

            return EvalHsl(hsl, args);
        }

        protected abstract INode EvalHsl(HslColor color, INode[] args);

        protected virtual INode EditHsl(HslColor color, Number number)
        {
            return null;
        }
    }

    public abstract class NumberFunctionBase : FunctionBase
    {
        public override INode Evaluate()
        {
            if (Arguments.Length < 1)
                throw new exceptions.ParsingException(string.Format("Expected at least 1 numeric argument in function '{0}'.", Name));

            var arg = Arguments[0];
            if (!(arg is Number))
                throw new exceptions.ParsingException(string.Format("Expected a numeric argument in function '{0}', found {1}.", Name, arg));

            var number = arg as Number;
            var args = Arguments.Skip(1).ToArray();

            return Eval(number, args);
        }

        protected abstract INode Eval(Number number, INode[] args);

        protected abstract string Name { get; }
    }

}