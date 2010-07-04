// ReSharper disable RedundantNameQualifier

namespace dotless.Core.Parser.Tree
{
    using System.Collections.Generic;
    using System.Linq;
    using Exceptions;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Utils;

    public class Mixin
    {
        public class Definition : Ruleset
        {
            private int _required;
            private int _arity;
            public string Name { get; set; }
            public NodeList<Rule> Params { get; set; }

            public Definition(string name, NodeList<Rule> parameters, List<Node> rules)
            {
                Name = name;
                Params = parameters;
                Rules = rules;
                Selectors = new NodeList<Selector> {new Selector(new NodeList<Element>(new Element(null, name)))};

                _arity = Params.Count;
                _required = Params.Count(r => string.IsNullOrEmpty(r.Name) || r.Value == null);
            }

            public override Node Evaluate(Env env)
            {
                return this;
            }

            public Ruleset Evaluate(NodeList<Expression> args, Env env)
            {
                if (args)
                    Guard.ExpectMaxArguments(Params.Count, args.Count, string.Format("'{0}'", Name), Index);

                var frame = new Ruleset(null, new List<Node>());

                for (var i = 0; i < Params.Count; i++)
                {
                    if (!string.IsNullOrEmpty(Params[i].Name))
                    {
                        Node val;
                        if (args && i < args.Count)
                            val = args[i];
                        else
                            val = Params[i].Value;

                        if (val)
                            frame.Rules.Add(new Rule(Params[i].Name, val.Evaluate(env)) { Index = val.Index });
                        else
                            throw new ParsingException(string.Format("wrong number of arguments for {0} ({1} for {2})", Name, args.Count, _arity), Index);
                    }
                }

                var frames = new[] {this, frame}.Concat(env.Frames).Reverse();
                var context = new Env {Frames = new Stack<Ruleset>(frames)};

                var newRules = new List<Node>();

                foreach (var rule in Rules)
                {
                    if (rule is Mixin.Definition)
                    {
                        var mixin = rule as Mixin.Definition;
                        var parameters = mixin.Params.Concat(frame.Rules.Cast<Rule>());
                        newRules.Add(new Mixin.Definition(mixin.Name, new NodeList<Rule>(parameters), mixin.Rules));
                    }
                    else if (rule is Ruleset)
                    {
                        var ruleset = (rule as Ruleset);
                        var rules = ruleset.EvaluateRules(context);

                        newRules.Add(new Ruleset(ruleset.Selectors, rules));
                    }
                    else if (rule is Mixin.Call)
                    {
                        newRules.AddRange((NodeList) rule.Evaluate(context));
                    }
                    else
                    {
                        newRules.Add(rule.Evaluate(context));
                    }
                }

                return new Ruleset(null, newRules);
            }

            public override bool MatchArguements(NodeList<Expression> arguements, Env env)
            {
                var argsLength = arguements != null ? arguements.Count : 0;

                if (argsLength < _required || argsLength > _arity)
                    return false;

                for (var i = 0; i < argsLength; i++)
                {
                    if (string.IsNullOrEmpty(Params[i].Name))
                    {
                        if (arguements[i].Evaluate(env).ToCSS(env) != Params[i].Value.Evaluate(env).ToCSS(env))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            protected override string ToCSS(Env env, Context list)
            {
                return "";
            }
        }

        public class Call : Node
        {
            public NodeList<Expression> Arguments { get; set; }
            protected Selector Selector { get; set; }

            public Call(NodeList<Element> elements, NodeList<Expression> arguments)
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

                    if (ruleset is Mixin.Definition)
                    {
                        try
                        {
                            var mixin = ruleset as Mixin.Definition;
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
                            NodeHelper.ExpandNodes<Mixin.Call>(env, nodes);

                            rules.AddRange(nodes);
                        }
                    }
                }

                if (!found)
                {
                    var message = string.Format("No matching definition was found for `{0}({1})`",
                                                Selector.ToCSS(env).Trim(),
                                                Arguments.Select(a => a.ToCSS(env)).JoinStrings(", "));
                    throw new ParsingException(message, Index);
                }

                return rules;
            }
        }
    }
}