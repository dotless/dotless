using System;
using System.Collections.Generic;
using System.Linq;
using dotless.Core.engine.CssNodes;
using dotless.Core.utils;

namespace dotless.Core.engine.Pipeline
{
    public class CssPropertyMerger : ICssDomPreprocessor
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
