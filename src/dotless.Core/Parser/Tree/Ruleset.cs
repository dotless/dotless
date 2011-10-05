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
        public NodeList Rules { get; set; }

        /// <summary>
        ///  The original Ruleset this was cloned from during evaluation
        ///   (may == this if this is the orginal)
        /// </summary>
        public Ruleset OriginalRuleset
        {
            get;
            set;
        }

        private Dictionary<string, List<Closure>> _lookups;

        public Ruleset(NodeList<Selector> selectors, NodeList rules)
            : this(selectors, rules, null)
        {
        }

        protected Ruleset(NodeList<Selector> selectors, NodeList rules, Ruleset originalRuleset)
            : this()
        {
            Selectors = selectors;
            Rules = rules;
            OriginalRuleset = originalRuleset ?? this;
        }

        protected Ruleset()
        {
            _lookups = new Dictionary<string, List<Closure>>();
            OriginalRuleset = this;
        }

        /// <summary>
        ///  returns whether this rulset is equal to or cloned from another node
        /// </summary>
        public bool IsEqualOrClonedFrom(Node node)
        {
            Ruleset ruleset = node as Ruleset;
            if (ruleset)
            {
                return IsEqualOrClonedFrom(ruleset);
            }
            return false;
        }

        /// <summary>
        ///  returns whether this rulset is equal to or cloned from another ruleset
        /// </summary>
        public bool IsEqualOrClonedFrom(Ruleset ruleset)
        {
            return ruleset.OriginalRuleset == OriginalRuleset;
        }

        public Rule Variable(string name, Node startNode)
        {
            Ruleset startNodeRuleset = startNode as Ruleset;

            return Rules
                .TakeWhile(r => r != startNode && (startNodeRuleset == null || !startNodeRuleset.IsEqualOrClonedFrom(r)))
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
            // create a clone so it is non destructive
            Ruleset clone = new Ruleset(Selectors, new NodeList(Rules), this.OriginalRuleset)
                .ReducedFrom<Ruleset>(this);
            clone.EvaluateRules(env);
            return clone;
        }

        /// <summary>
        ///  Evaluate Rules. Must only be run on a copy of the ruleset otherwise it will
        ///  overwrite defintions that might be required later..
        /// </summary>
        protected void EvaluateRules(Env env)
        {
            env.Frames.Push(this);

            NodeHelper.ExpandNodes<MixinCall>(env, this.Rules);

            for (var i = 0; i < Rules.Count; i++)
            {
                Rules[i] = Rules[i].Evaluate(env);
            }

            env.Frames.Pop();
        }

        public override void AppendCSS(Env env)
        {
            if (!Rules.Any())
                return;

            ((Ruleset)Evaluate(env))
                .AppendCSS(env, new Context());
        }

        /// <summary>
        ///  Append the rules in a {} block. Used by sub classes.
        /// </summary>
        protected void AppendRules(Env env)
        {
            if (env.Compress && Rules.Count == 0)
            {
                return;
            }

            env.Output
                .Append(env.Compress ? "{" : " {\n")

                .Push()
                .AppendMany(Rules, "\n")
                .Trim()
                .Indent(env.Compress ? 0 : 2)
                .PopAndAppend();

            if (env.Compress)
            {
                env.Output.TrimRight(';');
            }

            env.Output.Append(env.Compress ? "}" : "\n}");
        }

        protected virtual void AppendCSS(Env env, Context context)
        {
            var css = new List<string>(); // The CSS output
            var rules = new List<StringBuilder>(); // node.Ruleset instances
            int nonCommentRules = 0;
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
                if (comment != null && comment.Silent)
                    continue;

                var ruleset = node as Ruleset;
                if (ruleset != null)
                {
                    ruleset.AppendCSS(env, paths);
                }
                else
                {
                    var rule = node as Rule;
                    if ((rule != null && !rule.Variable) || (rule == null && !isRoot))
                    {
                        if (comment == null)
                            nonCommentRules++;

                        env.Output.Push()
                            .Append(node);
                        rules.Add(env.Output.Pop());
                    }
                    else if (rule == null)
                    {
                        env.Output.Append(node);
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
                if (nonCommentRules > 0)
                {
                    paths.AppendCSS(env);

                    env.Output.Append(env.Compress ? "{" : " {\n  ");

                    env.Output.AppendMany(rules, env.Compress ? "" : "\n  ");

                    if (env.Compress)
                    {
                        env.Output.TrimRight(';');
                    }

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