namespace dotless.Test.Specs
{
    using NUnit.Framework;

    public class OptimizationsFixture : SpecFixtureBase
    {
        [Test]
        public void DontStripComments()
        {
            string input = @"
/*
 *
 * Multiline Comment
 *
 */
";

            Optimisation = 0;
            AssertLessUnchanged(input);

            Optimisation = 1;
            AssertLessUnchanged(input);

            Optimisation = 2;
            AssertLessUnchanged(input);
        }

        [Test]
        public void Optimization0LeavesBlankLinesInComments()
        {
            string input = @"
/*

 * Multiline Comment

        
            

 */
";

            Optimisation = 0;
            AssertLessUnchanged(input);
        }

        [Test]
        public void Optimization1DoesNotRemoveWhitespaceLinesFromComments()
        {
            string input = @"
/*

 * Multiline Comment

        
            

 */
";

            Optimisation = 1;
            AssertLessUnchanged(input);
        }

        [Test]
        public void ErrorAfterCommentIsReportedOnCorrectLine()
        {
            var input =
                @"
/*
 * Comment
 */

.error { .mixin }
";

            Optimisation = 0;
            AssertError(".mixin is undefined", ".error { .mixin }", 5, 9, input);

            Optimisation = 1;
            AssertError(".mixin is undefined", ".error { .mixin }", 5, 9, input);

            Optimisation = 2;
            AssertError(".mixin is undefined", ".error { .mixin }", 5, 9, input);
        }

        [Test]
        public void SkipExcessiveSemicolon()
        {
            var input = @"
body {
  ;
  color: #123456;;
}
";
            var expected = @"
body {
  color: #123456;
}
";
            AssertLess(input, expected);
        }

        [Test]
        public void SkipEmptyRules()
        {
            var input = @"
body {
  /* before */ ; /* after */
}
";
            var expected = @"";
            AssertLess(input, expected);
        }
    }
}