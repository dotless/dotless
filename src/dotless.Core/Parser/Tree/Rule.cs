using dotless.Core.Exceptions;

namespace dotless.Core.Parser.Tree
{
    using Infrastructure;
    using Infrastructure.Nodes;
    using System.Text;

    public class Rule : Node
    {
        public string Name { get; set; }
        public Node Value { get; set; }
        public bool Variable { get; set; }

        public Rule(string name, Node value)
        {
            Name = name;
            Value = value;
            Variable = name != null ? name[0] == '@' : false;
        }

        public override Node Evaluate(Env env)
        {
            env.Rule = this;

            if (Value == null)
            {
                throw new ParsingException("No value found for rule " + Name, Index);
            }
            
            var rule = new Rule(Name, Value.Evaluate(env)) {Index = Index};

            env.Rule = null;
            
            return rule;
        }

        public override void ToCSS(Env env, StringBuilder output)
        {
            if (Variable)
                return;

            output.Append(Name);

            output.Append(env.Compress ? ":" : ": ");
            Value.ToCSS(env, output);
            output.Append(";");
        }
    }
}