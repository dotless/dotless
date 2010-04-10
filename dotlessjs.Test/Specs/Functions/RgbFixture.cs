using NUnit.Framework;

namespace dotless.Tests.Specs.Functions
{
  public class RgbFixture : SpecFixtureBase
  {
    [Test]
    public void TestRgb()
    {
      AssertExpression("#123456", "rgb(18, 52, 86)");
      AssertExpression("#beaded", "rgb(190, 173, 237)");
      AssertExpression("#00ff7f", "rgb(0, 255, 127)");
    }

    [Test]
    public void TestRgbPercent()
    {
      AssertExpression("#123456", "rgb(7.1%, 20.4%, 33.7%)");
      AssertExpression("#beaded", "rgb(74.7%, 173, 93%)");
      AssertExpression("#beaded", "rgb(190, 68%, 237)");
      AssertExpression("#00ff80", "rgb(0%, 100%, 50%)");
    }

    [Test]
    public void TestRgbOverflows()
    {
      AssertExpression("#ff0101", "rgb(256, 1, 1)");
      AssertExpression("#01ff01", "rgb(1, 256, 1)");
      AssertExpression("#0101ff", "rgb(1, 1, 256)");
      AssertExpression("#01ffff", "rgb(1, 256, 257)");
      AssertExpression("#000101", "rgb(-1, 1, 1)");
    }

    [Test]
    public void TestRgbTestPercentBounds()
    {
      AssertExpression("red", "rgb(100.1%, 0, 0)");
      AssertExpression("black", "rgb(0, -0.1%, 0)");
      AssertExpression("blue", "rgb(0, 0, 101%)");
    }

    [Test]
    public void TestRgbTestsTypes()
    {
      AssertExpressionError("Expected number in function 'rgb', found \"foo\"", "rgb(\"foo\", 10, 12)");
      AssertExpressionError("Expected number in function 'rgb', found \"foo\"", "rgb(10, \"foo\", 12)");
      AssertExpressionError("Expected number in function 'rgb', found \"foo\"", "rgb(10, 10, \"foo\")");
    }
  }
}