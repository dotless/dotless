namespace dotless.Core.Parser.Tree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Exceptions;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Utils;
    using Plugins;

    public class MixinCall : Node
    {
        public List<NamedArgument> Arguments { get; set; }
        public Selector Selector { get; set; }
        public bool Important { get; set; }

        public MixinCall(NodeList<Element> elements, List<NamedArgument> arguments, bool important)
        {
            Important = important;
            Selector = new Selector(elements);
            Arguments = arguments;
        }

        public override Node Evaluate(Env env)
        {
            var found = false;
            var closures = env.FindRulesets(Selector);

            if(closures == null)
                throw new ParsingException(Selector.ToCSS(env).Trim() + " is undefined", Index);

            env.Rule = this;

            var rules = new NodeList();

            if (PreComments)
                rules.AddRange(PreComments);

            foreach (var closure in closures)
            {
                var ruleset = closure.Ruleset;

                var matchType = ruleset.MatchArguments(Arguments, env);

                if (matchType == MixinMatch.ArgumentMismatch)
                    continue;

                found = true;

                if (matchType == MixinMatch.GuardFail)
                    continue;

                if (ruleset is MixinDefinition)
                {
                    try
                    {
                        var mixin = ruleset as MixinDefinition;
                        rules.AddRange(mixin.Evaluate(Arguments, env, closure.Context).Rules);
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
                        var nodes = new NodeList(ruleset.Rules);
                        NodeHelper.ExpandNodes<MixinCall>(env, nodes);

                        rules.AddRange(nodes);
                    }
                }
            }
            if (PostComments)
                rules.AddRange(PostComments);

            env.Rule = null;

            if (!found)
            {
                var message = String.Format("No matching definition was found for `{0}({1})`",
                                            Selector.ToCSS(env).Trim(),
                                            StringExtensions.JoinStrings(Arguments.Select(a => a.Value.ToCSS(env)), ", "));
                throw new ParsingException(message, Index);
            }

            if (Important)
            {
                var importantRules = new NodeList();

                foreach (Node node in rules)
                {
                    Rule r = node as Rule;
                    if (r != null)
                    {
                        var valueNode = r.Value;
                        var value = valueNode as Value;
                        if (value != null)
                        {
                            value = new Value(value.Values, "!important").ReducedFrom<Value>(value);
                        }
                        else
                        {
                            value = new Value(new NodeList() { valueNode }, "!important");
                        }

                        importantRules.Add((new Rule(r.Name, value)).ReducedFrom<Rule>(r));
                    }
                    else
                    {
                        importantRules.Add(node);
                    }
                }

                return importantRules;
            }
            else
            {
                return rules;
            }
        }

        public override void Accept(IVisitor visitor)
        {
            Selector = VisitAndReplace(Selector, visitor);

            foreach (var a in Arguments)
            {
                a.Value = VisitAndReplace(a.Value, visitor);
            }
        }
    }
}