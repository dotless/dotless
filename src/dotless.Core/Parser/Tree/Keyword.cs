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

        protected override Node CloneCore() {
            return new Keyword(Value);
        }

        public override void AppendCSS(Env env)
        {
            env.Output.Append(Value);
        }

        public override string ToString()
        {
            return Value;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return -1;
            }
            return obj.ToString().CompareTo(ToString());
        }
    }
}