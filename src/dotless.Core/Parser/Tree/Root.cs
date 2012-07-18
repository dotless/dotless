namespace dotless.Core.Parser.Tree
{
    using System;
    using System.Linq;
    using Exceptions;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Plugins;
    using dotless.Core.Utils;

    public class Root : Ruleset
    {
        public Func<ParsingException, ParserException> Error { get; set; }

        public Root(NodeList rules, Func<ParsingException, ParserException> error) 
            : this(rules, error, null)
        {
        }

        protected Root(NodeList rules, Func<ParsingException, ParserException> error, Ruleset master) 
            : base(new NodeList<Selector>(), rules, master)
        {
            Error = error;
            IsRoot = true;
        }

        public override void AppendCSS(Env env)
        {
            try
            {
                base.AppendCSS(env);
            }
            catch (ParsingException e)
            {
                throw Error(e);
            }
        }

        private Root DoVisiting(Root node, Env env, VisitorPluginType pluginType)
        {
            return env.VisitorPlugins
                .Where(p => p.AppliesTo == pluginType)
                .Aggregate(node, (current, plugin) => 
                {
                    try
                    {
                        plugin.OnPreVisiting(env);
                        Root r = plugin.Apply(current);
                        plugin.OnPostVisiting(env);
                        return r;
                    }
                    catch (Exception ex)
                    {
                        string message = string.Format("Plugin '{0}' failed during visiting with error '{1}'", plugin.GetName(), ex.Message);
                        throw new ParserException(message, ex);
                    }
                });

        }

        public override Node Evaluate(Env env)
        {
            if(Evaluated) return this;

            try
            {
                env = env ?? new Env();

                env.Frames.Push(this);
                NodeHelper.ExpandNodes<Import>(env, Rules);
                env.Frames.Pop();

                var clone = new Root(new NodeList(Rules), Error, OriginalRuleset);

                clone = DoVisiting(clone, env, VisitorPluginType.BeforeEvaluation);

                clone.ReducedFrom<Root>(this);
                clone.EvaluateRules(env);
                clone.Evaluated = true;

                clone = DoVisiting(clone, env, VisitorPluginType.AfterEvaluation);
                return clone;
            }
            catch (ParsingException e)
            {
                throw Error(e);
            }
        }
    }
}