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
      AssertExpression("abc d", "formatstring('abc {0}', 'd', 'e')");
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

    [Test]
    public void FormatGradients()
    {
      var input = @"
#gradients {
  @from: #444;
  @to: #999;
  @ffgradient: ""-moz-linear-gradient(top, {0}, {1})"";
  @wkgradient: ""-webkit-gradient(linear,left top,left bottom,color-stop(0, {0}),color-stop(1, {1}))"";
  @iegradient: ""progid:DXImageTransform.Microsoft.gradient(startColorstr='{0}', endColorstr='{1}')"";
  @ie8gradient: ""\""progid:DXImageTransform.Microsoft.gradient(startColorstr='{0}', endColorstr='{1}')\"""";

  background-image: formatString(@ffgradient, @from, @to);  // FF3.6
  background-image: formatString(@wkgradient, @from, @to);  // Saf4+, Chrome
  filter:           formatString(@iegradient, @from, @to);  // IE6,IE7
  -ms-filter:       formatString(@ie8gradient, @from, @to); // IE8
}";

      var expected = @"
#gradients {
  background-image: -moz-linear-gradient(top, #444444, #999999);
  background-image: -webkit-gradient(linear,left top,left bottom,color-stop(0, #444444),color-stop(1, #999999));
  filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#444444', endColorstr='#999999');
  -ms-filter: ""progid:DXImageTransform.Microsoft.gradient(startColorstr='#444444', endColorstr='#999999')"";
}";

      AssertLess(input, expected);
    }
  }
}