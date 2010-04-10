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
using dotless.Exceptions;
using dotless.Infrastructure;
using dotless.Tree;

namespace dotless.Functions
{
  public class AbsFunction : NumberFunctionBase
  {
    protected override Node Eval(Number number, Node[] args)
    {
      return new Number(Math.Abs(number.Value), number.Unit);
    }
  }

    public class RoundFunction : NumberFunctionBase
    {
        protected override Node Eval(Number number, Node[] args)
        {
            return new Number(Math.Round(number.Value), number.Unit);
        }
    }

    public class FloorFunction : NumberFunctionBase
    {
        protected override Node Eval(Number number, Node[] args)
        {
            return new Number(Math.Floor(number.Value), number.Unit);
        }
    }

    public class CeilFunction : NumberFunctionBase
    {
        protected override Node Eval(Number number, Node[] args)
        {
            return new Number(Math.Ceiling(number.Value), number.Unit);
        }
    }

    public class PercentageFunction : NumberFunctionBase
    {
        protected override Node Eval(Number number, Node[] args)
        {
            if (number.Unit == "%")
                return number;

            if (string.IsNullOrEmpty(number.Unit))
                return new Number(number.Value * 100, "%");

            throw new ParsingException(string.Format("Expected unitless number in function 'percentage', found {0}", number.ToCSS(null)));
        }
    }

    public class IncrementFunction : NumberFunctionBase
    {
        protected override Node Eval(Number number, Node[] args)
        {
            return new Number(number.Value + 1, number.Unit);
        }
    }
}