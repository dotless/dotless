using NUnit.Framework;

namespace dotless.Tests.Specs.Functions
{
  public class CielFixture : SpecFixtureBase
  {

    [Test]
    public void TestCeil()
    {
      AssertExpression("4", "ceil(4)");
      AssertExpression("5", "ceil(4.8)");
      AssertExpression("5px", "ceil(4.8px)");
      AssertExpression("6px", "ceil(5.49px)");
      AssertExpression("51%", "ceil(50.1%)");

      AssertExpressionError("Expected number in function 'ceil', found \"a\"", "ceil(\"a\")");
    }

  }
}