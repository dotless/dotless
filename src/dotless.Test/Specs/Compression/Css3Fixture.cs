namespace dotless.Test.Specs.Compression
{
    using NUnit.Framework;

    public class Css3Fixture : CompressedSpecFixtureBase
    {
        [Test]
        public void MediaDirectiveEmpty1()
        {
            var input = @"
@media only screen and (min-width: 768px) and (max-width: 959px) {
}
";

            AssertLess(input, "");
        }

        [Test]
        public void MediaDirectiveEmpty2NotOptimalButTestOkOutput()
        {
            // optimally this would compress to "" but this isn't implemented
            // so just test the output is at least valid.
            var input = @"
@media only screen and (min-width: 768px) and (max-width: 959px) {
  .class {
  }
}
";

            AssertLess(input, @"
@media only screen and (min-width:768px) and (max-width:959px){}");
        }
    }
}
