using NUnit.Framework;

namespace dotless.Tests.Specs.Functions
{
  public class RgbaFixture : SpecFixtureBase
  {
    [Test]
    public void TestRgba()
    {
      AssertExpression("rgba(18, 52, 86, 0.5)", "rgba(18, 52, 86, 0.5)");
      AssertExpression("#beaded", "rgba(190, 173, 237, 1)");
      AssertExpression("rgba(0, 255, 127, 0)", "rgba(0, 255, 127, 0)");
    }

    [Test]
    public void TestRgbaOverflows()
    {
      AssertExpression("rgba(255, 1, 1, 0.3)", "rgba(256, 1, 1, 0.3)");
      AssertExpression("rgba(1, 1, 255, 0.3)", "rgba(1, 1, 256, 0.3)");
      AssertExpression("rgba(1, 255, 255, 0.3)", "rgba(1, 256, 257, 0.3)");
      AssertExpression("rgba(0, 1, 1, 0.3)", "rgba(-1, 1, 1, 0.3)");
      AssertExpression("rgba(1, 1, 1, 0)", "rgba(1, 1, 1, -0.2)");
      AssertExpression("#010101", "rgba(1, 1, 1, 1.2)");
    }

    [Test]
    public void TestRgbaTestsTypes()
    {
      AssertExpressionError("Expected number in function 'rgba', found \"foo\"", "rgba(\"foo\", 10, 12, 0.2)");
      AssertExpressionError("Expected number in function 'rgba', found \"foo\"", "rgba(10, \"foo\", 12, 0.1)");
      AssertExpressionError("Expected number in function 'rgba', found \"foo\"", "rgba(10, 10, \"foo\", 0)");
      AssertExpressionError("Expected number in function 'rgba', found \"foo\"", "rgba(10, 10, 10, \"foo\")");
    }

    [Test]
    public void TestRgbaWithColor()
    {
      AssertExpression("rgba(16, 32, 48, 0.5)", "rgba(#102030, 0.5)");
      AssertExpression("rgba(0, 0, 255, 0.5)", "rgba(blue, 0.5)");
    }

    [Test]
    public void TestRgbaWithColorTestsTypes()
    {
      AssertExpressionError("Expected color in function 'rgba', found \"foo\"", "rgba(\"foo\", 0.2)");
      AssertExpressionError("Expected number in function 'rgba', found \"foo\"", "rgba(#123456, \"foo\")");
    }

    [Test]
    public void TestRgbaTestsNumArgs()
    {
      AssertExpressionError("Expected 4 arguments in function 'rgba', found 0", "rgba()");
      AssertExpressionError("Expected 4 arguments in function 'rgba', found 1", "rgba(blue)");
      AssertExpressionError("Expected 4 arguments in function 'rgba', found 3", "rgba(1, 2, 3)");
      AssertExpressionError("Expected 4 arguments in function 'rgba', found 5", "rgba(1, 2, 3, 0.4, 5)");
    }

  }
}