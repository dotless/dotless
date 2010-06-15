namespace dotless.Test.Specs.Compression
{
    using NUnit.Framework;

    public class CommentsFixture : CompressedSpecFixtureBase
    {
        [Test]
        public void LineCommentGetsRemoved()
        {
            var input = "/* Comment */";
            var expected = "";

            AssertLess(input, expected);
        }
    }
}