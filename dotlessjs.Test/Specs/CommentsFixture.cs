using NUnit.Framework;

namespace dotless.Tests.Specs
{
  public class CommentsFixture : SpecFixtureBase
  {
    [Test]
    public void Comments()
    {
      // Todo: split into separate atomic tests.
      var input =
        @"
/******************\
*                  *
*  Comment Header  *
*                  *
\******************/

/*

    Comment

*/

/*  
 * Comment Test
 * 
 * - dotless (http://dotlesscss.com)
 *
 */

////////////////

/* Colors
 * ------
 *   #EDF8FC (background blue)
 *   #166C89 (darkest blue)
 *
 * Text:
 *   #333 (standard text) // A comment within a comment!
 *   #1F9EC9 (standard link)
 *
 */

/* @group Variables
------------------- */
#comments /* boo */ {
  /**/ // An empty comment 
  color: red; /* A C-style comment */
  background-color: orange; // A little comment
  font-size: 12px;
  
  /* lost comment */ content: ""content"";
  
  border: 1px solid black;
  
  // padding & margin //
  padding: 0;
  margin: 2em;
} //

/* commented out
  #more-comments {
    color: grey;
  }
*/

#last { color: blue }
//
";

      var expected = @"
/******************\
*                  *
*  Comment Header  *
*                  *
\******************/
/*

    Comment

*/
/*  
 * Comment Test
 * 
 * - dotless (http://dotlesscss.com)
 *
 */
/* Colors
 * ------
 *   #EDF8FC (background blue)
 *   #166C89 (darkest blue)
 *
 * Text:
 *   #333 (standard text) // A comment within a comment!
 *   #1F9EC9 (standard link)
 *
 */
/* @group Variables
------------------- */
#comments {
  /**/
  color: red;
  /* A C-style comment */

  background-color: orange;
  font-size: 12px;
  /* lost comment */
  content: ""content"";
  border: 1px solid black;
  padding: 0;
  margin: 2em;
}
/* commented out
  #more-comments {
    color: grey;
  }
*/
#last {
  color: blue;
}
";

      AssertLess(input, expected);
    }
  }
}