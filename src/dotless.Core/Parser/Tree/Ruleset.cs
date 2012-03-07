namespace dotless.Core.Parser.Tree
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Utils;
    using Plugins;

    public class Ruleset : Node
    {
        public NodeList<Selector> Selectors { get; set; }
        public NodeList Rules { get; set; }
        public bool Evaluated { get; protected set; }
        public bool IsRoot { get; set; }

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

        public virtual MixinMatch MatchArguments(List<NamedArgument> arguments, Env env)
        {
            return (arguments == null || arguments.Count == 0) ? MixinMatch.Pass : MixinMatch.ArgumentMismatch;
        }

        public override Node Evaluate(Env env)
        {
            if(Evaluated) return this;

            // create a clone so it is non destructive
            var clone = new Ruleset(new NodeList<Selector>(Selectors), new NodeList(Rules), OriginalRuleset).ReducedFrom<Ruleset>(this);

            clone.EvaluateRules(env);
            clone.Evaluated = true;

            return clone;
        }

        public override void Accept(IVisitor visitor)
        {
            Selectors = VisitAndReplace(Selectors, visitor);

            Rules = VisitAndReplace(Rules, visitor);
        }

        /// <summary>
        ///  Evaluate Rules. Must only be run on a copy of the ruleset otherwise it will
        ///  overwrite defintions that might be required later..
        /// </summary>
        protected void EvaluateRules(Env env)
        {
            env.Frames.Push(this);

            for (var i = 0; i < Selectors.Count; i++)
            {
                Selectors[i] = Selectors[i].Evaluate(env) as Selector;
            }

            NodeHelper.ExpandNodes<MixinCall>(env, Rules);

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

            ((Ruleset) Evaluate(env)).AppendCSS(env, new Context());
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

        public virtual void AppendCSS(Env env, Context context)
        {
            var css = new List<string>(); // The CSS output
            var rules = new List<StringBuilder>(); // node.Ruleset instances
            int nonCommentRules = 0;
            var paths = new Context(); // Current selectors

            if (!IsRoot)
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

                    if (rule && rule.Variable)
                        continue;

                    if (!IsRoot)
                    {
                        if (!comment)
                            nonCommentRules++;

                        env.Output.Push()
                            .Append(node);
                        rules.Add(env.Output.Pop());
                    }
                    else
                    {
                        env.Output
                            .Append(node);

                        if (!env.Compress)
                        {
                            env.Output
                                .Append("\n");
                        }
                    }
                }
            }

            var rulesetOutput = env.Output.Pop();

            // If this is the root node, we don't render
            // a selector, or {}.
            // Otherwise, only output if this ruleset has rules.
            if (IsRoot)
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