using NUnit.Framework;

namespace dotless.Test.Spec
{
    [TestFixture]
    public class VariablesFixture : SpecFixtureBase
    {
        [Test]
        public void VariablesCanHoldPaddingInfo()
        {
            var less = @"
@cell_padding: 1 2em 3px 4;

table {
  th {
    padding: @cell_padding;
  }
}";

            var css = "table th { padding: 1 2em 3px 4; }";

            AssertLess(css, less);
        }
    }
}