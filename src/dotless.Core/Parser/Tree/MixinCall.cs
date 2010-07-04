namespace dotless.Core.Parser.Tree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Exceptions;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Utils;

    public class MixinCall : Node
    {
        public NodeList<Expression> Arguments { get; set; }
        protected Selector Selector { get; set; }

        public MixinCall(NodeList<Element> elements, NodeList<Expression> arguments)
        {
            Selector = new Selector(elements);
            Arguments = arguments;
        }

        public override Node Evaluate(Env env)
        {
            var found = false;
            var rulesets = env.FindRulesets(Selector);

            if(rulesets == null)
                throw new ParsingException(Selector.ToCSS(env).Trim() + " is undefined", Index);

            var rules = new NodeList();
            foreach (var ruleset in rulesets)
            {
                if (!ruleset.MatchArguements(Arguments, env))
                    continue;

                found = true;

                if (ruleset is MixinDefinition)
                {
                    try
                    {
                        var mixin = ruleset as MixinDefinition;
                        rules.AddRange(mixin.Evaluate(Arguments, env).Rules);
                    }
                    catch (ParsingException e)
                    {
                        throw new ParsingException(e.Message, e.Index, Index);
                    }
                }
                else
                {
                    if (ruleset.Rules != null)
                    {
                        var nodes = new List<Node>(ruleset.Rules);
                        NodeHelper.ExpandNodes<MixinCall>(env, nodes);

                        rules.AddRange(nodes);
                    }
                }
            }

            if (!found)
            {
                var message = String.Format("No matching definition was found for `{0}({1})`",
                                            Selector.ToCSS(env).Trim(),
                                            StringExtensions.JoinStrings(Arguments.Select(a => a.ToCSS(env)), ", "));
                throw new ParsingException(message, Index);
            }

            return rules;
        }
    }
}