namespace dotless.Test.Unit.Engine
{
    using NUnit.Framework;
    using dotless.Core.Exceptions;

    public class ParserExceptions : SpecFixtureBase
    {

        [Test, ExpectedException(typeof(ParserException), ExpectedMessage = @"
Missing closing quote ("") on line 3:", MatchType = MessageMatch.Contains)]
        public void NewLineInStringNotSupported1()
        {
            // this was never supported in the parser but is now not supported in the tokenizer either
            AssertExpressionUnchanged(@"""
""");
            AssertExpressionUnchanged(@"""\
""");
        }

        [Test, ExpectedException(typeof(ParserException), ExpectedMessage = @"
Missing closing quote ("") on line 3:", MatchType = MessageMatch.Contains)]
        public void NewLineInStringNotSupported2()
        {
            // this was never supported in the parser but is now not supported in the tokenizer either
            AssertExpressionUnchanged(@"""\
""");
        }

        [Test, ExpectedException(typeof(ParserException), ExpectedMessage = @"
Missing closing quote ("") on line 1:", MatchType = MessageMatch.Contains)]
        public void NoEndDoubleQuote()
        {
            var input =
                @"
.cla { background-image: ""my-image.jpg; }

.clb { background-image: ""my-second-image.jpg""; }";

            AssertLessUnchanged(input);
        }

        [Test, ExpectedException(typeof(ParserException), ExpectedMessage = @"
Missing closing quote ("") on line 1", MatchType = MessageMatch.Contains)]
        public void NoEndDoubleQuote2()
        {
            var input =
                @"
.cla { background-image: ""my-image.jpg; } 

.clb { background-position: 12px 3px; }";

            AssertLessUnchanged(input);
        }

        [Test, ExpectedException(typeof(ParserException), ExpectedMessage = @"
Missing closing quote (') on line 1:", MatchType = MessageMatch.Contains)]
        public void NoEndSingleQuote()
        {
            var input =
                @"
.cla { background-image: 'my-image.jpg; }

.clb { background-image: 'my-second-image.jpg'; }";

            AssertLessUnchanged(input);
        }

        [Test, ExpectedException(typeof(ParserException), ExpectedMessage = @"
Missing closing quote (') on line 1:", MatchType = MessageMatch.Contains)]
        public void NoEndSingleQuote2()
        {
            var input =
                @"
.cla { background-image: 'my-image.jpg; }

.clb { background-position: 12px 3px; }";

            AssertLessUnchanged(input);
        }

        [Test, ExpectedException(typeof(ParserException), ExpectedMessage = @"
Missing closing quote (') on line 1:", MatchType = MessageMatch.Contains)]
        public void NoEndSingleQuote3()
        {
            var input =
                @"
.cla { background-image: 'my-image.jpg; } /* comment

.clb { background-position: 12px 3px; }*/";

            AssertLessUnchanged(input);
        }

        [Test, ExpectedException(typeof(ParserException), ExpectedMessage = @"
Missing closing quote (') on line 1:", MatchType = MessageMatch.Contains)]
        public void NoEndSingleQuote4()
        {
            var input = @"
.cla { background-image: 'my-image.jpg; }";

            AssertLessUnchanged(input);
        }

        [Test, ExpectedException(typeof(ParserException), ExpectedMessage = @"
Missing closing comment on line 1:", MatchType = MessageMatch.Contains)]
        public void NoEndComment()
        {
            var input =
                @".cla { background-image: 'my-image.jpg'; } /* My comment starts here but isn't closed

.clb { background-image: 'my-second-image.jpg'; }";

            AssertLessUnchanged(input);
        }
    }
}