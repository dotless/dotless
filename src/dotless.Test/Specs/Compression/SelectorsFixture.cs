namespace dotless.Test.Specs.Compression
{
    using NUnit.Framework;

    public class SelectorsFixture : CompressedSpecFixtureBase
    {
        [Test]
        public void ParentSelector1()
        {
            var input =
                @"
h1, h2, h3 {
  a, p {
    &:hover {
      color: red;
    }
  }
}
";

            var expected = "h1 a:hover,h2 a:hover,h3 a:hover,h1 p:hover,h2 p:hover,h3 p:hover{color:red}";

            AssertLess(input, expected);
        }

        [Test]
        public void IdSelectors()
        {
            var input =
                @"
#all { color: blue; }
#the { color: blue; }
#same { color: blue; }
";

            var expected = "#all{color:blue}#the{color:blue}#same{color:blue}";

            AssertLess(input, expected);
        }

        [Test]
        public void Tag()
        {
            var input = @"
td {
  margin: 0;
  padding: 0;
}
";

            var expected = "td{margin:0;padding:0}";

            AssertLess(input, expected);
        }

        [Test]
        public void TwoTags()
        {
            var input = @"
td, input {
  line-height: 1em;
}
";

            var expected = "td,input{line-height:1em}";

            AssertLess(input, expected);
        }

        [Test]
        public void MultipleTags()
        {
            var input =
                @"
ul, li, div, q, blockquote, textarea {
  margin: 0;
}
";

            var expected = "ul,li,div,q,blockquote,textarea{margin:0}";

            AssertLess(input, expected);
        }


        [Test]
        public void DecendantSelectorWithTabs()
        {
            var input = "td \t input { line-height: 1em; }";

            var expected = "td input{line-height:1em}";

            AssertLess(input, expected);
        }
    }
}