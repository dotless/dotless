using System;
using System.Collections.Generic;
using System.Linq;
using dotless.Core.engine.CssNodes;

namespace dotless.Core.engine.Pipeline
{
    public class CssPropertyMerger : ICssDomPreprocessor
    {
        public CssDocument Process(CssDocument document)
        {
            document.Elements = MergeElements(document.Elements).ToList();
            return document;
        }

        private IEnumerable<CssElement> MergeElements(List<CssElement> elements)
        {
            while (elements.Count != 0)
            {
                var element = elements[0];

                var allProperties = (from e in elements
                                     from property in e.Properties
                                     where e.Identifiers == element.Identifiers
                                     select property).Reverse().Distinct(new CssPropertyComparer()).Reverse();


                yield return new CssElement(element.Identifiers) { Properties = new HashSet<CssProperty>(allProperties)};

                elements.RemoveAll(e => e.Identifiers == element.Identifiers);
            }
        }

        class CssPropertyComparer : IEqualityComparer<CssProperty>
        {
            public bool Equals(CssProperty x, CssProperty y)
            {
                return x.Key == y.Key;
            }

            public int GetHashCode(CssProperty obj)
            {
                return obj.Key.GetHashCode();
            }
        }
    }
}
