using NUnit.Framework;

namespace dotless.Tests.Specs
{
  public class ParensFixture : SpecFixtureBase
  {
    [Test]
    public void Parens()
    {
      var input =
        @"
.parens {
  @var: 1px;
  border: (@var * 2) solid black;
  margin: (@var * 1) (@var + 2) (4 * 4) 3;
  width: (6 * 6);
  padding: 2px (6px * 6px);
}
";

      var expected = @"
.parens {
  border: 2px solid black;
  margin: 1px 3px 16 3;
  width: 36;
  padding: 2px 36px;
}
";

      AssertLess(input, expected);
    }

    [Test]
    public void MoreParens()
    {
      var input =
        @"
.more-parens {
  @var: (2 * 2);
  padding: (2 * @var) 4 4 (@var * 1px);
  width: (@var * @var) * 6;
  height: (7 * 7) + (8 * 8);
  margin: 4 * (5 + 5) / 2 - (@var * 2);
  //margin: (6 * 6)px;
}
";

      var expected = @"
.more-parens {
  padding: 8 4 4 4px;
  width: 96;
  height: 113;
  margin: 12;
}
";

      AssertLess(input, expected);
    }

    [Test]
    public void NestedParens()
    {
      var input =
        @"
.nested-parens {
  width: 2 * (4 * (2 + (1 + 6))) - 1;
  height: ((2+3)*(2+3) / (9-4)) + 1;
}
";

      var expected = @"
.nested-parens {
  width: 71;
  height: 6;
}
";

      AssertLess(input, expected);
    }

    [Test]
    public void MixedUnits()
    {
      var input =
        @"
.mixed-units {
  margin: 2px 4em 1 5pc;
  padding: (2px + 4px) 1em 2px 2;
}

";

      var expected = @"
.mixed-units {
  margin: 2px 4em 1 5pc;
  padding: 6px 1em 2px 2;
}
";

      AssertLess(input, expected);
    }
  }
}