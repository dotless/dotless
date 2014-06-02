using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotless.Core.Parser.Tree;

namespace dotless.Core.Parser.Infrastructure
{
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
