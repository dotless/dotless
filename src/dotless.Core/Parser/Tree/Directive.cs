namespace dotless.Core.Parser.Tree
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Utils;
    using System.Text;

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

        protected override StringBuilder ToCSS(Env env, Context context, StringBuilder output)
        {
            output.Append(Name);

            if (Rules != null)
            {
                return output.Append(env.Compress ? "{" : " {\n")
                    .Append(
                        Rules.ToCSS(env, "\n")
                        .Trim()
                        .Indent(env.Compress ? 0 : 2))
                    .Append(env.Compress ? "}" : "\n}\n");
            }
            else
            {
                return output.Append(" ")
                    .AppendCSS(Value, env)
                    .Append(";\n");
            }
        }
    }
}