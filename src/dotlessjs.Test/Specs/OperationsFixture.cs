using System.Collections.Generic;
using NUnit.Framework;

namespace dotless.Tests.Specs
{
  public class OperationsFixture : SpecFixtureBase
  {
    [Test]
    public void Operations()
    {
      AssertExpression("#111111", "#110000 + #000011 + #001100");
      AssertExpression("9px", "10px / 2px + 6px - 1px * 2");
      AssertExpression("9px", "10px / 2px+6px-1px*2");
      AssertExpression("3em", "2 * 4 - 5em");
      AssertExpression("3em", "2  * 4-5em");
    }

    [Test]
    public void WithVariables()
    {
      var variables = new Dictionary<string, string>();
      variables["x"] = "4";
      variables["y"] = "12em";

      AssertExpression("16em", "@x + @y", variables);
      AssertExpression("24em", "12 + @y", variables);
      AssertExpression("1cm", "5cm - @x", variables);
    }

    [Test]
    public void Negative()
    {
      var variables = new Dictionary<string, string>();
      variables["z"] = "-2";

      AssertExpression("0px", "2px + @z", variables);
      AssertExpression("4px", "2px - @z", variables);
    }

    [Test]
    public void Shorthands()
    {
      AssertExpression("-1px 2px 0 -4px", "-1px 2px 0 -4px");
    }

    [Test]
    public void Colours()
    {
      AssertExpression("#123",    "#123");
      AssertExpression("#334455", "#234 + #111111");
      AssertExpression("black",   "#222222 - #fff");
      AssertExpression("#222222", "2 * #111");
      AssertExpression("#222222", "#333333 / 3 + #111");
    }

    [Test]
    public void OperationsAreLeftAssociative()
    {
      AssertExpression("1", "20 / 5 / 4");
      AssertExpression("0", "20 - 10 - 5 - 5");
    }

    [Test]
    public void Rounding()
    {
      var variables = new Dictionary<string, string>();
      variables["base"] = "16em";

      AssertExpression("3.75em", "60/@base", variables);
    }
  }
}