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