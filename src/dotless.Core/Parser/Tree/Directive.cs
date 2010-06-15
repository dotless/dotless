namespace dotless.Core.Parser.Tree
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Utils;

    public class Directive : Ruleset
    {
        public string Name { get; set; }
        public Node Value { get; set; }

        public Directive(string name, List<Node> rules)
        {
            Name = name;
            Rules = rules;
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
                Rules = new List<Node>(Rules.Select(r => r.Evaluate(env)));
            else
                Value = Value.Evaluate(env);

            env.Frames.Pop();

            return this;
        }

        protected override string ToCSS(Env env, Context context)
        {
            if (Rules != null)
                return Name + 
                    (env.Compress ? "{" : " {\n") + 
                    Rules.Select(r => r.ToCSS(env)).JoinStrings("\n").Trim().Indent(env.Compress ? 0 : 2) + 
                    (env.Compress ? "}" : "\n}\n");

            return Name + " " + Value.ToCSS(env) + ";\n";
        }
    }
}