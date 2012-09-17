namespace dotless.Core.Parser.Tree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using dotless.Core.Parser.Infrastructure.Nodes;
    using dotless.Core.Parser.Infrastructure;

    public class KeyFrame : Ruleset
    {
        public NodeList Identifiers { get; set; }

        public KeyFrame(NodeList identifiers, NodeList rules)
        {
            Identifiers = identifiers;
            Rules = rules;
        }

        public override Node Evaluate(Env env)
        {
            env.Frames.Push(this);

            Rules = new NodeList(Rules.Select(r => r.Evaluate(env))).ReducedFrom<NodeList>(Rules);

            env.Frames.Pop();
            return this;
        }

        public override void Accept(Plugins.IVisitor visitor)
        {
            Rules = VisitAndReplace(Rules, visitor);
        }

        public override void AppendCSS(Env env, Context context)
        {
            env.Output.AppendMany(Identifiers, env.Compress ? "," : ", ");

            // Append pre comments as we out put each rule ourselves
            if (Rules.PreComments)
            {
                env.Output.Append(Rules.PreComments);
            }

            AppendRules(env);
        }
    }
}
