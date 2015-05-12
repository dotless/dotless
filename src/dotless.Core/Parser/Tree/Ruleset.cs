using System;
using dotless.Core.Exceptions;

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
            return (Rules ?? Enumerable.Empty<Node>()).OfType<Ruleset>().ToList();
        }

        public List<Closure> Find<TRuleset>(Env env, Selector selector, Ruleset self) where TRuleset : Ruleset
        {
            var context = new Context();
            context.AppendSelectors(new Context(), Selectors ?? new NodeList<Selector>());

            var namespacedSelectorContext = new Context();
            namespacedSelectorContext.AppendSelectors(context, new NodeList<Selector>(selector));

            var namespacedSelector =
                namespacedSelectorContext
                    .Select(selectors => new Selector(selectors.SelectMany(s => s.Elements)))
                    .First();

            return FindInternal(env, namespacedSelector, self, context).ToList();
        }

        private IEnumerable<Closure> FindInternal(Env env, Selector selector, Ruleset self, Context context)
        {
            if (!selector.Elements.Any())
            {
                return Enumerable.Empty<Closure>();
            }

            string selectorCss = selector.ToCSS(env);
            var key = selectorCss;
            if (_lookups.ContainsKey(key))
                return _lookups[key];

            self = self ?? this;
            var rules = new List<Closure>();

            var bestMatch = context.Select(selectors => new Selector(selectors.SelectMany(s => s.Elements)))
                .Where(m => m.Elements.IsSubsequenceOf(selector.Elements, ElementValuesEqual))
                .OrderByDescending(m => m.Elements.Count)
                .FirstOrDefault();

            if (bestMatch != null && bestMatch.Elements.Count == selector.Elements.Count)
            {
                // exact match, good to go
                rules.Add(new Closure { Context = new List<Ruleset> { this }, Ruleset = this });
            }


            var validRulesets = Rulesets().Where(rule =>
                {
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
                if (rule.Selectors == null)
                {
                    continue;
                }

                var childContext = new Context();
                childContext.AppendSelectors(context, rule.Selectors);

                var closures = rule.FindInternal(env, selector, self, childContext);
                foreach (var closure in closures)
                {
                    closure.Context.Insert(0, this);
                    rules.Add(closure);
                }

            }
            return _lookups[key] = rules;
        }

        private static bool ElementValuesEqual(Element e1, Element e2)
        {
            if (e1.Value == null && e2.Value == null)
            {
                return true;
            }

            if (e1.Value == null || e2.Value == null)
            {
                return false;
            }

            return string.Equals(e1.Value.Trim(), e2.Value.Trim());
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

            for (var i = 0; i < Selectors.Count; i++) {
                var evaluatedSelector = Selectors[i].Evaluate(env);
                var expandedSelectors = evaluatedSelector as IEnumerable<Selector>;
                if (expandedSelectors != null) {
                    Selectors.RemoveAt(i);
                    Selectors.InsertRange(i, expandedSelectors);
                } else {
                    Selectors[i] = evaluatedSelector as Selector;
                }
            }

            int mediaBlocks = env.MediaBlocks.Count;

            NodeHelper.ExpandNodes<Import>(env, Rules);
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

            for (var i = 0; i < Rules.Count; i++)
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

        protected Ruleset EvaluateRulesForFrame(Ruleset frame, Env context)
        {
            var newRules = new NodeList();

            foreach (var rule in Rules)
            {
                if (rule is MixinDefinition)
                {
                    var mixin = rule as MixinDefinition;
                    var parameters = Enumerable.Concat(mixin.Params, frame.Rules.Cast<Rule>());
                    newRules.Add(new MixinDefinition(mixin.Name, new NodeList<Rule>(parameters), mixin.Rules, mixin.Condition, mixin.Variadic));
                }
                else if (rule is Import)
                {
                    var potentiolNodeList = rule.Evaluate(context);
                    var nodeList = potentiolNodeList as NodeList;
                    if (nodeList != null)
                    {
                        newRules.AddRange(nodeList);
                    }
                    else
                    {
                        newRules.Add(potentiolNodeList);
                    }
                }
                else if (rule is Directive || rule is Media)
                {
                    newRules.Add(rule.Evaluate(context));
                }
                else if (rule is Ruleset)
                {
                    var ruleset = (rule as Ruleset);

                    newRules.Add(ruleset.Evaluate(context));
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

            return new Ruleset(Selectors, newRules);
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
                var exactExtension = env.FindExactExtension(finalString);
                if (exactExtension != null) {
                    exactExtension.IsMatched = true;
                    paths.AppendSelectors(context.Clone(), exactExtension.ExtendedBy);
                }

                var partials = env.FindPartialExtensions(local);
                if (partials != null) {
                    foreach (var partialExtension in partials) {
                        partialExtension.IsMatched = true;
                    }
                    paths.AppendSelectors(context.Clone(), partials.SelectMany(p => p.Replacements(finalString)));
                }

                bool newExactExtenders = exactExtension != null && exactExtension.ExtendedBy.Any(e => !e.IsReference);
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