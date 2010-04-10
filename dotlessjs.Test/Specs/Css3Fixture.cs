using NUnit.Framework;

namespace dotless.Tests.Specs
{
  public class Css3Fixture : SpecFixtureBase
  {
    [Test]
    public void CommaDelimited()
    {
      var input = @"
.comma-delimited { 
  background: url(bg.jpg) no-repeat, url(bg.png) repeat-x top left, url(bg);
  text-shadow: -1px -1px 1px red, 6px 5px 5px yellow;
  -moz-box-shadow: 0pt 0pt 2px rgba(255, 255, 255, 0.4) inset,
    0pt 4px 6px rgba(255, 255, 255, 0.4) inset;
}
";

      var expected = @"
.comma-delimited {
  background: url(bg.jpg) no-repeat, url(bg.png) repeat-x top left, url(bg);
  text-shadow: -1px -1px 1px red, 6px 5px 5px yellow;
  -moz-box-shadow: 0pt 0pt 2px rgba(255, 255, 255, 0.4) inset, 0pt 4px 6px rgba(255, 255, 255, 0.4) inset;
}
";

      AssertLess(input, expected);
    }

    [Test]
    public void FontFace()
    {
      var input = @"
@font-face {
  font-family: Headline;
  src: local(Futura-Medium),
       url(fonts.svg#MyGeometricModern) format(""svg"");
}
";

      var expected = @"
@font-face {
  font-family: Headline;
  src: local(Futura-Medium), url(fonts.svg#MyGeometricModern) format(""svg"");
}
";

      AssertLess(input, expected);
    }
    
    [Test]
    public void MozTransform()
    {
      var input = @"
.other {
  -moz-transform: translate(0, 11em) rotate(-90deg);
}
";

      var expected = @"
.other {
  -moz-transform: translate(0, 11em) rotate(-90deg);
}
";

      AssertLess(input, expected);
    }
    
    [Test]
    public void NotPseudoClass()
    {
      var input = @"
p:not([class*=""lead""]) {
  color: black; 
}
";

      var expected = @"
p:not([class*=""lead""]) {
  color: black;
}
";

      AssertLess(input, expected);
    }
    
    [Test]
    public void MultipleAttributeSelectors()
    {
      var input = @"
input[type=""text""].class#id[attr=32]:not(1) {
  color: white;
}

div#id.class[a=1][b=2].class:not(1) {
  color: white;
}
";

      var expected = @"
input[type=""text""].class#id[attr=32]:not(1) {
  color: white;
}
div#id.class[a=1][b=2].class:not(1) {
  color: white;
}
";

      AssertLess(input, expected);
    }

    [Test]
    public void BeforePseudoElement()
    {
      var input = @"
p::before {
  color: black;
}
";

      var expected = @"
p::before {
  color: black;
}
";

      AssertLess(input, expected);
    }

    [Test]
    public void AfterPseudoElement()
    {
      var input = @"
ul.comma > li:not(:only-child)::after { 
  color: white;
}

ol.comma > li:nth-last-child(2)::after {
  color: white;
}
";

      var expected = @"
ul.comma > li:not(:only-child)::after {
  color: white;
}
ol.comma > li:nth-last-child(2)::after {
  color: white;
}
";

      AssertLess(input, expected);
    }

    [Test]
    public void NthChildExpressions()
    {
      var input = @"
li:nth-child(4n+1),
li:nth-child(-5n),
li:nth-child(-n+2) {
  color: white;
}
";

      var expected = @"
li:nth-child(4n+1), li:nth-child(-5n), li:nth-child(-n+2) {
  color: white;
}
";

      AssertLess(input, expected);
    }

    [Test]
    public void AttributeBeginsWith()
    {
      var input = @"
a[href^=""http://""] {
  color: black;
}
";

      var expected = @"
a[href^=""http://""] {
  color: black;
}
";

      AssertLess(input, expected);
    }

    [Test]
    public void AttributeEndsWith()
    {
      var input = @"
a[href$=""http://""] {
  color: black;
}
";

      var expected = @"
a[href$=""http://""] {
  color: black;
}
";

      AssertLess(input, expected);
    }

    [Test]
    public void SiblingSelector()
    {
      var input = @"
a ~ p {
  background: red;
}
";

      var expected = @"
a ~ p {
  background: red;
}
";

      AssertLess(input, expected);
    }

  }
}