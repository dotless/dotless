namespace dotless.Core.Parser.Tree
{
    using Infrastructure;
    using Infrastructure.Nodes;
    using System;

    public class Keyword : Node, IComparable
    {
        public string Value { get; set; }

        public Keyword(string value)
        {
            Value = value;
        }

        public override Node Evaluate(Env env)
        {
            return ((Node) Color.GetColorFromKeyword(Value) ?? this).ReducedFrom<Node>(this);
        }

        public override void AppendCSS(Env env)
        {
            env.Output.Append(Value);
        }

        public int CompareTo(object obj)
        {
            Keyword k = obj as Keyword;
            if (k)
            {
                return k.Value.CompareTo(Value);
            }
            return -1;
        }
    }
}