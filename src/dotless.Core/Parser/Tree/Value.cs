namespace dotless.Core.Parser.Tree
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Infrastructure.Nodes;

    public class Value : Node
    {
        public NodeList Values { get; set; }
        public NodeList PreImportantComments { get; set; }
        public string Important { get; set; }

        public Value(IEnumerable<Node> values, string important)
        {
            Values = new NodeList(values);
            Important = important;
        }

        public override void AppendCSS(Env env)
        {
            env.Output.AppendMany(Values, env.Compress ? "," : ", ");
 
            if  (!string.IsNullOrEmpty(Important)) 
            {
                if (PreImportantComments)
                {
                    env.Output.Append(PreImportantComments);
                }

                env.Output
                    .Append(" ")
                    .Append(Important);
            }
        }

        public override string ToString()
        {
            return ToCSS(new Env()); // only used during debugging.
        }

        public override Node Evaluate(Env env)
        {
            Node returnNode = null;
            Value value;

            if (Values.Count == 1 && string.IsNullOrEmpty(Important))
                returnNode = Values[0].Evaluate(env);
            else
            {
                returnNode = value = new Value(Values.Select(n => n.Evaluate(env)), Important);
                value.PreImportantComments = this.PreImportantComments;
            }

            return returnNode.ReducedFrom<Node>(this);
        }
    }
}