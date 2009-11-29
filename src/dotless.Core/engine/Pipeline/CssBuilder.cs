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
using System.Text;
using dotless.Core.engine.CssNodes;

namespace dotless.Core.engine.Pipeline
{
    public class CssBuilder : ICssBuilder
    {
        /// <summary>
        /// Takes a CssDocument and returns the CSS output
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public string ToCss(CssDocument document)
        {
            var stringBuilder  = new StringBuilder();
            var rulesets  = GroupElements(new List<CssElement>(document.Elements));

            foreach(var ruleset in rulesets)
            {
                var rulesetString = GetRuleSetString(new List<CssElement>(ruleset));
                var propertyString = GetPropertyString(new List<CssProperty>(ruleset[0].Properties));
                stringBuilder.Append(string.Format("{0} {{{1}}}\r\n", rulesetString, propertyString));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Get ruleset string i.e. "a .hello, d.test"
        /// </summary>
        /// <param name="ruleset"></param>
        /// <returns></returns>
        private static string GetRuleSetString(IList<CssElement> ruleset)
        {
            var setContent = new StringBuilder(ruleset[0].Identifiers);
            ruleset.RemoveAt(0);
            foreach (var rulesetItem in ruleset)
                setContent.AppendFormat(", {0}", rulesetItem.Identifiers);
            return setContent.ToString();
        }

        /// <summary>
        /// Build properties css string
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        private static string GetPropertyString(ICollection<CssProperty> properties)
        {
            var propertyStringBuilder = new StringBuilder();
            var singular = properties.Count < 2;
            foreach (var prop in properties)
            {
                if (singular)
                    propertyStringBuilder.Append(string.Format(" {0}: {1}; ", prop.Key, prop.Value));
                else
                    propertyStringBuilder.Append(string.Format("\r\n  {0}: {1};", prop.Key, prop.Value));
            }
            if (!singular) propertyStringBuilder.Append("\r\n");
            return propertyStringBuilder.ToString();
        }

        private IList<IList<CssElement>> GroupElements(IList<CssElement> elements)
        {
            var groupedElements = new List<IList<CssElement>>();
            while (elements.Count != 0)
            {
                var comparisonElement = elements[0];
                var matchingElements = new List<CssElement> { comparisonElement };
                elements.Remove(comparisonElement);

                foreach (var el in new List<CssElement>(elements))
                {
                    if (!comparisonElement.IsEquiv(el)) continue;
                    matchingElements.Add(el);
                    elements.Remove(el);
                }
                groupedElements.Add(matchingElements);
            }
            return groupedElements;
        }
    }
}