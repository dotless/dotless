namespace dotless.Core.Parser.Tree
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Utils;

    public class Value : Node
    {
        public NodeList Values { get; set; }
        public string Important { get; set; }

        public Value(IEnumerable<Node> values, string important)
        {
            Values = new NodeList(values);
            Important = important;
        }

        public override string ToCSS(Env env)
        {
            return 
                Values.Select(v => v.ToCSS(env)).JoinStrings(env.Compress ? "," : ", ") + 
                (string.IsNullOrEmpty(Important) ? "" : " " + Important);
        }

        public override string ToString()
        {
            return ToCSS(new Env()); // only used during debugging.
        }

        public override Node Evaluate(Env env)
        {
            if (Values.Count == 1 && string.IsNullOrEmpty(Important))
                return Values[0].Evaluate(env);

            return new Value(Values.Select(n => n.Evaluate(env)), Important);
        }
    }
}