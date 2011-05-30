using System.Collections.Generic;

namespace dotless.Core.Parser.Tree
{
    using System.Linq;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Utils;
    using System.Text;

    public class Expression : Node
    {
        public NodeList Value { get; set; }

        public Expression(IEnumerable<Node> value)
        {
            if(value is NodeList)
                Value = value as NodeList;
            else
                Value = new NodeList(value);
        }

        public override Node Evaluate(Env env)
        {
            if (Value.Count > 1)
                return new Expression(new NodeList(Value.Select(e => e.Evaluate(env))));

            if (Value.Count == 1)
                return Value[0].Evaluate(env);

            return this;
        }

        public override StringBuilder ToCSS(Env env, StringBuilder output)
        {
            return output.AppendCSS(Value, env, " ");
        }
    }
}