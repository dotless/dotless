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

        public MixinDefinition(string name, NodeList<Rule> parameters, NodeList rules)
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
//            if (args != null && args.Any())
//                Guard.ExpectMaxArguments(Params.Count, args.Count, String.Format("'{0}'", Name), Index);

            var arguments = new Dictionary<string, Node>();
            args = args ?? new List<NamedArgument>();

            var hasNamedArgs = false;
            foreach (var arg in args)
            {
                if (!string.IsNullOrEmpty(arg.Name))
                {
                    hasNamedArgs = true;

                    arguments[arg.Name] = new Rule(arg.Name, arg.Value.Evaluate(env)) { Index = arg.Value.Index };
                }
                else if (hasNamedArgs)
                    throw new ParsingException("Positional arguments must appear before all named arguments.", arg.Value.Index);
            }

            for (var i = 0; i < Params.Count; i++)
            {
                if (String.IsNullOrEmpty(Params[i].Name))
                    continue;

                if (arguments.ContainsKey(Params[i].Name))
                    continue;

                Node val;
                if (i < args.Count && string.IsNullOrEmpty(args[i].Name))
                    val = args[i].Value;
                else
                    val = Params[i].Value;

                if (val)
                    arguments[Params[i].Name] = new Rule(Params[i].Name, val.Evaluate(env)) {Index = val.Index};
                else
                    throw new ParsingException(
                        String.Format("wrong number of arguments for {0} ({1} for {2})", Name,
                                      args != null ? args.Count : 0, _arity), Index);
            }

            var _arguments = new List<Node>();

            for(var i = 0; i < Math.Max(Params.Count, args.Count); i++)
            {
              _arguments.Add(i < args.Count ? args[i].Value : Params[i].Value);
            }

            var frame = new Ruleset(null, new NodeList());

            frame.Rules.Insert(0, new Rule("@arguments", new Expression(_arguments.Where(a => a != null)).Evaluate(env)));

            foreach (var arg in arguments)
            {
                frame.Rules.Add(arg.Value);
            }

            var frames = new[] { this, frame }.Concat(env.Frames).Concat(closureContext).Reverse();
            var context = env.CreateChildEnv(new Stack<Ruleset>(frames));

            var newRules = new NodeList();

            foreach (var rule in Rules)
            {
                if (rule is MixinDefinition)
                {
                    var mixin = rule as MixinDefinition;
                    var parameters = Enumerable.Concat(mixin.Params, frame.Rules.Cast<Rule>());
                    newRules.Add(new MixinDefinition(mixin.Name, new NodeList<Rule>(parameters), mixin.Rules));
                }
                else if (rule is Directive)
                {
                    newRules.Add(rule);
                }
                else if (rule is Ruleset)
                {
                    var ruleset = (rule as Ruleset);

                    context.Frames.Push(ruleset);

                    var rules = new NodeList(NodeHelper.NonDestructiveExpandNodes<MixinCall>(context, ruleset.Rules)
                        .Select(r => r.Evaluate(context)));

                    context.Frames.Pop();

                    newRules.Add(new Ruleset(ruleset.Selectors, rules));
                }
                else if (rule is MixinCall)
                {
                    newRules.AddRange((NodeList)rule.Evaluate(context));
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

            if (argsLength < _required)
              return false;
            if (_required > 0 && argsLength > _arity)
              return false;

            for (var i = 0; i < Math.Min(argsLength, _arity); i++)
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

        protected override void AppendCSS(Env env, Context context)
        {

        }
    }
}