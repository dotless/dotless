namespace dotless.Test.Specs.Functions
{
  using NUnit.Framework;

  public class ColorFixture : SpecFixtureBase
  {
    [Test]
    public void TestColor()
    {
        AssertExpression("red", @"color(""#ff0000"")");
        AssertExpression("aqua", @"color(""#0ff"")");
    }
  }
}