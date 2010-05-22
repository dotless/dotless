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
  -moz-box-shadow: 0pt 0pt 2px rgba(255, 255, 255, 0.4) inset, 0pt 4px 6px rgba(255, 255, 255, 0.4) inset;
}
";

      AssertLessUnchanged(input);
    }

    [Test]
    public void FontFaceDirective()
    {
      var input = @"
@font-face {
  font-family: Headline;
  src: local(Futura-Medium), url(fonts.svg#MyGeometricModern) format(""svg"");
}
";

      AssertLessUnchanged(input);
    }

    [Test]
    public void MediaDirective()
    {
      var input = @"
@media all and (min-width: 640px) {
  #media-queries-1 {
    background-color: #0f0;
  }
}
";

      AssertLessUnchanged(input);
    }
    
    [Test]
    public void MozTransform()
    {
      var input = @"
.other {
  -moz-transform: translate(0, 11em) rotate(-90deg);
}
";

      AssertLessUnchanged(input);
    }
    
    [Test]
    public void NotPseudoClass()
    {
      var input = @"
p:not([class*=""lead""]) {
  color: black;
}
";

      AssertLessUnchanged(input);
    }
    
    [Test]
    public void MultipleAttributeSelectors1()
    {
      var input = @"
input[type=""text""].class#id[attr=32]:not(1) {
  color: white;
}
";

      AssertLessUnchanged(input);
    }

    [Test]
    public void MultipleAttributeSelectors2()
    {
      var input = @"
div#id.class[a=1][b=2].class:not(1) {
  color: white;
}
";

      AssertLessUnchanged(input);
    }
    
    [Test]
    public void BeforePseudoElement()
    {
      var input = @"
p::before {
  color: black;
}
";

      AssertLessUnchanged(input);
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

      AssertLessUnchanged(input);
    }

    [Test]
    public void NthChildExpressions()
    {
      var input = @"
li:nth-child(4n+1), li:nth-child(-5n), li:nth-child(-n+2) {
  color: white;
}
";

      AssertLessUnchanged(input);
    }

    [Test]
    public void AttributeBeginsWith()
    {
      var input = @"
a[href^=""http://""] {
  color: black;
}
";

      AssertLessUnchanged(input);
    }

    [Test]
    public void AttributeEndsWith()
    {
      var input = @"
a[href$=""http://""] {
  color: black;
}
";

      AssertLessUnchanged(input);
    }

    [Test]
    public void SiblingSelector()
    {
      var input = @"
a ~ p {
  background: red;
}
";

      AssertLessUnchanged(input);
    }

    [Test]
    public void MultipleBackgroundProperty()
    {
      var input = @"
.class {
  background: url(bg1.gif) top left no-repeat, url(bg2.jpg) top right no-repeat;
}
";

      AssertLessUnchanged(input);
    }

  }
}