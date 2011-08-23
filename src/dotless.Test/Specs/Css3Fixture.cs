namespace dotless.Test.Specs
{
    using NUnit.Framework;

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
        public void MediaDirective()
        {
            var input = @"
@media all and (min-width: 640px) {
  #header {
    background-color: #0f0;
  }
}
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void MediaDirectiveCanUseVariables()
        {
            var input =
                @"
@var: red;
@media screen {
  color: @var;
  #header {
    background-color: @var;
  }
}
";

            var expected = @"
@media screen {
  color: red;
  #header {
    background-color: red;
  }
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void MediaDirectiveCanDeclareVariables()
        {
            var input =
                @"
@media screen {
  @var: red;
  color: @var;
  #header {
    background-color: @var;
  }
}
";

            var expected = @"
@media screen {
  color: red;
  #header {
    background-color: red;
  }
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void MediaDirectiveCanHavePageDirective1()
        {
            // see https://github.com/dotless/dotless/issues/27
            var input =
                @"
@media print {
  @page {
    margin: 0.5cm;
  }
}
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void MediaDirectiveCanHavePageDirective2()
        {
            var input =
                @"
@media print {
  @page :left {
    margin: 0.5cm;
  }
  
  @page :right {
    margin: 0.5cm;
  }
  
  @page Test:first {
    margin: 1cm;
  }
  
  @page :first {
    size: 8.5in 11in;
    @top-left {
      margin: 1cm;
    }
    
    @top-left-corner {
      margin: 1cm;
    }
    
    @top-center {
      margin: 1cm;
    }
    
    @top-right {
      margin: 1cm;
    }
    
    @top-right-corner {
      margin: 1cm;
    }
    
    @bottom-left {
      margin: 1cm;
    }
    
    @bottom-left-corner {
      margin: 1cm;
    }
    
    @bottom-center {
      margin: 1cm;
    }
    
    @bottom-right {
      margin: 1cm;
    }
    
    @bottom-right-corner {
      margin: 1cm;
    }
    
    @left-top {
      margin: 1cm;
    }
    
    @left-middle {
      margin: 1cm;
    }
    
    @left-bottom {
      margin: 1cm;
    }
    
    @right-top {
      margin: 1cm;
    }
    
    @right-middle {
      content: ""Page "" counter(page);
    }
    
    @right-bottom {
      margin: 1cm;
    }
  }
}
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void MediaDirectiveCanHavePageDirective3()
        {
            var input =
                @"
@media print {
  @page:first {
    margin: 0.5cm;
  }
}";
            var expected =
                @"
@media print {
  @page :first {
    margin: 0.5cm;
  }
}";
            AssertLess(input, expected);
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
  30% {
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
  from,to {
    font-size: 12px;
  }
  0%,100% {
    font-size: 12px;
  }
}
#box {
  animation: fontbulger1 2s infinite;
}";
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
            var input =
                @"
.class {
  background: url(bg1.gif) top left no-repeat, url(bg2.jpg) top right no-repeat;
}
";

            AssertLessUnchanged(input);
        }
    }
}