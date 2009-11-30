using System.Collections.Generic;
using System.Linq;
using dotless.Core.engine.CssNodes;

namespace dotless.Core.engine.Pipeline
{
    public class CssPropertyMerger : ICssDomPreprocessor
    {
        public CssDocument Process(CssDocument document)
        {
            document.Elements = MergeElements(document.Elements);
            return document;
        }

        private List<CssElement> MergeElements(List<CssElement> elements)
        {
            var mergeElements = new List<CssElement>();
            while (elements.Count != 0)
            {
                var element = elements[0];
                var matchingElements = elements
                    .Where(e => e.Identifiers == element.Identifiers).ToList();
                
                var allProperties = matchingElements
                                    .SelectMany(e => e.Properties).ToList();

                mergeElements.Add(new CssElement(element.Identifiers, allProperties));
                foreach (var el in matchingElements)
                    elements.Remove(el);
            }
            return mergeElements;
        }
    }
}
