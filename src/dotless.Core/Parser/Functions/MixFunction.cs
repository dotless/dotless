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

namespace dotless.Core.Parser.Functions
{
    using System.Linq;
    using Infrastructure.Nodes;
    using Tree;
    using Utils;

    public class MixFunction : Function
  {
    protected override Node Evaluate()
    {
      Guard.ExpectMinArguments(2, Arguments.Count, this);
      Guard.ExpectMaxArguments(3, Arguments.Count, this);
      Guard.ExpectAllNodes<Color>(Arguments.Take(2), this);

      double weight = 50;
      if (Arguments.Count == 3)
      {
        Guard.ExpectNode<Number>(Arguments[2], this);

        weight = ((Number)Arguments[2]).Value;
      }


      var colors = Arguments.Take(2).Cast<Color>().ToArray();

      // Note: this algorithm taken from http://github.com/nex3/haml/blob/0e249c844f66bd0872ed68d99de22b774794e967/lib/sass/script/functions.rb

      var p = weight / 100.0;
      var w = p * 2 - 1;
      var a = colors[0].Alpha - colors[1].Alpha;

      var w1 = (((w * a == -1) ? w : (w + a) / (1 + w * a)) + 1) / 2.0;
      var w2 = 1 - w1;

      var rgb = colors[0].RGB.Select((x, i) => x * w1 + colors[1].RGB[i] * w2).ToArray();

      var alpha = colors[0].Alpha * p + colors[1].Alpha * (1 - p);

      var color = new Color(rgb[0], rgb[1], rgb[2], alpha);
      return color;
    }
  }
}