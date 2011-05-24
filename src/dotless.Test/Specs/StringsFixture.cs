namespace dotless.Test.Specs
{
    using NUnit.Framework;

    public class StringsFixture : SpecFixtureBase
    {
        [Test]
        public void Strings()
        {
            AssertExpressionUnchanged(@"""http://son-of-a-banana.com""");
            AssertExpressionUnchanged(@"""~"" ""~""");
            AssertExpressionUnchanged(@"""#*%:&^,)!.(~*})""");
            AssertExpressionUnchanged(@"""""");
        }

        [Test]
        public void Comments()
        {
            AssertExpressionUnchanged(@"""/* hello */ // not-so-secret""");
        }

        [Test]
        public void SingleQuotes()
        {
            AssertExpressionUnchanged(@"""'"" ""'""");
            AssertExpressionUnchanged(@"'""""#!&""""'");
            AssertExpressionUnchanged(@"''");
        }

        [Test]
        public void EscapingQuotes()
        {
            AssertExpressionUnchanged(@"""\""""");
            AssertExpressionUnchanged(@"'\''");
            AssertExpressionUnchanged(@"'\'\""'");
        }

        [Test]
        public void EscapedWithTilde()
        {
            AssertExpression(@"DX.Transform.MS.BS.filter(opacity=50)", @"~""DX.Transform.MS.BS.filter(opacity=50)""");
        }

        [Test]
		[Ignore("Supported by less.js, not by dotless")]
        public void StringInterpolation1()
        {
			var input = @"
#interpolation {
  @var: '/dev';
  url: ""http://lesscss.org@{var}/image.jpg"";
}
";
			var expected = @"
#interpolation {
  url: ""http://lesscss.org/dev/image.jpg"";
}";

			AssertLess(input, expected);
        }

        [Test]
		[Ignore("Supported by less.js, not by dotless")]
        public void StringInterpolation2()
        {
			var input = @"
#interpolation {
  @var2: 256;
  url2: ""http://lesscss.org/image-@{var2}.jpg"";
}
";
			var expected = @"
#interpolation {
  url2: ""http://lesscss.org/image-256.jpg"";
}";

			AssertLess(input, expected);
        }

        [Test]
		[Ignore("Supported by less.js, not by dotless")]
        public void StringInterpolation3()
        {
			var input = @"
#interpolation {
  @var3: #456;
  url3: ""http://lesscss.org@{var3}"";
}
";
			var expected = @"
#interpolation {
  url3: ""http://lesscss.org#445566"";
}";

			AssertLess(input, expected);
        }

        [Test]
		[Ignore("Supported by less.js, not by dotless")]
        public void StringInterpolation4()
        {
			var input = @"
#interpolation {
  @var4: hello;
  url4: ""http://lesscss.org/@{var4}"";
}
";
			var expected = @"
#interpolation {
  url4: ""http://lesscss.org/hello"";
}";

			AssertLess(input, expected);
        }

        [Test]
		[Ignore("Supported by less.js, not by dotless")]
        public void StringInterpolation5()
        {
			var input = @"
#interpolation {
  @var5: 54.4px;
  url5: ""http://lesscss.org/@{var5}"";
}
";
			var expected = @"
#interpolation {
  url5: ""http://lesscss.org/54.4"";
}";

			AssertLess(input, expected);
        }

		[Test]
		[Ignore("Supported by less.js, not by dotless")]
		public void StringInterpolationMixMulClass()
		{
			var input = @"
.mix-mul (@a: green) {
    color: ~""@{a}"";
}
.mix-mul-class {
    .mix-mul(blue);
    .mix-mul(red);
    .mix-mul(blue);
    .mix-mul(orange);
}";
			var expected = @"
.mix-mul-class {
  color: blue;
  color: red;
  color: blue;
  color: orange;
}";
			AssertLess(input, expected);
		}

        [Test]
        public void BracesInQuotes()
        {
            AssertExpressionUnchanged(@"""{"" ""}""");
        }

        [Test]
        public void BracesInQuotesUneven()
        {
            AssertExpressionUnchanged(@"""{"" """"");
        }

        [Test]
        public void SemiColonInQuotes()
        {
            AssertExpressionUnchanged(@"';'");
        }

        [Test]
        public void CloseBraceInsideStringAfterQuoteInsideString()
        {
            var input = @"
#test {
  prop: ""'test'"";
  prop: ""}"";
}
";
            AssertLessUnchanged(input);
        }

        [Test]
        public void NewLineInStringNotSupported1()
        {
            // this was never supported in the parser but is now not supported in the tokenizer either
            var input = @"""
""";

            AssertExpressionError("Missing closing quote (\")", 0, input);
        }

        [Test]
        public void NewLineInStringNotSupported2()
        {
            // this was never supported in the parser but is now not supported in the tokenizer either
            var input = @"""\
""";

            AssertExpressionError("Missing closing quote (\")", 0, input);
        }

        [Test]
        public void NoEndDoubleQuote()
        {
            var input = @"
.cla { background-image: ""my-image.jpg; }

.clb { background-image: ""my-second-image.jpg""; }";

            AssertError(
                "Missing closing quote (\")",
                ".cla { background-image: \"my-image.jpg; }",
                1,
                25,
                input);
        }

        [Test]
        public void NoEndDoubleQuote2()
        {
            var input =
                @"
.cla { background-image: ""my-image.jpg; }

.clb { background-position: 12px 3px; }";

            AssertError(
                "Missing closing quote (\")",
                ".cla { background-image: \"my-image.jpg; }",
                1,
                25,
                input);
        }

        [Test]
        public void NoEndSingleQuote()
        {
            var input = @"
.cla { background-image: 'my-image.jpg; }

.clb { background-image: 'my-second-image.jpg'; }";

            AssertError(
                "Missing closing quote (')",
                ".cla { background-image: 'my-image.jpg; }",
                1,
                25,
                input);
        }

        [Test]
        public void NoEndSingleQuote2()
        {
            var input = @"
.cla { background-image: 'my-image.jpg; }

.clb { background-position: 12px 3px; }";

            AssertError(
                "Missing closing quote (')",
                ".cla { background-image: 'my-image.jpg; }",
                1,
                25,
                input);
        }

        [Test]
        public void NoEndSingleQuote3()
        {
            var input = @"
.cla { background-image: 'my-image.jpg; } /* comment

.clb { background-position: 12px 3px; }*/";

            AssertError(
                "Missing closing quote (')",
                ".cla { background-image: 'my-image.jpg; } /* comment",
                1,
                25,
                input);
        }

        [Test]
        public void NoEndSingleQuote4()
        {
            var input = @".cla { background-image: 'my-image.jpg; }";

            AssertError(
                "Missing closing quote (')",
                ".cla { background-image: 'my-image.jpg; }",
                1,
                25,
                input);
        }

        [Test]
        public void NoEndComment()
        {
            var input = @"
.cla { background-image: 'my-image.jpg'; } /* My comment starts here but isn't closed

.clb { background-image: 'my-second-image.jpg'; }";

            AssertError(
                "Missing closing comment",
                ".cla { background-image: 'my-image.jpg'; } /* My comment starts here but isn't closed",
                1,
                43,
                input);
        }
    }
}