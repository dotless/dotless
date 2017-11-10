namespace dotless.Core.Test.Unit.Engine
{
    using NUnit.Framework;

    public class CommentBug : SpecFixtureBase
    {

        [Test]
        public void Block_comment_with_multiple_asterisks_before_closing_slash_does_not_cause_tokenizer_error()
        {
            var input =
                @"@xxx: yellow; 
/* Block comment ********************/ 
body 
{ 
    background-color: @xxx; 
    /* Another block comment */ 
}";

            var expected =
                @"/* Block comment ********************/

body {
  background-color: yellow;
  /* Another block comment */

}";

            AssertLess(input, expected);
        }
    }
}