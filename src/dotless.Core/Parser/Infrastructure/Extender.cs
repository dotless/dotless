using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading;
using dotless.Core.Parser.Tree;
using dotless.Core.Utils;

namespace dotless.Core.Parser.Infrastructure
{
    public class ExactExtender : Extender
    {
        [Obsolete("Use the overload that accepts the Extend node")]
        public ExactExtender(Selector baseSelector) : this(baseSelector, null)
        {

        }
        public ExactExtender(Selector baseSelector, Extend extend) : base(baseSelector, extend)
        {

        }
    }

    public class PartialExtender : Extender
    {
        [Obsolete("Use the overload that accepts the Extend node")]
        public PartialExtender(Selector baseSelector) : this(baseSelector, null)
        {

        }
        public PartialExtender(Selector baseSelector, Extend extend) : base(baseSelector, extend)
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
    internal static class ExtenderMatcherExtensions {
        internal static IEnumerable<PartialExtender> WhereExtenderMatches(this IEnumerable<PartialExtender> extenders, Context selection) {
            var selectionElements = selection.SelectMany(selectors => selectors.SelectMany(s => s.Elements)).ToList();

            return extenders.Where(e => e.ElementListMatches(selectionElements));
        }

        /// <summary>
        /// Tests whether or not this extender matches the selector elements in <paramref name="list"/>
        /// by checking if the elements in <see cref="Extender.BaseSelector"/> are a subsequence of the ones in 
        /// <paramref name="list"/>.
        /// 
        /// The special case in the comparison is that if the subsequence element we are comparing is the last one in its 
        /// sequence, we don't compare combinators.
        /// 
        /// A practical example of why we do that is an extendee with the selector:
        /// 
        /// .test .a
        /// 
        /// and an extender with the selector
        /// 
        /// .test
        /// 
        /// The extender should match, even though the .test element in the extendee will have the descendant combinator " "
        /// and the .test element in the extender won't.
        /// </summary>
        private static bool ElementListMatches(this PartialExtender extender, IList<Element> list) {
            int count = extender.BaseSelector.Elements.Count;

            return extender.BaseSelector.Elements.IsSubsequenceOf(list, (subIndex, subElement, index, seqelement) => {
                if (subIndex < count - 1) {
                    return Equals(subElement.Combinator, seqelement.Combinator) &&
                           string.Equals(subElement.Value, seqelement.Value) &&
                           Equals(subElement.NodeValue, seqelement.NodeValue);
                }

                return string.Equals(subElement.Value, seqelement.Value) &&
                       Equals(subElement.NodeValue, seqelement.NodeValue);
            });
        }
    }


    public class Extender
    {
        public Selector BaseSelector { get; private set; }
        public List<Selector> ExtendedBy { get; private set; }
        public bool IsReference { get; set; }
        public bool IsMatched { get; set; }
        public Extend Extend { get; private set; }

        [Obsolete("Use the overload that accepts the Extend node")]
        public Extender(Selector baseSelector)
        {
            BaseSelector = baseSelector;
            ExtendedBy = new List<Selector>();
            IsReference = baseSelector.IsReference;
        }

        public Extender(Selector baseSelector, Extend extend) : this(baseSelector)
        {
            Extend = extend;
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
            return new Selector(new[] {new Element(null, context.ToCss(env)) }) { IsReference = IsReference };
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
