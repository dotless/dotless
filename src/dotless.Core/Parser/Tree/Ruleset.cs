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
        public bool MultiMedia { get; set; }

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

        public List<Closure> Find<TRuleset>(Env env, Selector selector, Ruleset self) where TRuleset : Ruleset
        {
            self = self ?? this;
            var rules = new List<Closure>();

            var key = typeof(TRuleset).ToString() + ":" + selector.ToCSS(env);
            if (_lookups.ContainsKey(key))
                return _lookups[key];

            var validRulesets = Rulesets().Where(rule =>
                {
                    if (!typeof(TRuleset).IsAssignableFrom(rule.GetType()))
                        return false;

                    if (rule != self)
                        return true;

                    MixinDefinition mixinRule = rule as MixinDefinition;

                    if (mixinRule != null)
                    {
                        return mixinRule.Condition != null;
                    }

                    return false;
                });

            foreach (var rule in validRulesets)
            {
                if (rule.Selectors && rule.Selectors.Any(selector.Match))
                {
                    if ((selector.Elements.Count == 1) || rule.Selectors.Any(s => s.ToCSS(new Env(null)) == selector.ToCSS(new Env(null))))
                        rules.Add(new Closure { Ruleset = rule, Context = new List<Ruleset> { rule } });
                    else if (selector.Elements.Count > 1)
                    {
                        var remainingSelectors = new Selector(new NodeList<Element>(selector.Elements.Skip(1)));
                        var closures = rule.Find<Ruleset>(env, remainingSelectors, self);

                        foreach (var closure in closures)
                        {
                            closure.Context.Insert(0, rule);
                        }

                        rules.AddRange(closures);
                    }
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
            if (Evaluated) return this;

            // create a clone so it is non destructive
            var clone = Clone().ReducedFrom<Ruleset>(this);

            clone.EvaluateRules(env);
            clone.Evaluated = true;

            return clone;
        }

        private Ruleset Clone() {
            return new Ruleset(new NodeList<Selector>(Selectors), new NodeList(Rules), OriginalRuleset) {
                IsReference = IsReference
            };
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

            int mediaBlocks = env.MediaBlocks.Count;

            NodeHelper.ExpandNodes<MixinCall>(env, Rules);

            foreach (var r in Rules.OfType<Extend>().ToArray())
            {
                foreach (var s in this.Selectors)
                {
                    //If we're in a media block, then extenders can only apply to that media block
                    if (env.MediaPath.Any())
                    {
                        env.MediaPath.Peek().AddExtension(s, (Extend) r.Evaluate(env), env);
                    }
                    else //Global extend
                    {
                        env.AddExtension(s, (Extend) r.Evaluate(env), env);
                    }
                }
                
                Rules.Remove(r);
            }

            for (var i = Rules.Count - 1; i >= 0; i--)
            {
                Rules[i] = Rules[i].Evaluate(env);
            }

            // if media blocks are inside this ruleset we have to "catch" the bubbling and
            // make sure they get this rulesets selectors
            for (var j = mediaBlocks; j < env.MediaBlocks.Count; j++)
            {
                env.MediaBlocks[j].BubbleSelectors(Selectors);
            }

            env.Frames.Pop();
        }

        public override void AppendCSS(Env env)
        {
            if (Rules == null || !Rules.Any())
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
            var rules = new List<StringBuilder>(); // node.Ruleset instances
            int nonCommentRules = 0;
            var paths = new Context(); // Current selectors

            if (!IsRoot)
            {
                if (!env.Compress && env.Debug && Location != null)
                {
                    env.Output.Append(string.Format("/* {0}:L{1} */\n", Location.FileName, Zone.GetLineNumber(Location)));
                }
                paths.AppendSelectors(context, Selectors);
            }

            env.Output.Push();

            bool hasNonReferenceChildRulesets = false;
            foreach (var node in Rules)
            {
                if (node.IgnoreOutput())
                    continue;

                var comment = node as Comment;
                if (comment != null && !comment.IsValidCss)
                    continue;

                var ruleset = node as Ruleset;
                if (ruleset != null)
                {
                    ruleset.AppendCSS(env, paths);
                    if (!ruleset.IsReference)
                    {
                        hasNonReferenceChildRulesets = true;
                    }
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

            var hasExtenders = AddExtenders(env, context, paths);
            if (hasExtenders)
            {
                IsReference = false;
            }

            // If this is the root node, we don't render
            // a selector, or {}.
            // Otherwise, only output if this ruleset has rules.
            if (!IsReference) {
                if (IsRoot) {
                    env.Output.AppendMany(rules, env.Compress ? "" : "\n");
                } else {
                    if (nonCommentRules > 0) {
                        paths.AppendCSS(env);

                        env.Output.Append(env.Compress ? "{" : " {\n  ");

                        env.Output.AppendMany(rules.ConvertAll(stringBuilder => stringBuilder.ToString()).Distinct(),
                            env.Compress ? "" : "\n  ");

                        if (env.Compress) {
                            env.Output.TrimRight(';');
                        }

                        env.Output.Append(env.Compress ? "}" : "\n}\n");
                    }
                }
            }

            if (!IsReference || hasNonReferenceChildRulesets)
            {
                env.Output.Append(rulesetOutput);
            }
        }

        private bool AddExtenders(Env env, Context context, Context paths) {
            bool hasNonReferenceExtenders = false;
            foreach (var s in Selectors.Where(s => s.Elements.First().Value != null)) {
                var local = context.Clone();
                local.AppendSelectors(context, new[] {s});
                var finalString = local.ToCss(env);
                var extensions = env.FindExactExtension(finalString);
                if (extensions != null) {
                    paths.AppendSelectors(context.Clone(), extensions.ExtendedBy);
                }

                var partials = env.FindPartialExtensions(local);
                if (partials != null) {
                    paths.AppendSelectors(context.Clone(), partials.SelectMany(p => p.Replacements(finalString)));
                }

                bool newExactExtenders = extensions != null && extensions.ExtendedBy.Any(e => !e.IsReference);
                bool newPartialExtenders = partials != null && partials.Any(p => p.ExtendedBy.Any(e => !e.IsReference));

                hasNonReferenceExtenders = hasNonReferenceExtenders || newExactExtenders || newPartialExtenders;
            }
            return hasNonReferenceExtenders;
        }

        public override string ToString()
        {
            var format = "{0}{{{1}}}";
            return Selectors != null && Selectors.Count > 0
                       ? string.Format(format, Selectors.Select(s => s.ToCSS(new Env(null))).JoinStrings(""), Rules.Count)
                       : string.Format(format, "*", Rules.Count);
        }
    }
}