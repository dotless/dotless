namespace dotless.Core.Test.Specs.Functions
{
    using System.Collections.Generic;
    using NUnit.Framework;

    public class FormatStringFixture : SpecFixtureBase
    {
        [Test]
        public void TestFormatStringInfo()
        {
            var info1 = "formatstring(string, args...) is not supported by less.js, so this will work but not compile with other less implementations." +
                @" You may want to consider using string interpolation (""@{variable}"") which does the same thing and is supported.";

            AssertExpressionLogMessage(info1, "formatstring('abc')");
            AssertExpressionLogMessage(info1, "formatstring('abc {0}', 'd')");
        }

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
        public void SubsequentArgumentsIgnored()
        {
            AssertExpression("abc", "formatstring('abc', 'd', 'e')");
        }

        [Test]
        public void ExceptionOnMissingArguments1()
        {
            Assert.Throws<dotless.Core.Exceptions.ParserException>(() => AssertExpression("abc", "formatstring('abc{0}')"));
        }

        [Test]
        public void ExceptionOnMissingArguments2()
        {
            Assert.Throws<dotless.Core.Exceptions.ParserException>(() => AssertExpression("abc", "formatstring('{2}abc')"));
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
            var input =
                @"
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

            var expected =
                @"
#gradients {
  background-image: -moz-linear-gradient(top, #444, #999);
  background-image: -webkit-gradient(linear,left top,left bottom,color-stop(0, #444),color-stop(1, #999));
  filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#444', endColorstr='#999');
  -ms-filter: ""progid:DXImageTransform.Microsoft.gradient(startColorstr='#444', endColorstr='#999')"";
}";

            AssertLess(input, expected);
        }

        [Test]
        public void EscapeFunction()
        {
            var input = @"
#built-in {
  escaped: e(""-Some::weird(#thing, y)"");
}
";
            var expected = @"
#built-in {
  escaped: -Some::weird(#thing, y);
}";

            AssertLess(input, expected);
        }

        [Test]
        public void ShortFormatFunction()
        {
            var input = @"
#built-in {
  @r: 32;
  format: %(""rgb(%d, %d, %d)"", @r, 128, 64);
}
";
            var expected = @"
#built-in {
  format: ""rgb(32, 128, 64)"";
}";

            AssertLess(input, expected);
        }

        [Test]
        public void ShortFormatFunctionAcceptingString()
        {
            var input = @"
#built-in {
  format-string: %(""hello %s"", ""world"");
}
";
            var expected = @"
#built-in {
  format-string: ""hello world"";
}";

            AssertLess(input, expected);
        }

        [Test]
        public void ShortFormatFunctionUrlEncode()
        {
            var input = @"
#built-in {
  format-url-encode: %('red is %A', #ff1000);
}
";
            var expected = @"
#built-in {
  format-url-encode: 'red is %23ff1000';
}";

            AssertLess(input, expected);
        }

        [Test]
        public void EscapeAndShortFormatFunction()
        {
            var input = @"
#built-in {
  @r: 32;
  eformat: e(%(""rgb(%d, %d, %d)"", @r, 128, 64));
}
";
            var expected = @"
#built-in {
  eformat: rgb(32, 128, 64);
}";

            AssertLess(input, expected);
        }
    }
}