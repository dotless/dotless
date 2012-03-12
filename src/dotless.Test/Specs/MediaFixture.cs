namespace dotless.Test.Specs
{
    using NUnit.Framework;
    using System.Collections.Generic;

    public class MediaFixture : SpecFixtureBase
    {
        [Test]
        public void MediaDirective()
        {
            var input = @"
@media all and (min-width: 640px) {
  #header {
    background-color: #123456;
  }
}
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void MediaDirective2()
        {
            var input = @"
@media handheld and (min-width: 20em), screen and (min-width: 20em) {
  body {
    max-width: 480px;
  }
}
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void MediaDirectiveEmpty()
        {
            var input = @"
@media only screen and (min-width: 768px) and (max-width: 959px) {
  
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
        public void VariablesInMediaDirective()
        {
            var input = @"
@handheldMinWidth: 15em;
@screenWidth: 20px;
@media handheld and (min-width: @handheldMinWidth), screen and (min-width: @screenWidth) {
  body {
    max-width: 480px;
  }
}
";
            var expected = @"
@media handheld and (min-width: 15em), screen and (min-width: 20px) {
  body {
    max-width: 480px;
  }
}

";

            AssertLess(input, expected);
        }

        [Test]
        public void VariablesInMediaDirective2()
        {
            var input = @"
@smartphone: ~""only screen and (max-width: 200px)"";
@media @smartphone {
  body {
    max-width: 480px;
  }
}
";
            var expected = @"
@media only screen and (max-width: 200px) {
  body {
    max-width: 480px;
  }
}

";

            AssertLess(input, expected);
        }

        [Test]
        public void MediaDirectiveWithDecimal()
        {
            var input = @"
@media only screen and (min--moz-device-pixel-ratio: 1.5) {
  body {
    max-width: 480px;
  }
}
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void MediaDirectiveWithSlash()
        {
            var input = @"
@media only screen and (-o-min-device-pixel-ratio: 3/2) {
  body {
    max-width: 480px;
  }
}
";

            AssertLessUnchanged(input);
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
        public void MediaBubbling1()
        {
            var input = @"
body {
  @media print {
    padding: 20px;
  }
}";
            var expected = @"
@media print {
  body {
    padding: 20px;
  }
}
";
            AssertLess(input, expected);
        }

        [Test]
        public void MediaBubbling2()
        {
            var input = @"
body {
  @media print {
    input {
      padding: 20px;
    }
  }
}";
            var expected = @"
@media print {
  body input {
    padding: 20px;
  }
}
";
            AssertLess(input, expected);
        }

        [Test]
        public void MediaBubbling3()
        {
            var input = @"
    @media print {
        @media (orientation:landscape) {
            body {
                margin-left: 20px;
            }
        }
    }
";
            var expected = @"
@media print {
  
}
@media print and (orientation: landscape) {
  body {
    margin-left: 20px;
  }
}
";
            AssertLess(input, expected);
        }

        [Test]
        public void MediaBubbling4()
        {
            var input = @"
body {
    @media print {
        padding: 20px;

        header {
            background-color: red;
        }

        @media (orientation:landscape) {
            margin-left: 20px;
        }
    }
}
";
            var expected = @"
@media print {
  body {
    padding: 20px;
  }
  body header {
    background-color: red;
  }
}
@media print and (orientation: landscape) {
  body {
    margin-left: 20px;
  }
}
";
            AssertLess(input, expected);
        }

        [Test]
        public void MediaBubbling5()
        {
            var input = @"
body {
    @media a, b and c {
        width: 95%;

        @media x, y {
            width: 100%;
        }
    }
}";
            var expected = @"
@media a, b and c {
  body {
    width: 95%;
  }
}
@media a and x, b and c and x, a and y, b and c and y {
  body {
    width: 100%;
  }
}
";
            AssertLess(input, expected);
        }

        [Test]
        public void MediaMixin1()
        {
            var input = @"
.mediaMixin(@fallback: 200px) {
    background: black;
    @media handheld {
        background: white;

        @media (max-width: @fallback) {
            background: red;
        }
    }
}
.a {
  .mediaMixin(100px);
}
";
            var expected = @"

.a {
  background: black;
}
@media handheld {
  .a {
    background: white;
  }
}
@media handheld and (max-width: 100px) {
  .a {
    background: red;
  }
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void MediaMixin2()
        {
            var input = @"
.mediaMixin(@fallback: 200px) {
    background: black;
    @media handheld {
        background: white;

        @media (max-width: @fallback) {
            background: red;
        }
    }
}
.b {
  .mediaMixin();
}
";
            var expected = @"

.b {
  background: black;
}
@media handheld {
  .b {
    background: white;
  }
}
@media handheld and (max-width: 200px) {
  .b {
    background: red;
  }
}
";

            AssertLess(input, expected);
        }
    }
}