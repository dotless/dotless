namespace dotless.Test.Specs
{
    using NUnit.Framework;
    using System.Collections.Generic;

    public class Css3Fixture : SpecFixtureBase
    {
        [Test]
        public void CommaDelimited()
        {
            var input =
                @"
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
            var input =
                @"
@font-face {
  font-family: Headline;
  src: local(Futura-Medium), url(fonts.svg#MyGeometricModern) format(""svg"");
}
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void FontFaceDirectiveInClass()
        {
            var input = @"
#font {
  .test(){
    @font-face {
      font-family: 'MyFont';
      src: url('/css/fonts/myfont.eot');
      src: url('/css/fonts/myfont.eot?#iefix') format('embedded-opentype'),
           url('/css/fonts/myfont.woff') format('woff'),
           url('/css/fonts/myfont.ttf') format('truetype'),
           url('/css/fonts/myfont.svg#reg') format('svg');
    }
    .cl {
      background: red;
    }
  }
}

#font > .test();";

            var expected = @"
@font-face {
  font-family: 'MyFont';
  src: url('/css/fonts/myfont.eot');
  src: url('/css/fonts/myfont.eot?#iefix') format('embedded-opentype'), url('/css/fonts/myfont.woff') format('woff'), url('/css/fonts/myfont.ttf') format('truetype'), url('/css/fonts/myfont.svg#reg') format('svg');
}
.cl {
  background: red;
}
";
            AssertLess(input, expected);
        }

        [Test]
        public void SupportMozDocument()
        {
            // see https://github.com/dotless/dotless/issues/73
            var input =
                @"
@-moz-document url-prefix(""https://github.com"") {
  h1 {
    color: red;
  }
}
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void SupportVendorXDocument()
        {
            var input =
                @"
@-x-document url-prefix(""github.com"") {
  h1 {
    color: red;
  }
}
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void SupportSupports()
        {
            // example from http://www.w3.org/TR/2011/WD-css3-conditional-20110901/
            var input =
                @"
@supports ( box-shadow: 2px 2px 2px black ) or
          ( -moz-box-shadow: 2px 2px 2px black ) or
          ( -webkit-box-shadow: 2px 2px 2px black ) or
          ( -o-box-shadow: 2px 2px 2px black ) {
  .outline {
    color: white;
    box-shadow: 2px 2px 2px black;
    -moz-box-shadow: 2px 2px 2px black;
    -webkit-box-shadow: 2px 2px 2px black;
    -o-box-shadow: 2px 2px 2px black;
  }
}";

            AssertLessUnchanged(input);
        }

        [Test]
        public void NamespaceDirective()
        {
            // see https://github.com/dotless/dotless/issues/27
            var input =
                @"
@namespace lq ""http://example.com/q-markup"";";

            AssertLessUnchanged(input);
        }


        [Test]
        public void KeyFrameDirectiveWebKit()
        {
            var input = @"
@-webkit-keyframes fontbulger {
  0% {
    font-size: 10px;
  }
  30.5% {
    font-size: 15px;
  }
  100% {
    font-size: 12px;
  }
}
#box {
  -webkit-animation: fontbulger 2s infinite;
}";
            AssertLessUnchanged(input);
        }

        [Test]
        public void KeyFrameDirectiveMoz()
        {
            var input = @"
@-moz-keyframes fontbulger {
  0% {
    font-size: 10px;
  }
  30% {
    font-size: 15px;
  }
  100% {
    font-size: 12px;
  }
}
#box {
  -moz-animation: fontbulger 2s infinite;
}";
            AssertLessUnchanged(input);
        }

        [Test]
        public void KeyFrameDirective()
        {
            var input = @"
@keyframes fontbulger {
  0% {
    font-size: 10px;
  }
  30% {
    font-size: 15px;
  }
  100% {
    font-size: 12px;
  }
}
#box {
  animation: fontbulger 2s infinite;
}";
            AssertLessUnchanged(input);
        }

        [Test]
        public void KeyFrameDirective2()
        {
            // would not happen but tests syntax
            var input = @"
@keyframes fontbulger1 {
  from {
    font-size: 10px;
  }
  to {
    font-size: 15px;
  }
  from, to {
    font-size: 12px;
  }
  0%, 100% {
    font-size: 12px;
  }
}
#box {
  animation: fontbulger1 2s infinite;
}";
            AssertLessUnchanged(input);
        }

        [Test]
        public void KeyFrameDirective3()
        {
            var input = @"
@-webkit-keyframes rotate-this {
  0% {
    -webkit-transform: scale(0.6) rotate(0deg);
  }
  50.01% {
    -webkit-transform: scale(0.6) rotate(180deg);
  }
  100% {
    -webkit-transform: scale(0.6) rotate(315deg);
  }
}
#box {
  animation: rotate-this 2s infinite;
}";
            AssertLessUnchanged(input);
        }

        [Test]
        public void KeyFrameDirective4()
        {
            var input = @"
@keyframes rotate-this {
  0%, 1%, 10%, 80%, to {
    -webkit-transform: scale(0.6) rotate(0deg);
  }
  50% {
    -webkit-transform: scale(0.6) rotate(180deg);
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
        public void AttributeSelectorsInsideMixin()
        {
            // See github.com/dotless/issues/65
            var input = @"
.Grid {
    input[type=""checkbox""] {
        margin-right: 4px;
    }
}
";
            var expected = @"
.Grid input[type=""checkbox""] {
  margin-right: 4px;
}
";
            AssertLess(input, expected);
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
            var input =
                @"
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
li:nth-child(4n+1),
li:nth-child(-5n),
li:nth-child(-n+2) {
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
            var input =
                @"
.class {
  background: url(bg1.gif) top left no-repeat, url(bg2.jpg) top right no-repeat;
}
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void Css3UnitsSupported()
        {
            // see http://dev.w3.org/csswg/css3-values/

            List<string> units = new List<string>() { "em", "ex", "ch", "rem", "vw", "vh", "vmin", "vm", "cm", "mm", "%", "in", "pt", "px", "pc", "deg", "grad", "rad", "s", "ms", "fr", "gr", "Hz", "kHz", "dpcm", "dppx" };

            foreach (string unit in units)
            {
                AssertExpression("1" + unit, "2" + unit + " / 2");
            }
        }

        [Test]
        public void FontFaceMixin()
        {
            var input = @"
.def-font(@name) {
    @font-face {
        font-family: @name
    }
}

.def-font(font-a);
.def-font(font-b);";

            var expected = @"
@font-face {
  font-family: font-a;
}
@font-face {
  font-family: font-b;
}
";
            AssertLess(input, expected);
        }

        [Test]
        public void NestedPseudoclassSelectorsWork()
        {
            var input = @"
:not(:nth-child(1)) {
  margin-top: 5px;
}";

            AssertLessUnchanged(input);
        }

        [Test]
        public void PseudoclassParenMatchingWorks()
        {
            var input = @"
audio:not([controls]) {
  // this comment has parens ()
  margin-top: 5px;
}";

            var expected = @"
audio:not([controls]) {
  margin-top: 5px;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void Css3FilterFunctionsPassThrough()
        {
            AssertLessUnchanged("filter: url(resources.svg#c1);");
            AssertLessUnchanged("filter: blur(5px);");
            AssertLessUnchanged("filter: brightness(0.5);");
            AssertLessUnchanged("filter: contrast(200%);");
            AssertLessUnchanged("filter: drop-shadow(16px 16px 10px black);");
            AssertLessUnchanged("filter: grayscale(100%);");
            AssertLessUnchanged("filter: hue-rotate(90deg);");
            AssertLessUnchanged("filter: invert(100%);");
            AssertLessUnchanged("filter: opacity(50%);");
            AssertLessUnchanged("filter: saturate(200%);");
            AssertLessUnchanged("filter: sepia(100%);");
        }

        [Test]
        public void Css3FilterMultipleFunctions()
        {
            AssertLessUnchanged("filter: invert(100%) opacity(50%) saturate(200%);");
            AssertLess("filter: /* test */ invert(100%) /* test */ opacity(50%) /* test */ saturate(200%);","filter: /* test */invert(100%)/* test */ opacity(50%)/* test */ saturate(200%);");
        }

        [Test]
        public void Css3FilterWithEvaluatedValues()
        {
            AssertLess("filter: blur(10px + 5);", "filter: blur(15px);");
        }

#if CSS3EXPERIMENTAL
        [Test]
        public void GridRepeatingPatternSupported()
        {
            //see http://www.w3.org/TR/css3-grid/#example0

            AssertExpressionUnchanged("0 1em (0.5in 5rem 0)[2]");
            AssertExpressionUnchanged("(500px)[2]");
        }

        [Test]
        public void GridRepeatingPatternSupportedWithVars()
        {
            var input = @"
@a : 1em;
@b : 2px;
@c : red;
@d : 10;
.test {
  background: 0 @a (@b 0 @c)[@d];
}
";
            var expected = @"
.test {
  background: 0 1em (2px 0 red)[10];
}
";
            AssertLess(input, expected);
        }
#endif
    }
}