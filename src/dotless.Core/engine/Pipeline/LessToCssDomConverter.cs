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
using dotless.Core.engine.LessNodes;

namespace dotless.Core.engine.Pipeline
{
    public class LessToCssDomConverter : ILessToCssDomConverter
    {
        private CssDocument document; 
        private void BuildElement(ElementBlock elementBlock, ICollection<string> path)
        {
            if (!elementBlock.IsRoot())
            {
                path.Add(elementBlock.Selector.ToCss());
                path.Add(elementBlock.Name);


                //Only add an element to the document when we have reached the end of the path
                if (elementBlock.Properties.Count != 0)
                {
                    var cssProperties = new List<CssProperty>();

                    foreach (var property in elementBlock.Properties)
                        cssProperties.Add(new CssProperty(property.Key, property.Evaluate().ToCss()));

                    //Get path content i.e. "p > a:Hover"
                    var pathContent = string.Join(string.Empty, path.Where(p => !string.IsNullOrEmpty(p)).ToArray());
                    pathContent = pathContent.StartsWith(" ") ? pathContent.Substring(1) : pathContent;
                    document.Elements.Add(new CssElement(pathContent, cssProperties));
                }
            }
            if (elementBlock.Inserts.Count == 0) return;
            foreach (var insert in elementBlock.Inserts)
                document.Elements.Add(new CssElement { InsertContent = insert.ToString() });
        }

        private void BuildCssDocumentImpl(IBlock node, ICollection<string> path)
        {
            bool processChildNodes = true;
            if (node.GetType() == typeof(ElementBlock))
                BuildElement((ElementBlock) node, path);
            if (node.GetType() == typeof(IfBlock))
                processChildNodes = ((IfBlock) node).Expression.Evaluate().Value;

            if (processChildNodes)
            foreach (var nextNode in node.SubBlocks)
                BuildCssDocumentImpl(nextNode, new List<string>(path));
        }

        public CssDocument BuildCssDocument(ElementBlock lessRootElementBlock)
        {
            document = new CssDocument();
            BuildCssDocumentImpl(lessRootElementBlock, new List<string>());
            return document;
        }
    }

    public class LessToCssDomConverterOld : ILessToCssDomConverter
    {
        private ElementBlock elementBlock;
        private CssDocument _cssDocument;

        private void BuildCssDocument()
        {
            _cssDocument = new CssDocument();
            BuildCssDocumentImpl(elementBlock, new List<string>());
        }

        private void BuildCssDocumentImpl(ElementBlock elementBlock, ICollection<string> path)
        {
            if (!elementBlock.IsRoot())
            {
                path.Add(elementBlock.Selector.ToCss());
                path.Add(elementBlock.Name);


                //Only add an element to the document when we have reached the end of the path
                if(elementBlock.Properties.Count !=0 )
                {
                 var cssProperties = new List<CssProperty>();

                    foreach (var property in elementBlock.Properties)
                        cssProperties.Add(new CssProperty(property.Key, property.Evaluate().ToCss()));

                    //Get path content i.e. "p > a:Hover"
                    var pathContent = string.Join(string.Empty, path.Where(p => !string.IsNullOrEmpty(p)).ToArray());
                    pathContent = pathContent.StartsWith(" ") ? pathContent.Substring(1) : pathContent;
                    _cssDocument.Elements.Add(new CssElement(pathContent, cssProperties));
                }
            }
            if (elementBlock.Inserts.Count != 0){
                foreach (var insert in elementBlock.Inserts)
                    _cssDocument.Elements.Add(new CssElement { InsertContent = insert.ToString()});
            }
            //Keep going
            foreach (var nextElement in elementBlock.Elements)
            {
                BuildCssDocumentImpl(nextElement, new List<string>(path));
            }
        }


        public CssDocument BuildCssDocument(ElementBlock lessRootElementBlock)
        {
            elementBlock = lessRootElementBlock;
            BuildCssDocument();
            return _cssDocument;
        }
    }
}