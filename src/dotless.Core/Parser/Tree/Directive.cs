﻿namespace dotless.Core.Parser.Tree
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Infrastructure.Nodes;

    public class Directive : Ruleset
    {
        public string Name { get; set; }
        public string Identifier { get; set; }
        public Node Value { get; set; }

        public Directive(string name, string identifier, NodeList rules)
        {
            Name = name;
            Rules = rules;
            Identifier = identifier;
        }

        public Directive(string name, Node value)
        {
            Name = name;
            Value = value;
        }

        protected Directive()
        {
        }

        public override Node Evaluate(Env env)
        {
            env.Frames.Push(this);

            if (Rules != null)
                Rules = new NodeList(Rules.Select(r => r.Evaluate(env))).ReducedFrom<NodeList>(Rules);
            else
                Value = Value.Evaluate(env);

            env.Frames.Pop();

            return this;
        }

        protected override void AppendCSS(Env env, Context context)
        {
            env.Output.Append(Name);

            if (!string.IsNullOrEmpty(Identifier))
            {
                env.Output.Append(" ");
                env.Output.Append(Identifier);
            }

            if (Rules != null)
            {
                // Append pre comments as we out put each rule ourselves
                if (Rules.PreComments)
                {
                    env.Output.Append(Rules.PreComments);
                }

                AppendRules(env);
                env.Output.Append("\n");
            }
            else
            {
                env.Output
                    .Append(" ")
                    .Append(Value)
                    .Append(";\n");
            }
        }
    }
}