using NUnit.Framework;

namespace dotless.Tests.Specs.Functions
{
  public class PercentageFixture : SpecFixtureBase
  {
    [Test]
    public void TestPercentage()
    {
      AssertExpression("25%", "percentage(25%)");
      AssertExpression("2500%", "percentage(25)");
      AssertExpression("50%", "percentage(.5)");
      AssertExpression("100%", "percentage(1)");
      AssertExpression("25%", "percentage(25px / 100px)");
    }

    [Test]
    public void TestPercentageChecksTypes()
    {
      AssertExpressionError("Expected unitless number in function 'percentage', found 25px", "percentage(25px)");
      AssertExpressionError("Expected number in function 'percentage', found #cccccc", "percentage(#ccc)");
      AssertExpressionError("Expected number in function 'percentage', found 'string'", "percentage('string')");
    }

  }
}