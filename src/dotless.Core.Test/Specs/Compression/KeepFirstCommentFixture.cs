namespace dotless.Core.Test.Specs.Compression
{
    using NUnit.Framework;
    using dotless.Core.Parser.Infrastructure;

    public class KeepFirstCommentFixture : SpecFixtureBase
    {
        [SetUp]
        public void Setup()
        {
            DefaultEnv = () => new Env(null) { Compress = true, KeepFirstSpecialComment = true };
        }

        [Test]
        public void KeepsFirstCommentOnly1()
        {
            var input = "/** Comment *//** Comment*/";
            var expected = "/** Comment */";

            AssertLess(input, expected);
        }

        [Test]
        public void KeepsFirstCommentOnly2()
        {
            var input = @"/** Comment */
.mixin() {
  /** Comment*/
  foo: bar;
}
.foo {
  .mixin();
}
";
            var expected = @"/** Comment */.foo{foo:bar}";

            AssertLess(input, expected);
        }

        [Test]
        public void KeepsDoubleStarCommentOnly1()
        {
            var input = "/* Comment1 *//** Comment2 *//** Comment3 */";
            var expected = "/** Comment2 */";

            AssertLess(input, expected);
        }

        [Test]
        public void KeepsDoubleStarCommentOnly2()
        {
            var input = "// comment";
            var expected = "";

            AssertLess(input, expected);
        }

        [Test]
        public void KeepsDoubleStarCommentOnly3()
        {
            var input = @"/* Comment1 */
/*************
 * (c) msg   *
 *************//** Comment3 */";

            var expected = @"
/*************
 * (c) msg   *
 *************/";

            AssertLess(input, expected);
        }
    }
}