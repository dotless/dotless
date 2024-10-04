namespace dotless.Core.Parser.Tree
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Plugins;
    using System;

    public class Value : Node
    {
        public NodeList Values { get; set; }
        public NodeList PreImportantComments { get; set; }
        public string Merge { get; set; }
        public string Important { get; set; }

        public void AppendValues(IEnumerable<Node> values)
        {
            Values.AddRange(values);
        }

        public Value(IEnumerable<Node> values, string important, string merge = ", ")
        {
            Values = new NodeList(values);
            Important = important;
            Merge = merge;
        }

        protected override Node CloneCore() {
            return new Value((NodeList)Values.Clone(), Important, Merge);
        }

        public override void AppendCSS(Env env)
        {
            var separator = Merge;
            if(env.Compress && Merge.Length > 1) 
                separator = separator.Substring(0, 1);
            env.Output.AppendMany(Values, separator);
 
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
            return ToCSS(new Env(null)); // only used during debugging.
        }

        public override Node Evaluate(Env env)
        {
            Node returnNode = null;
            Value value;

            if (Values.Count == 1 && string.IsNullOrEmpty(Important))
                returnNode = Values[0].Evaluate(env);
            else
            {
                returnNode = value = new Value(Values.Select(n => n.Evaluate(env)), Important, Merge);
                value.PreImportantComments = this.PreImportantComments;
            }

            return returnNode.ReducedFrom<Node>(this);
        }

        public override void Accept(IVisitor visitor)
        {
            Values = VisitAndReplace(Values, visitor);
        }
    }
}