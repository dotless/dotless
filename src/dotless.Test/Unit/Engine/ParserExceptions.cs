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

        [Test, ExpectedException(typeof(ParserException), ExpectedMessage=@"
Missing closing quote ("") on line 1:", MatchType=MessageMatch.Contains)]
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
        public void NoEndSingleQuote4 ()
        {
            var input = @"
.cla { background-image: 'my-image.jpg; }";
            
            AssertLessUnchanged (input);
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

        [Test, ExpectedException(typeof(ParserException), ExpectedMessage = @"Comments are not supported in this location", MatchType = MessageMatch.Contains)]
        [Ignore("Would be nice to detect, but too difficult at the moment - probably easier to fix")]
        public void CommentNotSupported1 ()
        {
            var input = @".cla/*comment*/.clb{ background-image: 'my-image.jpg'; }";

            AssertLessUnchanged (input);
        }


        [Test, ExpectedException(typeof(ParserException), ExpectedMessage = @"Comments are not supported in this location", MatchType = MessageMatch.Contains)]
        [Ignore("Would be nice to detect, but too difficult at the moment - probably easier to fix")]
        public void CommentNotSupported3 ()
        {
            var input = @".cla .clb{ background-image/*comment*/: 'my-image.jpg'; }";

            AssertLessUnchanged (input);
        }

        [Test, ExpectedException(typeof(ParserException), ExpectedMessage = @"Comments are not supported in this location", MatchType = MessageMatch.Contains)]
        [Ignore("Would be nice to detect, but too difficult at the moment - probably easier to fix")]
        public void CommentNotSupported4 ()
        {
            var input = @".cla .clb{ background-image:/*comment*/ 'my-image.jpg'; }";
            
            AssertLessUnchanged (input);
        }


        [Test, ExpectedException(typeof(ParserException), ExpectedMessage = @"Comments are not supported in this location", MatchType = MessageMatch.Contains)]
        [Ignore("Would be nice to detect, but too difficult at the moment - probably easier to fix")]
        public void CommentNotSupported5 ()
        {
            var input = @".cla .clb{ background-image: 'my-image.jpg'/*comment*/; }";

            AssertLessUnchanged (input);
        }

        [Test, ExpectedException(typeof(ParserException), ExpectedMessage = @"Comments are not supported in this location", MatchType = MessageMatch.Contains)]
        [Ignore("Would be nice to detect, but too difficult at the moment - probably easier to fix")]
        public void CommentNotSupported6 ()
        {
            var input = @"@a/*comment*/ : 12 + 4;";
            
            AssertLessUnchanged (input);
        }

        [Test, ExpectedException(typeof(ParserException), ExpectedMessage = @"Comments are not supported in this location", MatchType = MessageMatch.Contains)]
        [Ignore("Would be nice to detect, but too difficult at the moment - probably easier to fix")]
        public void CommentNotSupported7 ()
        {
            var input = @"@a : /*comment*/12 + 4;";

            AssertLessUnchanged (input);
        }

        [Test, ExpectedException(typeof(ParserException), ExpectedMessage = @"Comments are not supported in this location", MatchType = MessageMatch.Contains)]
        [Ignore("Would be nice to detect, but too difficult at the moment - probably easier to fix")]
        public void CommentNotSupported8 ()
        {
            var input = @"@a : 12 /*comment*/+ 4;";

            AssertLessUnchanged (input);
        }

        [Test, ExpectedException(typeof(ParserException), ExpectedMessage = @"Comments are not supported in this location", MatchType = MessageMatch.Contains)]
        [Ignore("Would be nice to detect, but too difficult at the moment - probably easier to fix")]
        public void CommentNotSupported9 ()
        {
            var input = @"@a : 12 + /*comment*/4;";

            AssertLessUnchanged (input);
        }

        [Test, ExpectedException(typeof(ParserException), ExpectedMessage = @"Comments are not supported in this location", MatchType = MessageMatch.Contains)]
        [Ignore("Would be nice to detect, but too difficult at the moment - probably easier to fix")]
        public void CommentNotSupported10 ()
        {
            var input = @"@a : 12 + 4/*comment*/;";

            AssertLessUnchanged (input);
        }

        [Test, ExpectedException(typeof(ParserException), ExpectedMessage = @"Comments are not supported in this location", MatchType = MessageMatch.Contains)]
        [Ignore("Would be nice to detect, but too difficult at the moment - probably easier to fix")]
        public void CommentNotSupported11 ()
        {
            var input = @".clb/*comment*/(@a) { background-image: url(""here.jpg"");}";
            
            AssertLessUnchanged (input);
        }

        [Test, ExpectedException(typeof(ParserException), ExpectedMessage = @"Comments are not supported in this location", MatchType = MessageMatch.Contains)]
        public void CommentNotSupported12 ()
        {
            var input = @".clb (/*comment*/@a) { background-image: url(""here.jpg"");}";

            AssertLessUnchanged (input);
        }

        [Test, ExpectedException(typeof(ParserException), ExpectedMessage = @"Comments are not supported in this location", MatchType = MessageMatch.Contains)]
        public void CommentNotSupported13 ()
        {
            var input = @".clb (@a/*comment*/) { background-image: url(""here.jpg"");}";

            AssertLessUnchanged (input);
        }

        [Test, ExpectedException(typeof(ParserException), ExpectedMessage = @"Comments are not supported in this location", MatchType = MessageMatch.Contains)]
        public void CommentNotSupported14 ()
        {
            var input = @".clb (@a,/*comment*/@b) { background-image: url(""here.jpg"");}";
            
            AssertLessUnchanged (input);
        }

        [Test, ExpectedException(typeof(ParserException), ExpectedMessage = @"Comments are not supported in this location", MatchType = MessageMatch.Contains)]
        public void CommentNotSupported15 ()
        {
            var input = @".clb (@a,@b/*comment*/) { background-image: url(""here.jpg"");}";
            
            AssertLessUnchanged (input);
        }

        [Test, ExpectedException(typeof(ParserException), ExpectedMessage = @"Comments are not supported in this location", MatchType = MessageMatch.Contains)]
        [Ignore("Would be nice to detect, but too difficult at the moment - probably easier to fix")]
        public void CommentNotSupported16 ()
        {
            var input = @".clb (@a,@b) /*comment*/ { background-image: url(""here.jpg"");}";

            AssertLessUnchanged (input);
        }
	}
}