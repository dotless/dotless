namespace dotless.Test.Specs
{
    using NUnit.Framework;
	using dotless.Core.Exceptions;
	
    public class CommentsFixture : SpecFixtureBase
    {
        [Test]
        public void CommentHeader()
        {
            var input =
                @"
/******************\
*                  *
*  Comment Header  *
*                  *
\******************/
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void MultilineComment()
        {
            var input =
                @"
/*

    Comment

*/
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void MultilineComment2()
        {
            var input =
                @"
/*  
 * Comment Test
 * 
 * - dotless (http://dotlesscss.com)
 *
 */
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void LineCommentGetsRemoved()
        {
            var input = "////////////////";
            var expected = "";

            AssertLess(input, expected);
        }
		
        [Test]
        public void ColorInsideComments()
        {
            var input =
                @"
/* Colors
 * ------
 *   #EDF8FC (background blue)
 *   #166C89 (darkest blue)
 *
 * Text:
 *   #333 (standard text)
 *   #1F9EC9 (standard link)
 *
 */
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void CommentInsideAComment()
        {
            var input =
                @"
/*  
 * Comment Test
 * 
 *  // A comment within a comment!
 * 
 */
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void VariablesInsideComments()
        {
            var input =
                @"
/* @group Variables
------------------- */
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void BlockCommentAfterSelector()
        {
            var input =
                @"
#comments /* boo */ {
  color: red;
}
";

            var expected = @"
#comments/* boo */ {
  color: red;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void EmptyComment()
        {
            var input =
                @"
#comments {
  border: solid black;
  /**/
  color: red;
}
";

            var expected =
                @"
#comments {
  border: solid black;
  /**/
  color: red;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void BlockCommentAfterProperty()
        {
            var input =
                @"
#comments {
  border: solid black;
  color: red; /* A C-style comment */
  padding: 0;
}
";

            var expected =
                @"
#comments {
  border: solid black;
  color: red;
  /* A C-style comment */
  padding: 0;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void LineCommentAfterProperty()
        {
            var input =
                @"
#comments {
  border: solid black;
  color: red; // A little comment
  padding: 0;
}
";

            var expected = @"
#comments {
  border: solid black;
  color: red;
  padding: 0;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void BlockCommentBeforeProperty()
        {
            var input =
                @"
#comments {
  border: solid black;
  /* comment */ color: red;
  padding: 0;
}
";

            var expected = @"
#comments {
  border: solid black;
  /* comment */
  color: red;
  padding: 0;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void LineCommentAfterALineComment()
        {
            var input =
                @"
#comments {
  border: solid black;
  // comment //
  color: red;
  padding: 0;
}
";

            var expected = @"
#comments {
  border: solid black;
  color: red;
  padding: 0;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void LineCommentAfterBlock()
        {
            var input =
                @"
#comments /* boo */ {
  color: red;
} // comment
";

            var expected = @"
#comments/* boo */ {
  color: red;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void BlockCommented1()
        {
            var input =
                @"
/* commented out
  #more-comments {
    color: grey;
  }
*/
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void BlockCommented2()
        {
            var input =
                @"
/* 
#signup form h3.title {
    background:transparent url(../img/ui/signup.png) no-repeat 50px top;
    height:100px;
}
*/
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void BlockCommented3()
        {
            var input =
                @"
#more-comments {
  quotes: ""/*"" ""*/"";
}
";

            AssertLessUnchanged(input);
        }


        [Test]
        public void CommentOnLastLine()
        {
            var input =
                @"
#last { color: blue }
//
";

            var expected = @"
#last {
  color: blue;
}
";

            AssertLess(input, expected);
        }
		
		[Test]
		[ExpectedException(typeof(ParserException))]
        public void CheckCommentsAreNotAcceptedAsASelector()
        {
			// Note: https://github.com/dotless/dotless/issues/31
        	var input = @"/* COMMENT *//* COMMENT */, /* COMMENT */,/* COMMENT */ .clb /* COMMENT */ {background-image: url(pickture.asp);}";

            AssertLessUnchanged(input);
        }
		
		[Test]
        public void CheckCommentsAreAcceptedBetweenSelectors()
        {
			// Note: https://github.com/dotless/dotless/issues/31
        	var input = @"/* COMMENT */body/* COMMENT */,/* COMMENT */ .clb /* COMMENT */ {background-image: url(pickture.asp);}";

			var expected = @"/* COMMENT */body/* COMMENT */, /* COMMENT */ .clb/* COMMENT */ {
  background-image: url(pickture.asp);
}";
			
            AssertLess(input, expected);
        }
		
		[Test, Ignore("Bug to fix in the future - dotLess still doesn't allow comments everywhere")]
        public void CheckCommentsAreAcceptedWhereWhitespaceIsAllowed()
        {
			// Note: https://github.com/dotless/dotless/issues/31
        	var input = @"/* COMMENT */body/* COMMENT */, /* COMMENT */.cls/* COMMENT */ .cla,/* COMMENT */ .clb /* COMMENT */ {background-image: url(pickture.asp);}";

			var expected = @"body, .cls .cla, .clb {
  background-image: url(pickture.asp);
}";
			
            AssertLess(input, expected);
        }
		
		[Test, Ignore("Bug to fix in the future - dotLess still doesn't allow comments everywhere")]
        public void CheckCommentsAreTakenToBeWhitespace1()
        {
        	var input = @".cls/* COMMENT */.cla {background-image: url(pickture.asp);}";

			var expected = @".cls .cla {
  background-image: url(pickture.asp);
}";
			
            AssertLess(input, expected);
        }
		
		[Test, Ignore("Bug to fix in the future - dotLess still doesn't allow comments everywhere")]
        public void CheckCommentsAreTakenToBeWhitespace2()
        {
        	var input = @".cls/* COMMENT */ + /* COMMENT */.cla {background-image: url(pickture.asp);}";

			var expected = @".cls + .cla {
  background-image: url(pickture.asp);
}";
			
            AssertLess(input, expected);
        }
		
		[Test]
        public void CommentCSSHackException1Accepted()
        {
        	var input = @"/*\*/.cls {background-image: url(picture.asp);} /**/";
			
			var expected = @"/*\*/.cls {
  background-image: url(picture.asp);
}
/**/";

            AssertLess(input, expected);
        }
		
		[Test]
        public void CommentCSSHackException2Accepted()
        {
        	var input = @"/*\*//*/ .cls {background-image: url(picture.asp);} /**/";

            AssertLessUnchanged(input);
        }
    }
}