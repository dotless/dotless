namespace dotless.Test.Specs.Compression
{
    using NUnit.Framework;

    public class CommentsFixture : CompressedSpecFixtureBase
    {
        [Test]
        public void BlockCommentGetsRemoved()
        {
            var input = "/* Comment */";
            var expected = "";

            AssertLess(input, expected);
        }

        [Test]
        public void DoubleStarBlockCommentGetsRemoved()
        {
            var input = "/** Comment */";
            var expected = "";

            AssertLess(input, expected);
        }

        [Test]
        public void CheckCommentsAreNotTakenToBeWhitespace1()
        {
            // IE7/IE8/FF/Chrome - All take a comment between selectors to mean as if the comment was not there
            //  e.g. it does not count toward the descendent selector. IE7 Quirks applies both .cls.cla and .cls .cla
            var input = @".cls/* COMMENT */.cla {background-image: url(pickture.asp);}";

            var expected = @".cls.cla{background-image:url(pickture.asp)}";

            AssertLess(input, expected);
        }

        [Test]
        public void CheckCommentsAreNotTakenToBeWhitespace2()
        {
            var input = @".cls/* COMMENT */ + /* COMMENT */.cla {background-image: url(pickture.asp);}";

            var expected = @".cls+.cla{background-image:url(pickture.asp)}";

            AssertLess(input, expected);
        }

        [Test]
        public void CheckCommentsAreNotObscuringWhitepsace1()
        {
            var input = @".cls /* COMMENT *//* COMMENT */.cla {background-image: url(pickture.asp);}";

            var expected = @".cls .cla{background-image:url(pickture.asp)}";

            AssertLess(input, expected);
        }

        [Test]
        public void CheckCommentsAreNotObscuringWhitepsace2()
        {
            var input = @".cls/* COMMENT */ /* COMMENT */.cla {background-image: url(pickture.asp);}";

            var expected = @".cls .cla{background-image:url(pickture.asp)}";

            AssertLess(input, expected);
        }

        [Test]
        public void CheckCommentsAreNotObscuringWhitepsace3()
        {
            var input = @".cls/* COMMENT *//* COMMENT */ .cla {background-image: url(pickture.asp);}";

            var expected = @".cls .cla{background-image:url(pickture.asp)}";

            AssertLess(input, expected);
        }

        [Test]
        public void CheckEmptyRuleSetsAreNotCreatedBecauseOfComments()
        {
            // inspired by https://github.com/cloudhead/less.js/issues/147
            var input = @"
.wrapper {
    /* the header */
    .header {
        font-weight: bold;
    }
    /* the footer */
    .footer {
        /* color: #fff; */
    }
}";
            var expected = @".wrapper .header{font-weight:bold}";

            AssertLess(input, expected);
        }
    }
}