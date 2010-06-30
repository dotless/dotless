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
    }
}