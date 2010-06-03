using NUnit.Framework;

namespace dotless.Tests.Specs.Functions
{
  public class BlueFixture : SpecFixtureBase
  {    
    [Test]
    public void TestBlue()
    {
      AssertExpression("86", "blue(#123456)");
    }

    [Test]
    public void TestBlueException()
    {
      AssertExpressionError("Expected color in function 'blue', found 12", "blue(12)");
    }

    [Test]
    public void TestEditBlue()
    {
      AssertExpression("#123460", "blue(#123456, 10)");
    }

    [Test]
    public void TestEditBlueTestsTypes()
    {
      AssertExpressionError("Expected color in function 'blue', found 12", "blue(12)");
    }
  }
}