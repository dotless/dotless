namespace dotless.Test.Specs.Functions
{
    using NUnit.Framework;

    public class LengthFixture : SpecFixtureBase
    {
        [Test]
        public void TestLengthOfCommaSeparatedList()
        {
            var input =
                @"
@list: ""yes"", ""no"", ""maybe"";
@listLength: length(@list);
.someClass {
  border: e('@{listLength}px solid blue');
}";

            var expected =
                @"
.someClass {
  border: 3px solid blue;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void TestLengthOfSpaceSeparatedList()
        {
            var input =
                @"
@list: red green blue;
@listLength: length(@list);
.someClass {
  border: e('@{listLength}px solid blue');
}";

            var expected =
                @"
.someClass {
  border: 3px solid blue;
}";

            AssertLess(input, expected);
        }
    }
}
