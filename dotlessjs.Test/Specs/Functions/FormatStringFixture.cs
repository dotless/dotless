using System.Collections.Generic;
using NUnit.Framework;

namespace dotless.Tests.Specs.Functions
{
  public class FormatStringFixture : SpecFixtureBase
  {
    [Test]
    public void NoFormatting()
    {
      AssertExpression("abc", "formatstring('abc')");
      AssertExpression("abc", "formatstring(\"abc\")");
    }

    [Test]
    public void EscapesQuotes()
    {
      AssertExpression("'abc'", @"formatstring('\'abc\'')");
      AssertExpression("\"abc\"", @"formatstring('\""abc\""')");
    }

    [Test]
    public void SubsequentArgumentssIgnored()
    {
      AssertExpression("abc", "formatstring('abc', 'd', 'e')");
    }

    [Test]
    public void SimpleFormatting()
    {
      AssertExpression("abc d", "formatstring('abc {0}', 'd', 'e')");         // Currently unable to have "{" inside a string
      AssertExpression("abc d e", "formatstring('abc {0} {1}', 'd', 'e')");
      AssertExpression("abc e d", "formatstring('abc {1} {0}', 'd', 'e')");
    }

    [Test]
    public void FormattingWithVariables()
    {
      var variables = new Dictionary<string, string> { { "x", "'def'" }, { "y", "'ghi'" }, { "z", @"'\'jkl\''" } };

      AssertExpression("abc def ghi", "formatstring('abc {0} {1}', @x, @y)", variables);
      AssertExpression("abc def ghi 'jkl'", "formatstring('abc {0} {1} {2}', @x, @y, @z)", variables);
    }

  }
}