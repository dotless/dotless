namespace dotless.Test.Specs
{
    using System.Collections.Generic;
    using NUnit.Framework;

    public class ExtendFixture : SpecFixtureBase
    {
        [Test]
        public void ExtendRule()
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
.inline, nav ul {
  color: red;
}";
            AssertLess(input, expected);
        }

        [Test]
        public void ExtendDefinition()
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
    }
}