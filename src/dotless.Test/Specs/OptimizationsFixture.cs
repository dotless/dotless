namespace dotless.Test.Specs
{
    using Core.Parser;
    using NUnit.Framework;

    public class OptimizationsFixture : SpecFixtureBase
    {
        [Test]
        public void Optimization0And1DontStripComments()
        {
            string input = @"
/*
 *
 * Multiline Comment
 *
 */
";

            AssertLessUnchanged(input, new Parser(0));
            AssertLessUnchanged(input, new Parser(1));
        }

        [Test]
        public void Optimization0LeavesBlankLinesInComments()
        {
            string input = @"
/*

 * Multiline Comment

        
            

 */
";

            AssertLessUnchanged(input, new Parser(0));
        }

        [Test]
        public void Optimization1RemovesWhitespaceLinesFromComments()
        {
            string input = @"
/*

 * Multiline Comment

        
            

 */
";

            string expected = @"
/*
 * Multiline Comment
 */
";

            AssertLess(input, expected, new Parser(1));
        }

        [Test]
        public void Optimization2AndGreaterStripsComments()
        {
            string input = @"
/*
 *
 * Multiline Comment
 *
 */
";

            var expected = "";

            AssertLess(input, expected, new Parser(2));
            AssertLess(input, expected, new Parser(3));
            AssertLess(input, expected, new Parser(4));
            // ...
            AssertLess(input, expected, new Parser(100));
        }
    }
}