using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotless.Core.Parser.Tree;

namespace dotless.Core.Parser.Infrastructure
{
    public class ExactExtender : Extender
    {
        public ExactExtender(Selector baseSelector):base(baseSelector)
        {

        }
    }

    public class PartialExtender : Extender
    {
        public PartialExtender(Selector baseSelector):base(baseSelector)
        {

        }

        public IEnumerable<Selector> Replacements(Selector selector)
        {
            foreach (var ex in ExtendedBy)
            {
                yield return new Selector(selector.Elements.Where(e => e.Value != null).Select(e => new Element(e.Combinator, e.Value.Replace(BaseSelector.ToString().Trim(), ex.ToString().Trim()))));       
            }
        }
    }

    public class Extender
    {
        public Selector BaseSelector { get; private set; }
        public List<Selector> ExtendedBy { get; private set; }

        public Extender(Selector baseSelector)
        {
            BaseSelector = baseSelector;
            ExtendedBy = new List<Selector>();
        }

        public void AddExtension(Selector selector)
        {
            ExtendedBy.Add(selector);
        }
    }
}
