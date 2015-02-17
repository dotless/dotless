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
            var path = new List<IEnumerable<Selector>> {new [] {selector} };
            path.AddRange(env.Frames.Skip(1).Select(f => f.Selectors.Where(partialSelector => partialSelector != null)));

            path.Reverse();

            ExtendedBy.Add(GenerateExtenderSelector(env, path));
        }

        private Selector GenerateExtenderSelector(Env env, List<IEnumerable<Selector>> selectorStack) {
            var context = GenerateExtenderSelector(new Context(), selectorStack);
            return new Selector(new[] {new Element(null, context.ToCss(env)) });
        }

        private Context GenerateExtenderSelector(Context parentContext, List<IEnumerable<Selector>> selectorStack) {
            if (!selectorStack.Any()) {
                return parentContext;
            }

            var childContext = new Context();
            childContext.AppendSelectors(parentContext, selectorStack.First());
            return GenerateExtenderSelector(childContext, selectorStack.Skip(1).ToList());
        }
    }
}
