using dotless.Core.Parser.Infrastructure;

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
        public void ExtendSelectorMultiple()
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

        [Test]
        public void ExtendSelectorAll()
        {
            string input = @".a.b.test,
.test.c {
  color: orange;
}
.test {
  &:hover {
    color: green;
  }
}

.replacement:extend(.test all) {}";

            string expected = @".a.b.test,
.test.c,
.a.b.replacement,
.replacement.c {
  color: orange;
}
.test:hover,
.replacement:hover {
  color: green;
}";
            AssertLess(input, expected);
        }

        [Test]
        public void ExtendInsideBodyRuleset()
        {
            string input = @"
div pre { color:red; }

pre:hover,
.some-class {
  &:extend(div pre);
}";
            string expected = @"div pre,
pre:hover,
.some-class {
  color: red;
}";
        }

    }
}