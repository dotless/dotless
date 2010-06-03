using NUnit.Framework;

namespace dotless.Tests.Specs.Functions
{
  public class LightnessFixture : SpecFixtureBase
  {
    [Test]
    public void TestLightness()
    {
      AssertExpression("86%", "lightness(hsl(120, 50%, 86%))");
      AssertExpression("86%", "lightness(hsl(120, 50%, 86))");
    }

    [Test]
    public void TestLightnessException()
    {
      AssertExpressionError("Expected color in function 'lightness', found 12", "lightness(12)");
    }

    [Test]
    public void TestEditLightness()
    {
      //Lighten
      AssertExpression("#4c4c4c", "lightness(hsl(0, 0, 0), 30%)");
      AssertExpression("#ee0000", "lightness(#800, 20%)");
      AssertExpression("white", "lightness(#fff, 20%)");
      AssertExpression("white", "lightness(#800, 100%)");
      AssertExpression("#880000", "lightness(#800, 0%)");
      AssertExpression("rgba(238, 0, 0, 0.5)", "lightness(rgba(136, 0, 0, .5), 20%)");

      //Darken
      AssertExpression("#ff6a00", "lightness(hsl(25, 100, 80), -30%)");
      AssertExpression("#220000", "lightness(#800, -20%)");
      AssertExpression("black", "lightness(#000, -20%)");
      AssertExpression("black", "lightness(#800, -100%)");
      AssertExpression("#880000", "lightness(#800, 0%)");
      AssertExpression("rgba(34, 0, 0, 0.5)", "lightness(rgba(136, 0, 0, .5), -20%)");
    }

    [Test]
    public void TestEditLightnessOverflow()
    {
      AssertExpression("white", "lightness(#000000, 101%)");
      AssertExpression("black", "lightness(#ffffff, -101%)");
    }

    [Test]
    public void TestEditLightnessTestsTypes()
    {
      AssertExpressionError("Expected color in function 'lightness', found \"foo\"", "lightness(\"foo\", 10%)");
      AssertExpressionError("Expected number in function 'lightness', found \"foo\"", "lightness(#fff, \"foo\")");
    }
  }
}