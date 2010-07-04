namespace dotless.Core.Utils
{
    using System.Collections.Generic;
    using Parser.Infrastructure;
    using Parser.Infrastructure.Nodes;

    internal class NodeHelper
    {
        public static void ExpandNodes<TNode>(Env env, List<Node> rules)
            where TNode : Node
        {
            for (var i = 0; i < rules.Count; i++)
            {
                var node = rules[i];

                if (node is TNode)
                {
                    rules.RemoveAt(i);

                    rules.InsertRange(i, (IEnumerable<Node>) node.Evaluate(env));
                }
            }
        }
    }
}