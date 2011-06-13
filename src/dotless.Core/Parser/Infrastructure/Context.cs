namespace dotless.Core.Parser.Infrastructure
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Tree;
    using Utils;

    public class Context : IEnumerable<IEnumerable<Selector>>
    {
        private List<List<Selector>> Paths { get; set; }

        public Context()
        {
            Paths = new List<List<Selector>>();
        }

        public void AppendSelectors(Context context, IEnumerable<Selector> selectors)
        {
            if (context == null || context.Paths.Count == 0)
            {
                Paths.AddRange(selectors.Select(s => new List<Selector> {s}));
                return;
            }

            foreach (var selector in selectors)
            {
                AppendSelector(context, selector);
            }
        }

        private void AppendSelector(Context context, Selector selector)
        {
            if (!selector.Elements.Any(e => e.Combinator.Value.StartsWith("&")))
            {
                Paths.AddRange(context.Paths.Select(path => path.Concat(new[] {selector}).ToList()));
                return;
            }

            var beforeEl = selector.Elements.TakeWhile(s => !s.Combinator.Value.StartsWith("&"));
            var afterEl = selector.Elements.SkipWhile(s => !s.Combinator.Value.StartsWith("&"));

            var before = new List<Selector>();
            var after = new List<Selector>();

            if (beforeEl.Any())
                before.Add(new Selector(beforeEl));

            if (afterEl.Any())
                after.Add(new Selector(afterEl));

            Paths.AddRange(context.Paths.Select(path => before.Concat(path).Concat(after).ToList()));
        }

        public void AppendCSS(Env env)
        {
            env.Output.AppendMany(
                Paths,
                path => path.Select(p => p.ToCSS(env)).JoinStrings("").Trim(),
                env.Compress ? "," : (Paths.Count > 3 ? ",\n" : ", "));
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