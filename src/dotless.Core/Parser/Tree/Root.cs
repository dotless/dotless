namespace dotless.Core.Parser.Tree
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Exceptions;
    using Infrastructure;
    using Infrastructure.Nodes;

    public class Root : Ruleset
    {
        public Func<ParsingException, ParserException> Error { get; set; }

        public Root(List<Node> rules, Func<ParsingException, ParserException> error) :base(new NodeList<Selector>(), rules)
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

    }
}