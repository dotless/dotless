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

namespace dotless.Core.engine.Pipeline
{
    using System.Collections.Generic;
    using System.Linq;
    using CssNodes;

    public class CssPropertyGrouper : ICssDomPreprocessor
    {
        public CssDocument Process(CssDocument document)
        {
            document.Elements = MergeElements(document.Elements).ToList();
            return document;
        }

        private static IEnumerable<CssElement> MergeElements(IEnumerable<CssElement> elements)
        {
            return from e in elements
                   group e by e.Identifiers
                   into g
                       let properties = g.SelectMany(a => a.Properties)
                       select
                       new CssElement
                           {
                               Identifiers = g.Key,
                               Properties = new HashSet<CssProperty>(properties),
                               // Note: There is no test requiring the InsertContent to be set here. Please create a failing test and uncomment.
                               //       Also shouldn't we need to join all insert contents together? (assuming it's required at all)
                               // InsertContent = g.First().InsertContent
                           };
        }
    }
}
