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
        public int Index { get; set; }

        public Node Call(Env env, IEnumerable<Node> arguments)
        {
            Arguments = arguments.ToList();

            var node = Evaluate(env);
            node.Index = Index;
            return node;
        }

        protected abstract Node Evaluate(Env env);

        public override string ToString()
        {
            return string.Format("function '{0}'", Name.ToLowerInvariant());
        }
    }
}