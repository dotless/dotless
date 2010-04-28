using System;
using System.Collections.Generic;
using System.Linq;
using dotless.Infrastructure;
using dotless.Tree;

namespace dotless.Utils
{
  internal class NodeHelper
  {
    public static void ExpandNodes<TNode>(Env env, Ruleset ruleset)
      where TNode : Node
    {
      for (var i = 0; i < ruleset.Rules.Count; i++)
      {
        var node = ruleset.Rules[i];

        if (node is TNode)
        {
          ruleset.Rules.RemoveAt(i);
          ruleset.Rules.InsertRange(i, (IEnumerable<Node>) node.Evaluate(env));
        }
      }      
    }
  }
}