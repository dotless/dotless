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
using System.Linq;
using dotless.Core.engine.CssNodes;

namespace dotless.Core.engine.Pipeline
{
    public class LessToCssDomConverter : ILessToCssDomConverter
    {
        private Element _element;
        private CssDocument _cssDocument;

        private void BuildCssDocument()
        {
            _cssDocument = new CssDocument();
            BuildCssDocumentImpl(_element, new List<string>());
        }

        private void BuildCssDocumentImpl(Element element, ICollection<string> path)
        {
            if (!element.IsRoot)
            {
                path.Add(element.Selector.ToCss());
                path.Add(element.Name);

                //Only add an element to the document when we have reached the end of the path
                if(element.Properties.Count !=0 )
                {
                    var cssProperties = new List<CssProperty>();
                    foreach (var properties in element.Properties)
                        cssProperties.Add(new CssProperty(properties.Key, properties.Value.Evaluate().ToCss()));

                    //Get path content i.e. "p > a:Hover"
                    var pathContent = string.Join(string.Empty, path.Where(p => !string.IsNullOrEmpty(p)).ToArray());
                    pathContent = pathContent.StartsWith(" ") ? pathContent.Substring(1) : pathContent;
                    _cssDocument.Elements.Add(new CssElement(pathContent, cssProperties));
                }
            }
            //Keep going
            foreach (var nextElement in element.Elements)
            {
                BuildCssDocumentImpl(nextElement, new List<string>(path));
            }
        }


        public CssDocument BuildCssDocument(Element LessRootElement)
        {
            _element = LessRootElement;
            BuildCssDocument();
            return _cssDocument;
        }
    }
}