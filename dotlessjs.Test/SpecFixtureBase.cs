using System.Collections.Generic;
using dotless.Infrastructure;
using dotless.Tree;
using NUnit.Framework;

namespace dotless.Tests
{
  public class SpecFixtureBase
  {
    protected static void AssertLess(string input, string expected)
    {
      AssertLess(input, expected, new Parser());
    }

    protected static void AssertLess(string input, string expected, Parser parser)
    {
      var output = ParseLess(input, parser).Trim();

      expected = expected.Trim().Replace("\r\n", "\n");

      Assert.That(output, Is.EqualTo(expected));
    }

    private static string ParseLess(string input)
    {
      return ParseLess(input, new Parser());
    }

    private static string ParseLess(string input, Parser parser)
    {;
      var tree = parser.Parse(input);
      return tree.ToCSS(new List<IEnumerable<Selector>>(), new Env());
    }
  }
}