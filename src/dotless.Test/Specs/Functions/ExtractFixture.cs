namespace dotless.Test.Specs.Functions
{
    using NUnit.Framework;

    public class ExtractFixture : SpecFixtureBase
    {
        [Test]
        public void TestExtractFromCommaSeparatedList()
        {
            var input =
                @"
@list: ""Arial"", ""Helvetica"";
.someClass {
    font-family: e(extract(@list, 2));
}";

            var expected =
                @"
.someClass {
  font-family: Helvetica;
}";

            AssertLess(input, expected);
        }
        [Test]
        public void TestExtractFromSpaceSeparatedList()
        {
            var input =
                @"
@list: 1px solid blue;
.someClass {
    border: e(extract(@list, 2));
}";

            var expected =
                @"
.someClass {
  border: solid;
}";

            AssertLess(input, expected);
        }
    }
}
