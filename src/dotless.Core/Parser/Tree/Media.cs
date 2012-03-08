namespace dotless.Core.Parser.Tree
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Infrastructure.Nodes;

    public class Media : Ruleset
    {
        public Node Features { get; set; }
        public Ruleset Ruleset { get; set; }

        public Media(Node features, NodeList rules)
        {
            Features = features;
            NodeList<Selector> selectors = new NodeList<Selector>() { new Selector(new NodeList<Element>() { new Element(new Combinator("&"), "") }) };
            Ruleset = new Ruleset(selectors, rules);
        }

        public Media(Node features, Ruleset ruleset)
        {
            Features = features;
            Ruleset = ruleset;
        }

        public override void Accept(Plugins.IVisitor visitor)
        {
            Features = VisitAndReplace(Features, visitor);

            Ruleset = VisitAndReplace(Ruleset, visitor);
        }

        public override Node Evaluate(Env env)
        {
            //env.Frames.Push(this);

            var features = Features.Evaluate(env);
            var ruleset = Ruleset.Evaluate(env) as Ruleset;

            //env.Frames.Pop();

            return new Media(features, ruleset).ReducedFrom<Media>(this);
        }

        public override void AppendCSS(Env env, Context ctx)
        {
            if (env.Compress && !Ruleset.Rules.Any())
                return;

            env.Output.Append("@media");

            if (Features)
            {
                env.Output.Append(' ');
                env.Output.Append(Features);
            }

            if (env.Compress)
                env.Output.Append('{');
            else
                env.Output.Append(" {\n");

            Ruleset.IsRoot = ctx.Count == 0;

            env.Output.Push();
            Ruleset.AppendCSS(env, ctx);

            if (!env.Compress)
                env.Output.Trim().Indent(2);

            env.Output.PopAndAppend();

            if (env.Compress)
                env.Output.Append('}');
            else
                env.Output.Append("\n}\n");
        }
    }
}