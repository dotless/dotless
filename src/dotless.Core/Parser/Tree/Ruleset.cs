namespace dotless.Core.Parser.Tree
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Utils;
    using System.Text;

    public class Ruleset : Node
    {
        public NodeList<Selector> Selectors { get; set; }
        public List<Node> Rules { get; set; }

        private Dictionary<string, List<Closure>> _lookups;

        public Ruleset(NodeList<Selector> selectors, List<Node> rules)
            : this()
        {
            Selectors = selectors;
            Rules = rules;
        }

        protected Ruleset()
        {
            _lookups = new Dictionary<string, List<Closure>>();
        }

        public Rule Variable(string name, Node startNode)
        {
            return Rules
                .TakeWhile(r => r != startNode)
                .OfType<Rule>()
                .Where(r => r.Variable)
                .Reverse()
                .FirstOrDefault(r => r.Name == name);
        }

        public List<Ruleset> Rulesets()
        {
            return Rules.OfType<Ruleset>().ToList();
        }

        public List<Closure> Find(Env env, Selector selector, Ruleset self)
        {
            self = self ?? this;
            var rules = new List<Closure>();
            var key = selector.ToCSS(env);

            if (_lookups.ContainsKey(key))
                return _lookups[key];

            foreach (var rule in Rulesets().Where(rule => rule != self))
            {
                if (rule.Selectors && rule.Selectors.Any(selector.Match))
                {
                    if (selector.Elements.Count > 1)
                    {
                        var remainingSelectors = new Selector(new NodeList<Element>(selector.Elements.Skip(1)));
                        var closures = rule.Find(env, remainingSelectors, self);

                        foreach (var closure in closures)
                        {
                            closure.Context.Insert(0, rule);
                        }

                        rules.AddRange(closures);
                    }
                    else
                        rules.Add(new Closure { Ruleset = rule, Context = new List<Ruleset> { rule } });
                }
            }
            return _lookups[key] = rules;
        }

        public virtual bool MatchArguments(List<NamedArgument> arguements, Env env)
        {
            return arguements == null || arguements.Count == 0;
        }

        public override Node Evaluate(Env env)
        {
            if (this is Root)
            {
                env = env ?? new Env();

                NodeHelper.ExpandNodes<Import>(env, this.Rules);
            }

            EvaluateRules(env);

            return this;
        }

        public List<Node> EvaluateRules(Env env)
        {
            env.Frames.Push(this);

            NodeHelper.ExpandNodes<MixinCall>(env, this.Rules);

            for (var i = 0; i < Rules.Count; i++)
            {
                Rules[i] = Rules[i].Evaluate(env);
            }

            env.Frames.Pop();

            return Rules;
        }

        public string AppendCSS()
        {
            var env = new Env();

            AppendCSS(env);

            return env.Output.ToString();
        }

        public override void AppendCSS(Env env)
        {
            if (!Rules.Any())
                return;

            Evaluate(env);

            AppendCSS(env, new Context());
        }

        protected virtual void AppendCSS(Env env, Context context)
        {
            var css = new List<string>(); // The CSS output
            var rules = new List<StringBuilder>(); // node.Ruleset instances
            var paths = new Context(); // Current selectors
            bool isRoot = this is Root;

            if (!isRoot)
            {
                paths.AppendSelectors(context, Selectors);
            }

            env.Output.Push();

            foreach (var node in Rules)
            {
                if (node.IgnoreOutput())
                    continue;

                var comment = node as Comment;
                if(comment != null && comment.Silent)
                    continue;

                var ruleset = node as Ruleset;
                if (ruleset != null)
                {
                    ruleset.AppendCSS(env, paths);
                }
                else {
                    var rule = node as Rule;
                    if ((rule != null && !rule.Variable) || (rule == null && !isRoot))
                    {
                        env.Output.Push();
                        node.AppendCSS(env);
                        rules.Add(env.Output.Pop());
                    }
                    else if (rule == null)
                    {
                        node.AppendCSS(env);
                    }
                }
            }

            var rulesetOutput = env.Output.Pop();

            // If this is the root node, we don't render
            // a selector, or {}.
            // Otherwise, only output if this ruleset has rules.
            if (isRoot)
            {
                env.Output.AppendMany(rules, env.Compress ? "" : "\n");
            }
            else
            {
                if (rules.Count > 0)
                {
                    paths.AppendCSS(env);

                    env.Output.Append(env.Compress ? "{" : " {\n  ");

                    env.Output.AppendMany(rules, env.Compress ? "" : "\n  ");
                    
                    env.Output.Append(env.Compress ? "}" : "\n}\n");

                }
            }

            env.Output.Append(rulesetOutput);
        }

        public override string ToString()
        {
            var format = "{0}{{{1}}}";
            return Selectors != null && Selectors.Count > 0
                       ? string.Format(format, Selectors.Select(s => s.ToCSS(new Env())).JoinStrings(""), Rules.Count)
                       : string.Format(format, "*", Rules.Count);
        }
    }
}