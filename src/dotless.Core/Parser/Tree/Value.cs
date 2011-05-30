namespace dotless.Core.Parser.Tree
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Utils;
    using System.Text;

    public class Value : Node
    {
        public NodeList Values { get; set; }
        public string Important { get; set; }

        public Value(IEnumerable<Node> values, string important)
        {
            Values = new NodeList(values);
            Important = important;
        }

        public override void ToCSS(Env env, StringBuilder output)
        {
            Values.JoinStringBuilder(output, (v, builder) => v.ToCSS(env, builder), env.Compress ? "," : ", ");
 
            if  (!string.IsNullOrEmpty(Important)) {
                output.Append(" ");
                output.Append(Important);
            }
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