namespace dotless.Test.Specs.Functions
{
  using NUnit.Framework;

  public class ContrastFixture : SpecFixtureBase
  {
    [Test]
    public void TestContrast()
    {
      AssertExpression("white", "contrast(#000000)");
      AssertExpression("white", "contrast(#6D6D6D)");
      AssertExpression("white", "contrast(#6E6E6E)");
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
      AssertExpression("yellow", "contrast(#000000, yellow)");
      AssertExpression("yellow", "contrast(#6D6D6D, yellow)");
      AssertExpression("yellow", "contrast(#6E6E6E, yellow)");
      AssertExpression("black", "contrast(#FFFFFF, yellow)");
    }

    [Test]
    public void OverrideDarkColor()
    {
      AssertExpression("white", "contrast(#6D6D6D, white, green)");
      AssertExpression("white", "contrast(#000000, white, green)");
      AssertExpression("white", "contrast(#6E6E6E, white, green)");
      AssertExpression("green", "contrast(#FFFFFF, white, green)");
    }

    [Test]
    public void OverrideThreshold()
    {
      AssertExpression("white", "contrast(hsl(120, 50%, 0%), white, black, 1%)");
      AssertExpression("white", "contrast(hsl(120, 50%, 24%), white, black, 25%)");
      AssertExpression("white", "contrast(hsl(120, 50%, 25%), white, black, 25%)");
      AssertExpression("white", "contrast(#84C447, white, black, 60%)");
      AssertExpression("black", "contrast(hsl(120, 50%, 74%), white, black, 25%)");
      AssertExpression("black", "contrast(hsl(120, 50%, 75%), white, black, 25%)");
      AssertExpression("black", "contrast(hsl(120, 50%, 100%), white, black, 25%)");
      
    }
  }
}