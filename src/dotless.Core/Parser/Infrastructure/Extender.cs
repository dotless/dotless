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
            var selectorPath = new List<IEnumerable<Selector>> {new [] {selector} };
            selectorPath.AddRange(env.Frames.Skip(1).Select(f => f.Selectors.Where(partialSelector => partialSelector != null)));

            ExtendedBy.Add(GenerateExtenderSelector(env, selectorPath));
        }

        private Selector GenerateExtenderSelector(Env env, List<IEnumerable<Selector>> selectorPath) {
            var context = GenerateExtenderSelector(selectorPath);
            return new Selector(new[] {new Element(null, context.ToCss(env)) });
        }

        private Context GenerateExtenderSelector(List<IEnumerable<Selector>> selectorStack) {
            if (!selectorStack.Any()) {
                return null;
            }

            var parentContext = GenerateExtenderSelector(selectorStack.Skip(1).ToList());

            var childContext = new Context();
            childContext.AppendSelectors(parentContext, selectorStack.First());
            return childContext;
        }
    }
}
