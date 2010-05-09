using System.Collections.Generic;
using dotless.Exceptions;
using dotless.Infrastructure;

namespace dotless.Utils
{
  public static class Guard
  {
    public static void Expect(string expected, string actual, object @in)
    {
      if (actual == expected)
        return;

      var message = string.Format("Expected '{0}' in {1}, found '{2}'", expected, @in, actual);

      throw new ParsingException(message);
    }
    
    public static void ExpectNode<TExpected>(Node actual, object @in) where TExpected : Node
    {
      if (actual is TExpected)
        return;

      var expected = typeof(TExpected).Name.ToLowerInvariant();

      var message = string.Format("Expected {0} in {1}, found {2}", expected, @in, actual.ToCSS());

      throw new ParsingException(message);
    }

    public static void ExpectAllNodes<TExpected>(IEnumerable<Node> actual, object @in) where TExpected : Node
    {
      foreach (var node in actual)
      {
        ExpectNode<TExpected>(node, @in);
      }
    }


    public static void ExpectNumArguments(int expected, int actual, object @in)
    {
      if (actual == expected)
        return;

      var message = string.Format("Expected {0} arguments in {1}, found {2}", expected, @in, actual);

      throw new ParsingException(message);
    }

    public static void ExpectMinArguments(int expected, int actual, object @in)
    {
      if (actual >= expected)
        return;

      var message = string.Format("Expected at least {0} arguments in {1}, found {2}", expected, @in, actual);

      throw new ParsingException(message);
    }

    public static void ExpectMaxArguments(int expected, int actual, object @in)
    {
      if (actual <= expected)
        return;

      var message = string.Format("Expected at most {0} arguments in {1}, found {2}", expected, @in, actual);

      throw new ParsingException(message);
    }
  }
}
