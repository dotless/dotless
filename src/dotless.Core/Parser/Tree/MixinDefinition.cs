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

        public Ruleset Evaluate(NodeList<Expression> args, Env env, List<Ruleset> closureContext)
        {
            if (args)
                Guard.ExpectMaxArguments(Params.Count, args.Count, String.Format("'{0}'", Name), Index);

            var frame = new Ruleset(null, new List<Node>());

            for (var i = 0; i < Params.Count; i++)
            {
                if (!String.IsNullOrEmpty(Params[i].Name))
                {
                    Node val;
                    if (args && i < args.Count)
                        val = args[i];
                    else
                        val = Params[i].Value;

                    if (val)
                        frame.Rules.Add(new Rule(Params[i].Name, val.Evaluate(env)) { Index = val.Index });
                    else
                        throw new ParsingException(String.Format("wrong number of arguments for {0} ({1} for {2})", Name, args.Count, _arity), Index);
                }
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
                    var rules = ruleset.EvaluateRules(context);

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

        public override bool MatchArguements(NodeList<Expression> arguements, Env env)
        {
            var argsLength = arguements != null ? arguements.Count : 0;

            if (argsLength < _required || argsLength > _arity)
                return false;

            for (var i = 0; i < argsLength; i++)
            {
                if (String.IsNullOrEmpty(Params[i].Name))
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
}