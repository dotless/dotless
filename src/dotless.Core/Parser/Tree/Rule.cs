namespace dotless.Core.Parser.Tree
{
    using Exceptions;
    using Infrastructure;
    using Infrastructure.Nodes;

    public class Rule : Node
    {
        public string Name { get; set; }
        public Node Value { get; set; }
        public bool Variable { get; set; }
        public NodeList PostNameComments { get; set; }

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

            var rule = new Rule(Name, Value.Evaluate(env)).ReducedFrom<Rule>(this);
            rule.PostNameComments = this.PostNameComments;

            env.Rule = null;

            return rule;
        }

        public override void AppendCSS(Env env)
        {
            if (Variable)
                return;

            env.Output
                .Append(Name)
                .Append(PostNameComments)
                .Append(env.Compress ? ":" : ": ")
                .Append(Value)
                .Append(";");
        }
    }
}