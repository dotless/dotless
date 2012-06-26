namespace dotless.Core.Parser.Tree
{
    using Exceptions;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Plugins;

    public class Rule : Node
    {
        public string Name { get; set; }
        public Node Value { get; set; }
        public bool Variable { get; set; }
        public NodeList PostNameComments { get; set; }
        public bool IsSemiColonRequired { get; set; }
        public bool Variadic { get; set; }

        public Rule(string name, Node value) : this(name, value, false)
        { 
        }

        public Rule(string name, Node value, bool variadic)
        {
            Name = name;
            Value = value;
            Variable = !string.IsNullOrEmpty(name) && name[0] == '@';
            IsSemiColonRequired = true;
            Variadic = variadic;
        }

        public override Node Evaluate(Env env)
        {
            env.Rule = this;

            if (Value == null)
            {
                throw new ParsingException("No value found for rule " + Name, Location);
            }

            var rule = new Rule(Name, Value.Evaluate(env)).ReducedFrom<Rule>(this);
            rule.IsSemiColonRequired = this.IsSemiColonRequired;
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
                .Append(Value);

            if (IsSemiColonRequired)
            {
                env.Output.Append(";");
            }
        }

        public override void Accept(IVisitor visitor)
        {
            Value = VisitAndReplace(Value, visitor);
        }
    }
}