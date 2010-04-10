using NUnit.Framework;

namespace dotless.Tests.Specs
{
  public class CssFixture : SpecFixtureBase
  {
    [Test]
    public void CharsetDirective()
    {
      var input = @"
@charset ""utf-8"";
";

      var expected = @"
@charset ""utf-8"";
";

      AssertLess(input, expected);
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

      var expected = @"
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

      AssertLess(input, expected);
    }

    [Test]
    public void TagSelector()
    {
      var input = @"
div { color: black; }
div { width: 99%; }
";

      var expected = @"
div {
  color: black;
}
div {
  width: 99%;
}
";

      AssertLess(input, expected);
    }

    [Test]
    public void GlobalSelector()
    {
      var input = @"
* {
  min-width: 45em;
}
";

      var expected = @"
* {
  min-width: 45em;
}
";

      AssertLess(input, expected);
    }

    [Test]
    public void ChildSelector()
    {
      var input = @"
h1, h2 > a > p, h3 {
  color: none;
}
";

      var expected = @"
h1, h2 > a > p, h3 {
  color: none;
}
";

      AssertLess(input, expected);
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

      var expected = @"
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

      AssertLess(input, expected);
    }

    [Test]
    public void PseudoClass()
    {
      var input = @"
a:hover, a:link {
  color: #999;
}
";

      var expected = @"
a:hover, a:link {
  color: #999;
}
";

      AssertLess(input, expected);
    }

    [Test]
    public void FirstChildPseudoClass()
    {
      var input = @"
p, p:first-child {
  text-transform: none;
}
";

      var expected = @"
p, p:first-child {
  text-transform: none;
}
";

      AssertLess(input, expected);
    }

    [Test]
    public void LangPseudoClass()
    {
      var input = @"
q:lang(no) {
  quotes: none;
}
";

      var expected = @"
q:lang(no) {
  quotes: none;
}
";

      AssertLess(input, expected);
    }

    [Test]
    public void NextSiblingSelector()
    {
      var input = @"
p + h1 {
  font-size: 2.2em;
}
";

      var expected = @"
p + h1 {
  font-size: 2.2em;
}
";

      AssertLess(input, expected);
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

      var expected = @"
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

      AssertLess(input, expected);
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

      var expected = @"
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

      AssertLess(input, expected);
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

      var expected = @"
#important {
  color: red !important;
  width: 100%!important;
  height: 20px ! important;
}
";

      AssertLess(input, expected);
    }

    [Test]
    public void AttributeEquals()
    {
      var input = @"
input[type=""text""] {
  font-weight: normal;
}
";

      var expected = @"
input[type=""text""] {
  font-weight: normal;
}
";

      AssertLess(input, expected);
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
";

      var expected = @"
h2[title] {
  font-size: 100%;
}
[disabled] {
  color: transparent;
}
";

      AssertLess(input, expected);
    }

    [Test]
    public void CssAttributes()
    {
      var input = @"
input[type=""text""] {
  font-weight: normal;
}

h2[title] {
  font-size: 100%;
}

[disabled] {
  color: transparent;
}
";

      var expected = @"
input[type=""text""] {
  font-weight: normal;
}
h2[title] {
  font-size: 100%;
}
[disabled] {
  color: transparent;
}
";

      AssertLess(input, expected);
    }

  }
}