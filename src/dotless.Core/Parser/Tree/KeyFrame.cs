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
        public string Identifier { get; set; }

        public KeyFrame(string identifier, NodeList rules)
        {
            Identifier = identifier;
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
            env.Output.Append(Identifier);

            // Append pre comments as we out put each rule ourselves
            if (Rules.PreComments)
            {
                env.Output.Append(Rules.PreComments);
            }

            AppendRules(env);
        }
    }
}
