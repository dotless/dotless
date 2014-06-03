namespace dotless.Test.Specs
{
    using System.Collections.Generic;
    using NUnit.Framework;

    public class ExtendFixture : SpecFixtureBase
    {
        [Test]
        public void ExtendSelector()
        {
            string input = @"nav ul:extend(.inline) {
  background: blue;
}
.inline {
  color: red;
}";
            string expected = @"nav ul {
  background: blue;
}
.inline,
nav ul {
  color: red;
}";
            AssertLess(input, expected);
        }

        [Test]
        public void ExtendRuleSet()
        {
            string input = @"nav ul {
  &:extend(.inline);
  background: blue;
}
.inline {
  color: red;
}";

            string expected = @"nav ul {
  background: blue;
}
.inline,
nav ul {
  color: red;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void ExtendMultiple()
        {
            string input = @".e:extend(.f, .g) { background-color:green; }
.f { color: red; }
.g { color: blue; }
";
            string expected = @".e {
  background-color: green;
}
.f,
.e {
  color: red;
}
.g,
.e {
  color: blue;
}
";
            AssertLess(input,expected);
        }

    }
}