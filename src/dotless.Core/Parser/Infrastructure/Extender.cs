using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using dotless.Core.Parser.Tree;
using dotless.Core.Utils;

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

        public IEnumerable<Selector> Replacements(string selection)
        {
            foreach (var ex in ExtendedBy)
            {
                yield return new Selector(new []{new Element(null, selection.Replace(BaseSelector.ToString().Trim(), ex.ToString().Trim()))});       
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

        public static string FullPathSelector()
        {
            throw new NotImplementedException();
        }

        public void AddExtension(Selector selector, Env env)
        {
            var path = new List<Selector>();
            path.Add(selector);
            foreach (var f in env.Frames.Skip(1))
            {
                var partialSelector = f.Selectors.FirstOrDefault();
                if (partialSelector != null)
                {
                    path.Add(partialSelector);
                }
            }
            ExtendedBy.Add(new Selector(new[] { new Element(null, path.Select(p => p.ToCSS(env)).Reverse().JoinStrings("").Trim()) }));
        }
    }
}
