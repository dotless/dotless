namespace dotless.Core.Parser.Tree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Exceptions;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Utils;

    public class MixinDefinition : Ruleset
    {
        private int _required;
        private int _arity;
        public string Name { get; set; }
        public NodeList<Rule> Params { get; set; }

        public MixinDefinition(string name, NodeList<Rule> parameters, List<Node> rules)
        {
            Name = name;
            Params = parameters;
            Rules = rules;
            Selectors = new NodeList<Selector> {new Selector(new NodeList<Element>(new Element(null, name)))};

            _arity = Params.Count;
            _required = Params.Count(r => String.IsNullOrEmpty(r.Name) || r.Value == null);
        }

        public override Node Evaluate(Env env)
        {
            return this;
        }

        public Ruleset Evaluate(List<NamedArgument> args, Env env, List<Ruleset> closureContext)
        {
            if (args != null && args.Any())
                Guard.ExpectMaxArguments(Params.Count, args.Count, String.Format("'{0}'", Name), Index);

            var arguments = new Dictionary<string, Node>();
            args = args ?? new List<NamedArgument>();

            for (var i = 0; i < Params.Count; i++)
            {
                if (!String.IsNullOrEmpty(Params[i].Name))
                {
                    Node val;
                    if (i < args.Count && string.IsNullOrEmpty(args[i].Name))
                        val = args[i].Value;
                    else
                        val = Params[i].Value;

                    if (val)
                        arguments[Params[i].Name] = new Rule(Params[i].Name, val.Evaluate(env)) { Index = val.Index };
                    else
                        throw new ParsingException(
                            String.Format("wrong number of arguments for {0} ({1} for {2})", Name, args != null ? args.Count : 0, _arity), Index);
                }
            }

            var hasNamedArgs = false;
            foreach (var arg in args)
            {
                if(!string.IsNullOrEmpty(arg.Name))
                {
                    hasNamedArgs = true;

                    arguments[arg.Name] = new Rule(arg.Name, arg.Value.Evaluate(env)) { Index = arg.Value.Index };
                }
                else if (hasNamedArgs)
                    throw new ParsingException("Positional arguments must appear before all named arguments.", arg.Value.Index);
            }

            var frame = new Ruleset(null, new List<Node>());

            foreach (var arg in arguments)
            {
                frame.Rules.Add(arg.Value);
            }

            var frames = new[] { this, frame }.Concat(env.Frames).Concat(closureContext).Reverse();
            var context = new Env {Frames = new Stack<Ruleset>(frames)};

            var newRules = new List<Node>();

            foreach (var rule in Rules)
            {
                if (rule is MixinDefinition)
                {
                    var mixin = rule as MixinDefinition;
                    var parameters = Enumerable.Concat(mixin.Params, frame.Rules.Cast<Rule>());
                    newRules.Add(new MixinDefinition(mixin.Name, new NodeList<Rule>(parameters), mixin.Rules));
                }
                else if (rule is Ruleset)
                {
                    var ruleset = (rule as Ruleset);

                    context.Frames.Push(ruleset);

                    var rules = NodeHelper.NonDestructiveExpandNodes<MixinCall>(context, ruleset.Rules)
                        .Select(r => r.Evaluate(context)).ToList();

                    context.Frames.Pop();

                    newRules.Add(new Ruleset(ruleset.Selectors, rules));
                }
                else if (rule is MixinCall)
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

        public override bool MatchArguments(List<NamedArgument> arguements, Env env)
        {
            var argsLength = arguements != null ? arguements.Count : 0;

            if (argsLength < _required || argsLength > _arity)
                return false;

            for (var i = 0; i < argsLength; i++)
            {
                if (String.IsNullOrEmpty(Params[i].Name))
                {
                    if (arguements[i].Value.Evaluate(env).ToCSS(env) != Params[i].Value.Evaluate(env).ToCSS(env))
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
}