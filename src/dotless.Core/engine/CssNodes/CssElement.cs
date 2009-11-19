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
using System.Collections.Generic;
using System.Linq;

namespace dotless.Core.engine.CssNodes
{
    public class CssElement 
    {
        //full set of identifiers and selectors i.e. ".test > a:Hover span.trial"
        public string Identifiers { get; set; } 
        public List<CssProperty> Properties { get; set; }

        public CssElement(string identifierWithSelectors)
            : this(identifierWithSelectors, new List<CssProperty>())
        {
        }

        public CssElement(string identifierWithSelectors, List<CssProperty> properties)
        {
            Identifiers = identifierWithSelectors;
            Properties = properties;
        }

        public bool HasProperty(CssProperty property)
        {
            return Properties.FirstOrDefault(p => p.Key == property.Key && p.Value == property.Value) != null;
        }

        public CssElement AddProperty(CssProperty property)
        {
            if(!HasProperty(property))
                Properties.Add(property);
            return this;
        }

        public bool IsEquiv(CssElement element)
        {
            var equiv = element.Properties.Count == Properties.Count;
            if (!equiv) return false;
           
                var differentToCss =
                    Properties.SelectMany(a => element.Properties, (a, b) => new { a, b })
                        .Where(@t => @t.a.Key != @t.b.Key && @t.a.Value != @t.b.Value)
                        .Select(@t => @t.a);
            
            return equiv && differentToCss.Count() == 0;
        }
    }
}