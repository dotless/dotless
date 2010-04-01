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
using dotless.Core.exceptions;
using dotless.Core.utils;

namespace dotless.Core.engine.Functions
{
    public class RedFunction : ColorFunctionBase
    {
        protected override INode Eval(Color color)
        {
            return new Number(color.R);
        }

        protected override INode EditColor(Color color, Number number)
        {
            var value = number.Value;

            if (number.Unit == "%")
                value = (value * 255) / 100d;

            return new Color(color.R + value, color.G, color.B);
        }
    }

    public class GreenFunction : ColorFunctionBase
    {
        protected override INode Eval(Color color)
        {
            return new Number(color.G);
        }

        protected override INode EditColor(Color color, Number number)
        {
            var value = number.Value;

            if (number.Unit == "%")
                value = (value * 255) / 100d;

            return new Color(color.R, color.G + value, color.B);
        }
    }

    public class BlueFunction : ColorFunctionBase
    {
        protected override INode Eval(Color color)
        {
            return new Number(color.B);
        }

        protected override INode EditColor(Color color, Number number)
        {
            var value = number.Value;

            if (number.Unit == "%")
                value = (value * 255) / 100d;

            return new Color(color.R, color.G, color.B + value);
        }
    }

    public class AlphaFunctionImpl : ColorFunctionBase
    {
        protected override INode Eval(Color color)
        {
            return new Number(color.A);
        }

        protected override INode EditColor(Color color, Number number)
        {
            var alpha = number.Value;

            if (number.Unit == "%")
                alpha = alpha / 100d;

            return new Color(color.R, color.G, color.B, color.A + alpha);
        }
    }

    // HACK: avoid Alpha from throwing an error in case of "filter: alpha(Opacity = x);"
    public class AlphaFunction : FunctionBase
    {
        public override INode Evaluate()
        {
            try
            {
                var function = new AlphaFunctionImpl();
                function.SetArguments(Arguments);
                function.Name = Name;

                return function.Evaluate();
            }
            catch(ParsingException)
            {
                if(!Arguments[0].ToCss().ToLowerInvariant().Contains("opacity"))
                    throw;

                var argumentString = Arguments.Select(x => x.ToCss()).JoinStrings(", ");
                return new Literal(string.Format("ALPHA({0})", argumentString));
            }
        }
    }

    public class GrayscaleFunction : ColorFunctionBase
    {
        protected override INode Eval(Color color)
        {
            var grey = (color.RGB.Max() + color.RGB.Min()) / 2;

            return new Color(grey, grey, grey);
        }
    }
}