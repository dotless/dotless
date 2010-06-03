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
                Paths.AddRange(context.Paths.Select(path => path.Concat(new[] {selector}).ToList()));
            }
        }

        public override string ToString()
        {
            return Paths
                .Select(p => p.Select(s => s.ToCSS()).JoinStrings("").Trim())
                .JoinStrings(Paths.Count > 3 ? ",\n" : ", ");
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