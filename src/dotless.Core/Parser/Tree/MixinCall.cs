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

            // To address bug https://github.com/dotless/dotless/issues/136, where a mixin and ruleset selector may have the same name, we
            // need to favour matching a MixinDefinition with the required Selector and only fall back to considering other Ruleset types
            // if no match is found.
            var closures = env.FindRulesets<MixinDefinition>(Selector) ?? env.FindRulesets<Ruleset>(Selector);
            if(closures == null)
                throw new ParsingException(Selector.ToCSS(env).Trim() + " is undefined", Location);

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
                        throw new ParsingException(e.Message, e.Location, Location);
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
                                            Arguments.Select(a => a.Value.ToCSS(env)).JoinStrings(env.Compress ? "," : ", "));
                throw new ParsingException(message, Location);
            }

            if (Important)
            {
                var importantRules = new NodeList();

                foreach (Node node in rules)
                {
                    if (node is Rule)
                    {
                        importantRules.Add(MakeRuleImportant(node as Rule));
                    }
                    else if (node is Ruleset)
                    {
                        importantRules.Add(MakeRulesetImportant(node as Ruleset));
                    }
                    else
                    {
                        importantRules.Add(node);
                    }
                }

                return importantRules;
            }

            return rules;
        }

        public override void Accept(IVisitor visitor)
        {
            Selector = VisitAndReplace(Selector, visitor);

            foreach (var a in Arguments)
            {
                a.Value = VisitAndReplace(a.Value, visitor);
            }
        }

        private Ruleset MakeRulesetImportant(Ruleset ruleset)
        {
            var importantRules = new NodeList();
            foreach (var x in ruleset.Rules)
            {
                if (x is Rule)
                {
                    importantRules.Add(MakeRuleImportant((Rule) x));
                }
                else if (x is Ruleset)
                {
                    importantRules.Add(MakeRulesetImportant((Ruleset) x));
                }
                else
                {
                    importantRules.Add(x);
                }
            }
            var importantRuleset = new Ruleset(ruleset.Selectors, importantRules).ReducedFrom<Ruleset>(ruleset);
            return importantRuleset;
        }

        private Rule MakeRuleImportant(Rule rule)
        {
            var valueNode = rule.Value;
            var value = valueNode as Value;
            value = value != null
                        ? new Value(value.Values, "!important").ReducedFrom<Value>(value)
                        : new Value(new NodeList {valueNode}, "!important");

            var importantRule = new Rule(rule.Name, value).ReducedFrom<Rule>(rule);
            return importantRule;
        }
    }
}