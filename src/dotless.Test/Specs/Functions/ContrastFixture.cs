namespace dotless.Test.Specs.Functions
{
  using NUnit.Framework;

  public class ContrastFixture : SpecFixtureBase
  {
    [Test]
    public void TestContrast()
    {
      AssertExpression("white", "contrast(#000000)");
      AssertExpression("white", "contrast(#797979)");
      AssertExpression("black", "contrast(#808080)");
      AssertExpression("black", "contrast(#FFFFFF)");
    }

    [Test]
    public void TestContrastException()
    {
      AssertExpressionError("Expected color in function 'contrast', found \"foo\"", 0, "contrast(\"foo\")");
    }

    [Test]
    public void OverrideLightColor()
    {
      AssertExpression("blue", "contrast(#000000, blue)");
      AssertExpression("blue", "contrast(#797979, blue)");
      AssertExpression("black", "contrast(#808080, blue)");
      AssertExpression("black", "contrast(#FFFFFF, blue)");
    }

    [Test]
    public void OverrideDarkColor()
    {
      AssertExpression("white", "contrast(#797979, white, green)");
      AssertExpression("white", "contrast(#000000, white, green)");
      AssertExpression("green", "contrast(#808080, white, green)");
      AssertExpression("green", "contrast(#FFFFFF, white, green)");
    }

    [Test]
    public void OverrideThreshold()
    {
      AssertExpression("white", "contrast(hsl(120, 50%, 0%), white, black, 1%)");
      AssertExpression("white", "contrast(hsl(120, 50%, 24%), white, black, 25%)");
      AssertExpression("black", "contrast(hsl(120, 50%, 25%), white, black, 25%)");
      AssertExpression("white", "contrast(hsl(120, 50%, 74%), white, black, 75%)");
      AssertExpression("black", "contrast(hsl(120, 50%, 75%), white, black, 75%)");
      AssertExpression("black", "contrast(hsl(120, 50%, 100%), white, black, 99%)");
    }
  }
}