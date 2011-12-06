namespace dotless.Core.Parser.Tree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
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
        }

        public override void AppendCSS(Env env)
        {
            try
            {
                base.AppendCSS(env);

                if (env.Compress)
                    env.Output.Reset(Regex.Replace(env.Output.ToString(), @"(\s)+", " "));
            }
            catch (ParsingException e)
            {
                throw Error(e);
            }
        }

        public override Node Evaluate(Env env)
        {
            if(Evaluated) return this;

            try
            {
                env = env ?? new Env();

                NodeHelper.ExpandNodes<Import>(env, Rules);

                var clone = new Root(new NodeList(Rules), Error, OriginalRuleset);

                if(env.Plugins != null)
                {
                    clone = env.Plugins
                        .Where(p => p.AppliesTo == PluginType.BeforeEvaluation)
                        .Aggregate(clone, (current, plugin) => (Root)plugin.Apply(current));
                }

                clone.ReducedFrom<Root>(this);
                clone.EvaluateRules(env);
                clone.Evaluated = true;

                if(env.Plugins != null)
                {
                    clone = env.Plugins
                        .Where(p => p.AppliesTo == PluginType.AfterEvaluation)
                        .Aggregate(clone, (current, plugin) => (Root)plugin.Apply(current));
                }

                return clone;
            }
            catch (ParsingException e)
            {
                throw Error(e);
            }
        }
    }
}