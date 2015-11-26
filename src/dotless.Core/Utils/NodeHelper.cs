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

        public static void RecursiveExpandNodes<TNode>(Env env, Ruleset parentRuleset)
        where TNode : Node
        {
            
            env.Frames.Push(parentRuleset);

            for (var i = 0; i < parentRuleset.Rules.Count; i++)
            {
                var node = parentRuleset.Rules[i];

                if (node is TNode)
                {
                    var evaluated = node.Evaluate(env);
                    var nodes = evaluated as IEnumerable<Node>;
                    if (nodes != null)
                    {
                        parentRuleset.Rules.InsertRange(i + 1, nodes);
                        parentRuleset.Rules.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        parentRuleset.Rules[i] = evaluated;
                    }
                }
                else
                {
                    var ruleset = node as Ruleset;
                    if (ruleset != null && ruleset.Rules != null)
                    {
                        RecursiveExpandNodes<TNode>(env, ruleset);
                    }
                }
            }

            env.Frames.Pop();
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