using System.Collections.Generic;

namespace dotless.Core.Parser.Tree
{
    using Exceptions;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Plugins;
    using System.Text.RegularExpressions;

    public class Rule : Node
    {
        public string Name { get; set; }
        public Node Value { get; set; }
        public bool Variable { get; set; }
        public NodeList PostNameComments { get; set; }
        public bool IsSemiColonRequired { get; set; }
        public bool Variadic { get; set; }
        public string Merge { get; set; }

        public bool Important
        {
            get { return Value is Value && ((Value) Value).Important == "!important"; }
        }

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
            Merge = "";
        }

        public override Node Evaluate(Env env)
        {
            env.Rule = this;

            if (Value == null)
            {
                throw new ParsingException("No value found for rule " + Name, Location);
            }

            var rule = new Rule(Name, Value.Evaluate(env)).ReducedFrom<Rule>(this);
            rule.Merge = Merge;
            rule.IsSemiColonRequired = this.IsSemiColonRequired;
            rule.PostNameComments = this.PostNameComments;

            env.Rule = null;

            return rule;
        }

        protected override Node CloneCore() {
            return new Rule(Name, Value.Clone(), Variadic) {
                IsSemiColonRequired = IsSemiColonRequired,
                Variable = Variable
            };
        }

        public override void AppendCSS(Env env)
        {
            if (Variable)
                return;

            var value = Value;

            env.Output
                .Append(Name)
                .Append(PostNameComments)
                .Append(env.Compress ? ":" : ": ");

            env.Output.Push()
                .Append(value);

            if (env.Compress)
            {
                env.Output.Reset(Regex.Replace(env.Output.ToString(), @"(\s)+", " ").Replace(", ", ","));
            }

            env.Output.PopAndAppend();

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