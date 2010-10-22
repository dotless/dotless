namespace dotless.Test.Specs.Functions
{
  using NUnit.Framework;

  public class ContrastFixture : SpecFixtureBase
  {
    [Test]
    public void TestContrast()
    {
      AssertExpression("white", "contrast(#000000)");
      AssertExpression("black", "contrast(#FFFFFF)");

      //AssertExpressionError("Expected Color in function 'TestContrast', found \"foo\"", 6, "contrast(\"foo\")");
    }
  }
}