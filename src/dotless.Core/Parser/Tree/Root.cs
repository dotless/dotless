using System.Collections.Generic;

namespace dotless.Core.Parser.Tree
{
    using System;
    using System.Linq;
    using Exceptions;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Plugins;
    using dotless.Core.Utils;

    public class Root : Ruleset
    {
        public Func<ParsingException, ParserException> Error { get; set; }

        public Root(NodeList rules, Func<ParsingException, ParserException> error) 
            : this(rules, error, null)
        {
        }

        protected Root(NodeList rules, Func<ParsingException, ParserException> error, Ruleset master) 
            : base(new NodeList<Selector>(), rules, master)
        {
            Error = error;
            IsRoot = true;
        }

        public override void AppendCSS(Env env)
        {
            try
            {
                if (Rules == null || !Rules.Any())
                    return;

                var evaluated = (Root) Evaluate(env);
                evaluated.Rules.InsertRange(0, evaluated.CollectImports().Cast<Node>());
                evaluated.AppendCSS(env, new Context());
            }
            catch (ParsingException e)
            {
                throw Error(e);
            }
        }


        /// <summary>
        /// Gather the import statements from this instance, remove them from the list of rules and return them.
        /// Used for hoisting imports to the top of the file.
        /// </summary>
        private IList<Import> CollectImports() {
            var imports = Rules.OfType<Import>().ToList();
            foreach (var import in imports) {
                Rules.Remove(import);
            }
            return imports;
        }

        private Root DoVisiting(Root node, Env env, VisitorPluginType pluginType)
        {
            return env.VisitorPlugins
                .Where(p => p.AppliesTo == pluginType)
                .Aggregate(node, (current, plugin) => 
                {
                    try
                    {
                        plugin.OnPreVisiting(env);
                        Root r = plugin.Apply(current);
                        plugin.OnPostVisiting(env);
                        return r;
                    }
                    catch (Exception ex)
                    {
                        string message = string.Format("Plugin '{0}' failed during visiting with error '{1}'", plugin.GetName(), ex.Message);
                        throw new ParserException(message, ex);
                    }
                });

        }

        public override Node Evaluate(Env env)
        {
            if(Evaluated) return this;

            try
            {
                env = env ?? new Env();

                env.Frames.Push(this);
                NodeHelper.ExpandNodes<Import>(env, Rules);
                env.Frames.Pop();

                var clone = new Root(new NodeList(Rules), Error, OriginalRuleset);

                clone = DoVisiting(clone, env, VisitorPluginType.BeforeEvaluation);

                clone.ReducedFrom<Root>(this);
                clone.EvaluateRules(env);
                clone.Evaluated = true;

                clone = DoVisiting(clone, env, VisitorPluginType.AfterEvaluation);
                return clone;
            }
            catch (ParsingException e)
            {
                throw Error(e);
            }
        }
    }
}