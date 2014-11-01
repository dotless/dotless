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
        public Condition Condition { get; set; }
        public bool Variadic { get; set; }

        public MixinDefinition(string name, NodeList<Rule> parameters, NodeList rules, Condition condition, bool variadic)
        {
            Name = name;
            Params = parameters;
            Rules = rules;
            Condition = condition;
            Variadic = variadic;
            Selectors = new NodeList<Selector> {new Selector(new NodeList<Element>(new Element(null, name)))};

            _arity = Params.Count;
            _required = Params.Count(r => String.IsNullOrEmpty(r.Name) || r.Value == null);
        }

        public override Node Evaluate(Env env)
        {
            return this;
        }

        public Ruleset EvaluateParams(Env env, List<NamedArgument> args)
        {
            var arguments = new Dictionary<string, Node>();
            args = args ?? new List<NamedArgument>();

            var hasNamedArgs = false;
            foreach (var arg in args)
            {
                if (!string.IsNullOrEmpty(arg.Name))
                {
                    hasNamedArgs = true;

                    arguments[arg.Name] = new Rule(arg.Name, arg.Value.Evaluate(env)) { Location = arg.Value.Location };
                }
                else if (hasNamedArgs)
                    throw new ParsingException("Positional arguments must appear before all named arguments.", arg.Value.Location);
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
                {
                    //evaluate in scope of mixin definition?
                    val = Params[i].Value;
                }

                if (val)
                {
                    Node argRuleValue;
                    if (Params[i].Variadic)
                    {
                        NodeList varArgs = new NodeList();
                        for (int j = i; j < args.Count; j++)
                        {
                            varArgs.Add(args[j].Value.Evaluate(env));
                        }

                        argRuleValue = (new Expression(varArgs)).Evaluate(env);
                    }
                    else
                    {
                        argRuleValue = val.Evaluate(env);
                    }
                    arguments[Params[i].Name] = new Rule(Params[i].Name, argRuleValue) { Location = val.Location };
                }
                else
                    throw new ParsingException(
                        String.Format("wrong number of arguments for {0} ({1} for {2})", Name,
                                      args != null ? args.Count : 0, _arity), Location);
            }

            var argumentNodes = new List<Node>();

            for(var i = 0; i < Math.Max(Params.Count, args.Count); i++)
            {
              argumentNodes.Add(i < args.Count ? args[i].Value : Params[i].Value);
            }

            var frame = new Ruleset(null, new NodeList());

            frame.Rules.Insert(0, new Rule("@arguments", new Expression(argumentNodes.Where(a => a != null)).Evaluate(env)));

            foreach (var arg in arguments)
            {
                frame.Rules.Add(arg.Value);
            }

            return frame;
        }

        public Ruleset Evaluate(List<NamedArgument> args, Env env, List<Ruleset> closureContext)
        {
            var frame = EvaluateParams(env, args);

            var frames = new[] { this, frame }.Concat(env.Frames).Concat(closureContext).Reverse();
            var context = env.CreateChildEnv(new Stack<Ruleset>(frames));

            return EvaluateRulesForFrame(frame, context);
        }

        public override MixinMatch MatchArguments(List<NamedArgument> arguments, Env env)
        {
            var argsLength = arguments != null ? arguments.Count : 0;

            if (!Variadic)
            {
                if (argsLength < _required)
                    return MixinMatch.ArgumentMismatch;
                if (argsLength > _arity)
                    return MixinMatch.ArgumentMismatch;
            }

            if (Condition)
            {
                env.Frames.Push(EvaluateParams(env, arguments));

                bool isPassingConditions = Condition.Passes(env);

                env.Frames.Pop();

                if (!isPassingConditions)
                    return MixinMatch.GuardFail;
            }

            for (var i = 0; i < Math.Min(argsLength, _arity); i++)
            {
                if (String.IsNullOrEmpty(Params[i].Name))
                {
                    if (arguments[i].Value.Evaluate(env).ToCSS(env) != Params[i].Value.Evaluate(env).ToCSS(env))
                    {
                        return MixinMatch.ArgumentMismatch;
                    }
                }
            }

            return MixinMatch.Pass;
        }

        public override void Accept(Plugins.IVisitor visitor)
        {
            base.Accept(visitor);

            Params = VisitAndReplace(Params, visitor);
            Condition = VisitAndReplace(Condition, visitor, true);
        }

        public override void AppendCSS(Env env, Context context)
        {
            
        }
    }
}