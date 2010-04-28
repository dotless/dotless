using NUnit.Framework;

namespace dotless.Tests.Specs
{
  public class CssFixture : SpecFixtureBase
  {
    [Test]
    public void CharsetDirective()
    {
      var input = "@charset \"utf-8\";";

      AssertLessUnchanged(input);
    }

    [Test]
    public void Directives()
    {
      var input = @"
@media print {
  font-size: 3em;
}
@media screen {
  font-size: 10px;
}
@font-face {
  font-family: 'Garamond Pro';
  src: url(""/fonts/garamond-pro.ttf"");
}
";

      AssertLessUnchanged(input);
    }

    [Test]
    public void TagSelector()
    {
      var input = @"
div {
  color: black;
}
div {
  width: 99%;
}
";

      AssertLessUnchanged(input);
    }

    [Test]
    public void GlobalSelector()
    {
      var input = @"
* {
  min-width: 45em;
}
";

      AssertLessUnchanged(input);
    }

    [Test]
    public void ChildSelector()
    {
      var input = @"
h1, h2 > a > p, h3 {
  color: none;
}
";

      AssertLessUnchanged(input);
    }

    [Test]
    public void ClassAndIdSelectors()
    {
      var input = @"
div.class {
  color: blue;
}
div#id {
  color: green;
}
.class#id {
  color: purple;
}
.one.two.three {
  color: grey;
}
";

      AssertLessUnchanged(input);
    }

    [Test]
    public void PseudoClass()
    {
      var input = @"
a:hover, a:link {
  color: #999;
}
";

      AssertLessUnchanged(input);
    }

    [Test]
    public void FirstChildPseudoClass()
    {
      var input = @"
p, p:first-child {
  text-transform: none;
}
";

      AssertLessUnchanged(input);
    }

    [Test]
    public void LangPseudoClass()
    {
      var input = @"
q:lang(no) {
  quotes: none;
}
";

      AssertLessUnchanged(input);
    }

    [Test]
    public void NextSiblingSelector()
    {
      var input = @"
p + h1 {
  font-size: 2.2em;
}
";

      AssertLessUnchanged(input);
    }

    [Test]
    public void Shorthands()
    {
      var input = @"
#shorthands {
  border: 1px solid #000;
  font: 12px/16px Arial;
  margin: 1px 0;
  padding: 0 auto;
  background: url(""http://www.lesscss.org/spec.html"") no-repeat 0 4px;
}
#more-shorthands {
  margin: 0;
  padding: 1px 0 2px 0;
  font: normal small/20px 'Trebuchet MS', Verdana, sans-serif;
}
";

      AssertLessUnchanged(input);
    }

    [Test]
    public void Misc()
    {
      var input = @"
.misc {
  -moz-border-radius: 2px;
  display: -moz-inline-stack;
  width: .1em;
  background-color: #009998;
  background-image: url(images/image.jpg);
  background: -webkit-gradient(linear, left top, left bottom, from(red), to(blue));
  margin: ;
}
";

      AssertLessUnchanged(input);
    }

    [Test]
    public void Important()
    {
      var input = @"
#important {
  color: red !important;
  width: 100%!important;
  height: 20px ! important;
}
";

      AssertLessUnchanged(input);
    }

    [Test]
    public void AttributeEquals()
    {
      var input = @"
input[type=""text""] {
  font-weight: normal;
}
";

      AssertLessUnchanged(input);
    }

    [Test]
    public void AttributeExists()
    {
      var input = @"
h2[title] {
  font-size: 100%;
}
[disabled] {
  color: transparent;
}
form[data-disabled] {
  color: #444;
}
";

      AssertLessUnchanged(input);
    }

  }
}