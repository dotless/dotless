namespace dotless.Core.Parser.Functions
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Infrastructure.Nodes;
    using dotless.Core.Loggers;

    public abstract class Function
    {
        public string Name { get; set; }
        protected List<Node> Arguments { get; set; }
        public ILogger Logger { get; set; }
        public NodeLocation Location { get; set; }

        public Node Call(Env env, IEnumerable<Node> arguments)
        {
            Arguments = arguments.ToList();

            var node = Evaluate(env);
            node.Location = Location;
            return node;
        }

        protected abstract Node Evaluate(Env env);

        public override string ToString()
        {
            return string.Format("function '{0}'", Name.ToLowerInvariant());
        }

        /// <summary>
        ///  Warns that a function is not supported by less.js
        /// </summary>
        /// <param name="functionPattern">unsupported pattern function call e.g. alpha(color number)</param>
        protected void WarnNotSupportedByLessJS(string functionPattern)
        {
            WarnNotSupportedByLessJS(functionPattern, null, null);
        }

        /// <summary>
        ///  Warns that a function is not supported by less.js
        /// </summary>
        /// <param name="functionPattern">unsupported pattern function call e.g. alpha(color number)</param>
        /// <param name="replacementPattern">Replacement pattern function call e.g. fadein(color, number)</param>
        protected void WarnNotSupportedByLessJS(string functionPattern, string replacementPattern)
        {
            WarnNotSupportedByLessJS(functionPattern, replacementPattern, null);
        }

        /// <summary>
        ///  Warns that a function is not supported by less.js
        /// </summary>
        /// <param name="functionPattern">unsupported pattern function call e.g. alpha(color number)</param>
        /// <param name="replacementPattern">Replacement pattern function call e.g. fadein(color, number)</param>
        /// <param name="extraInfo">Extra information to put on the end.</param>
        protected void WarnNotSupportedByLessJS(string functionPattern, string replacementPattern, string extraInfo)
        {
            if (string.IsNullOrEmpty(replacementPattern))
            {
                Logger.Info("{0} is not supported by less.js, so this will work but not compile with other less implementations.{1}", functionPattern, extraInfo);
            }
            else
            {
                Logger.Warn("{0} is not supported by less.js, so this will work but not compile with other less implementations." +
                    " You may want to consider using {1} which does the same thing and is supported.{2}", functionPattern, replacementPattern, extraInfo);
            }
        }
    }
}