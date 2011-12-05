namespace dotless.Core.Parser.Tree
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Exceptions;
    using Infrastructure;
    using Infrastructure.Nodes;
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

                var clone = new Root(new NodeList(Rules), Error, OriginalRuleset).ReducedFrom<Root>(this);

                clone.EvaluateRules(env);
                clone.Evaluated = true;

                return clone;
            }
            catch (ParsingException e)
            {
                throw Error(e);
            }
        }
    }
}