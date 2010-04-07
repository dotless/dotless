using System.Collections.Generic;
using System.Linq;
using dotless.Exceptions;
using dotless.Infrastructure;

namespace dotless.Utils
{
  public class Guard
  {
    public static void ExpectNode<TNode>(Node node)
    {
      if (node is TNode)
        return;

      var message = string.Format("Expected '{0}', found '{1}'.", typeof(TNode).Name, node.GetType().Name);

      throw new ParsingException(message);
    }

    public static void ExpectNodes<TNode>(IEnumerable<Node> nodes)
    {
      if (nodes.All(n => n is TNode))
        return;

      var node = nodes.First(n => !(n is TNode));

      var message = string.Format("Expected '{0}', found '{1}'.", typeof(TNode).Name, node.GetType().Name);

      throw new ParsingException(message);
    }

    public static void ExpectNumArguments(IEnumerable<Node> arguments, int expected)
    {
      if(arguments.Count() == expected)
        return;

      var message = string.Format("Expected {0} arguments, found {1}.", expected, arguments.Count());

      throw new ParsingException(message);
    }
  }
}