// ReSharper disable ConvertToConstant.Local

namespace dotless.Test.Specs.Compression
{
    using NUnit.Framework;

    [TestFixture]
    public class WhitespaceFixture : CompressedSpecFixtureBase
    {
        [Test]
        public void Whitespace()
        {
            var input =
                @"
.whitespace { color: white; }
.whitespace{color:white;}
.whitespace { color: white ; }
.whitespace 
{
color:
white;
}
.whitespace { color : white; }
";

            var expected = ".whitespace{color:white}.whitespace{color:white}.whitespace{color:white}.whitespace{color:white}.whitespace{color:white}";

            AssertLess(input, expected);
        }

        [Test]
        public void Whitespace2()
        {
            var input = @"
.white,
.space,
.mania
{ 
 color
      :
        white;
}";

            var expected = ".white,.space,.mania{color:white}";

            AssertLess(input, expected);
        }

        [Test]
        public void NoSemiColon1()
        {
            var input = ".no-semi-colon { color: white }";

            var expected = ".no-semi-colon{color:#fff}";

            AssertLess(input, expected);
        }

        [Test]
        public void NoSemiColon2()
        {
            var input = @"
.no-semi-colon {
  color: white;
  white-space: pre
}";

            var expected = ".no-semi-colon{color:white;white-space:pre}";

            AssertLess(input, expected);
        }

        [Test]
        public void NoSemiColon3()
        {
            var input = ".no-semi-colon { border: 2px solid white }";

            var expected = ".no-semi-colon{border:2px solid #fff}";

            AssertLess(input, expected);
        }

        [Test]
        public void NewLines1()
        {
            var input =
                @"
.newlines {
  background: the,
              
great,
              wall;
  border: 2px
          solid
          black;
}";

            var expected = ".newlines{background:the,great,wall;border:2px solid black}";

            AssertLess(input, expected);
        }

        [Test]
        public void NewLines2()
        {
            var input =
                @"
.newlines,
.are
.bad {
  foo: bar;
}";

            var expected = ".newlines,.are .bad{foo:bar}";

            AssertLess(input, expected);
        }

        [Test]
        public void Empty()
        {
            var input = ".empty { }";

            var expected = "";

            AssertLess(input, expected);
        }
    }
}