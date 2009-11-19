using System.Collections.Generic;
using System.Linq;
using dotless.Core.engine.CssNodes;

namespace dotless.Core.engine
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