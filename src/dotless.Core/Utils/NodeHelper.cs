using dotless.Core.Parser.Tree;

namespace dotless.Core.Utils
{
    using System.Collections.Generic;
    using Parser.Infrastructure;
    using Parser.Infrastructure.Nodes;

    internal class NodeHelper
    {
        public static void ExpandNodes<TNode>(Env env, NodeList rules)
            where TNode : Node
        {
            for (var i = 0; i < rules.Count; i++)
            {
                var node = rules[i];

                if (node is TNode)
                {
                    var evaluated = node.Evaluate(env);
                    var nodes = evaluated as IEnumerable<Node>;
                    if (nodes != null)
                    {
                        rules.InsertRange(i + 1, nodes);
                        rules.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        rules[i] = evaluated;
                    }
                }
            }
        }

        public static void RecursiveExpandNodes<TNode>(Env env, NodeList rules)
        where TNode : Node
        {
            for (var i = 0; i < rules.Count; i++)
            {
                var node = rules[i];

                if (node is TNode)
                {
                    var evaluated = node.Evaluate(env);
                    var nodes = evaluated as IEnumerable<Node>;
                    if (nodes != null)
                    {
                        rules.InsertRange(i + 1, nodes);
                        rules.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        rules[i] = evaluated;
                    }
                }
                else
                {
                    var ruleset = node as Ruleset;
                    if (ruleset != null && ruleset.Rules != null)
                    {
                        RecursiveExpandNodes<TNode>(env, ruleset.Rules);
                    }
                }
            }
        }

        public static IEnumerable<Node> NonDestructiveExpandNodes<TNode>(Env env, NodeList rules)
            where TNode : Node
        {
            foreach (var node in rules)
            {
                if (node is TNode)
                {
                    var expandedNodes = (IEnumerable<Node>)node.Evaluate(env);

                    foreach (var expandedNode in expandedNodes)
                    {
                        yield return expandedNode;
                    }
                }
                else
                {
                    yield return node;
                }
            }
        }
    }
}