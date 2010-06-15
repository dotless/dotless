namespace dotless.Core.Parser.Functions
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Infrastructure.Nodes;

    public abstract class Function
    {
        public string Name { get; set; }
        protected List<Node> Arguments { get; set; }

        public Node Call(Env env, IEnumerable<Node> arguments)
        {
            Arguments = arguments.ToList();

            return Evaluate(env);
        }

        protected abstract Node Evaluate(Env env);

        public override string ToString()
        {
            return string.Format("function '{0}'", Name.ToLowerInvariant());
        }
    }
}