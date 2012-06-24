namespace dotless.Core.Parser.Infrastructure
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Tree;
    using Utils;
    using dotless.Core.Parser.Infrastructure.Nodes;

    public class Context : IEnumerable<IEnumerable<Selector>>
    {
        private List<List<Selector>> Paths { get; set; }

        public Context()
        {
            Paths = new List<List<Selector>>();
        }

        public void AppendSelectors(Context context, IEnumerable<Selector> selectors)
        {
            foreach (var selector in selectors)
            {
                AppendSelector(context, selector);
            }
        }

        private void AppendSelector(Context context, Selector selector)
        {
            if (!selector.Elements.Any(e => e.Value == "&"))
            {
                if (context != null && context.Paths.Count > 0)
                {
                    Paths.AddRange(context.Paths.Select(path => path.Concat(new[] { selector }).ToList()));
                }
                else
                {
                    Paths.Add(new List<Selector>() { selector });
                }
                return;
            }

            // The paths are List<List<Selector>>
            // The first list is a list of comma seperated selectors
            // The inner list is a list of inheritance seperated selectors
            // e.g.
            // .a, .b {
            //   .c {
            //   }
            // }
            // == {{.a} {.c}} {{.b} {.c}}
            //

            // the elements from the current selector so far
            var currentElements = new NodeList<Element>();
            // the current list of new selectors to add to the path.
            // We will build it up. We initiate it with one empty selector as we "multiply" the new selectors
            // by the parents
            var newSelectors = new List<List<Selector>>() { new List<Selector>() };

            foreach (Element el in selector.Elements)
            {
                // non parent reference elements just get added
                if (el.Value != "&")
                {
                    currentElements.Add(el);
                } else
                {
                    // the new list of selectors to add
                    var selectorsMultiplied = new List<List<Selector>>();

                    // merge the current list of non parent selector elements
                    // on to the current list of selectors to add
                    if (currentElements.Count > 0)
                    {
                        MergeElementsOnToSelectors(currentElements, newSelectors);
                    }

                    // loop through our current selectors
                    foreach(List<Selector> sel in newSelectors)
                    {
                        // if we don't have any parent paths, the & might be in a mixin so that it can be used
                        // whether there are parents or not
                        if (context.Paths.Count == 0)
                        {
                            // the combinator used on el should now be applied to the next element instead so that
                            // it is not lost
                            if (sel.Count > 0)
                            {
                                sel[0].Elements = new NodeList<Element>(sel[0].Elements);
                                sel[0].Elements.Add(new Element(el.Combinator,  ""));
                            }
                            selectorsMultiplied.Add(sel);
                        }
                        else
                        {
                            // and the parent selectors
                            foreach (List<Selector> parentSel in context.Paths)
                            {
                                // We need to put the current selectors
                                // then join the last selector's elements on to the parents selectors

                                // our new selector path
                                List<Selector> newSelectorPath = new List<Selector>();
                                // selectors from the parent after the join
                                List<Selector> afterParentJoin = new List<Selector>();
                                Selector newJoinedSelector;
                                bool newJoinedSelectorEmpty = true;

                                //construct the joined selector - if & is the first thing this will be empty,
                                // if not newJoinedSelector will be the last set of elements in the selector
                                if (sel.Count > 0)
                                {
                                    newJoinedSelector = new Selector(new NodeList<Element>(sel[sel.Count - 1].Elements));
                                    newSelectorPath.AddRange(sel.Take(sel.Count - 1));
                                    newJoinedSelectorEmpty = false;
                                }
                                else
                                {
                                    newJoinedSelector = new Selector(new NodeList<Element>());
                                }

                                //put together the parent selectors after the join
                                if (parentSel.Count > 1)
                                {
                                    afterParentJoin.AddRange(parentSel.Skip(1));
                                }

                                if (parentSel.Count > 0)
                                {
                                    newJoinedSelectorEmpty = false;

                                    // join the elements so far with the first part of the parent
                                    newJoinedSelector.Elements.Add(new Element(el.Combinator, parentSel[0].Elements[0].Value));
                                    newJoinedSelector.Elements.AddRange(parentSel[0].Elements.Skip(1));
                                }

                                if (!newJoinedSelectorEmpty)
                                {
                                    // now add the joined selector
                                    newSelectorPath.Add(newJoinedSelector);
                                }

                                // and the rest of the parent
                                newSelectorPath.AddRange(afterParentJoin);

                                // add that to our new set of selectors
                                selectorsMultiplied.Add(newSelectorPath);
                            }
                        }
                    }

                    // our new selectors has been multiplied, so reset the state
                    newSelectors = selectorsMultiplied;
                    currentElements = new NodeList<Element>();
                }
            }

            // if we have any elements left over (e.g. .a& .b == .b)
            // add them on to all the current selectors
            if (currentElements.Count > 0)
            {
                MergeElementsOnToSelectors(currentElements, newSelectors);
            }

            Paths.AddRange(newSelectors);
        }

        private void MergeElementsOnToSelectors(NodeList<Element> elements, List<List<Selector>> selectors)
        {
            if (selectors.Count == 0)
            {
                selectors.Add(new List<Selector>() { new Selector(elements) });
                return;
            }

            foreach (List<Selector> sel in selectors)
            {
                // if the previous thing in sel is a parent this needs to join on to it?
                if (sel.Count > 0)
                {
                    sel[sel.Count - 1] = new Selector(sel[sel.Count - 1].Elements.Concat(elements));
                }
                else
                {
                    sel.Add(new Selector(elements));
                }
            }
        }

        public void AppendCSS(Env env)
        {
            env.Output.AppendMany(
                Paths,
                path => path.Select(p => p.ToCSS(env)).JoinStrings("").Trim(),
                env.Compress ? "," : ",\n");
        }

        public int Count
        {
            get { return Paths.Count; }
        }

        public IEnumerator<IEnumerable<Selector>> GetEnumerator()
        {
            return Paths.Cast<IEnumerable<Selector>>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}