namespace dotless.Test.Specs
{
    using System.Collections.Generic;
    using NUnit.Framework;

    public class ExtendFixture : SpecFixtureBase
    {
        [Test]
        public void ExtendSelector()
        {
            string input = @"nav ul:extend(.inline) {
  background: blue;
}
.inline {
  color: red;
}";
            string expected = @"nav ul {
  background: blue;
}
.inline,
nav ul {
  color: red;
}";
            AssertLess(input, expected);
        }

        [Test]
        public void ExtendRuleSet()
        {
            string input = @"nav ul {
  &:extend(.inline);
  background: blue;
}
.inline {
  color: red;
}";

            string expected = @"nav ul {
  background: blue;
}
.inline,
nav ul {
  color: red;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void ExtendMultiple()
        {
            string input = @".e:extend(.f, .g) { background-color:green; }
.f { color: red; }
.g { color: blue; }
";
            string expected = @".e {
  background-color: green;
}
.f,
.e {
  color: red;
}
.g,
.e {
  color: blue;
}
";
            AssertLess(input, expected);
        }

        [Test]
        public void ExtendSelectorAll()
        {
            string input = @".a.b.test,
.test.c {
  color: orange;
}
.test {
  &:hover {
    color: green;
  }
}

.replacement:extend(.test all) {}";

            string expected = @".a.b.test,
.test.c,
.a.b.replacement,
.replacement.c {
  color: orange;
}
.test:hover,
.replacement:hover {
  color: green;
}";
            AssertLess(input, expected);
        }

        [Test]
        public void IgnoreVariableSelector()
        {
            string input = @".bucket {
  color: blue;
}


.some-class:extend(@{variable}) {} // interpolated selector matches nothing
@variable: bucket;
";


            string expected = @".bucket {
  color: blue;
}
";
                AssertLess(input,expected);;
        }

        [Test]
        public void ExtendInsideBodyRuleset()
        {
            string input = @"
div pre { color:red; }

pre:hover,
.some-class {
  &:extend(div pre);
}";
            string expected = @"div pre,
pre:hover,
.some-class {
  color: red;
}";

            AssertLess(input,expected);
        }

        [Test]
        public void ExtendNestedSmall()
        {
            var input = @".bucket {
  tr { // nested ruleset with target selector
    color: blue;
  }
}
.some-class:extend(.bucket tr) {} // nested ruleset is recognized";

            var expected = @".bucket tr,
.some-class {
  color: blue;
}";
            AssertLess(input,expected);
        }

        [Test]
        public void ExtendNestedLarge()
        {
            var input = @".img-responsive(@display: block) {
  display: @display;
  max-width: 100%; // Part 1: Set a maximum relative to the parent
  height: auto; // Part 2: Scale the height according to the width, otherwise you get stretching
}

.img-responsive {
  .img-responsive();
}

.carousel-inner {
  position: relative;
  overflow: hidden;
  width: 100%;

  > .item {
    display: none;
    position: relative;

    // Account for jankitude on images
    > img,
    > a > img {
      &:extend(.img-responsive);
      line-height: 1;
    }
  }
}";

            var expected = @".img-responsive,
.carousel-inner > .item > img,
.carousel-inner > .item > a > img {
  display: block;
  max-width: 100%;
  height: auto;
}
.carousel-inner {
  position: relative;
  overflow: hidden;
  width: 100%;
}
.carousel-inner > .item {
  display: none;
  position: relative;
}
.carousel-inner > .item > img,
.carousel-inner > .item > a > img {
  line-height: 1;
}

";
            //var output = Evaluate(input, DefaultParser()).Trim().Replace("\r\n", "\n");
            AssertLess(input, expected);
        }

        [Test]
        public void ExtendMediaScoping()
        {
            var input = @"@media print {
  .screenClass:extend(.selector) {}
  .selector {
    color: black;
  }
}
.selector {
  color: red;
}
@media screen {
  .selector {
    color: blue;
  }
}";

            var expected = @"@media print {
  .selector,
  .screenClass {
    color: black;
  }
}
.selector {
  color: red;
}
@media screen {
  .selector {
    color: blue;
  }
}";

            AssertLess(input, expected);
        }

        [Test]
        public void ParentSelector() {
            var input = @"
.btn-primary {
  color: white;
}

a:link {
  color: blue;
}

a:link {
    &.btn-primary:extend(.btn-primary){}
}
";

            var expected = @"
.btn-primary,
a:link.btn-primary {
  color: white;
}
a:link {
  color: blue;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void ParentSelectorWithMultipleOuterSelectors() {
            var input = @"
.btn-primary {
  color: white;
}

a:active,a:visited
{
  color: blue;
}

a:active, a:visited {
    &.btn-primary:extend(.btn-primary){}
}
";

            var expected = @"
.btn-primary,
a:active.btn-primary,
a:visited.btn-primary {
  color: white;
}
a:active,
a:visited {
  color: blue;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void ExtendInMixinDoesNotCauseError() {
            var input = @"
.extension {
  color: red;
}

.mixin() {
    .rule {
        &:extend(.extension all);
    }
}

.mixin();
";
            var expected = @"
.extension,
.rule {
  color: red;
}
";
            AssertLess(input, expected);
        }

        [Test]
        public void ExtendInLoop() {
            var input = @"
.clearfix() {
  &:before,
  &:after {
    content: "" ""; // 1
    display: table; // 2
  }
  &:after {
    clear: both;
  }
}

.make-row() {
  &:extend(.clearfix all);
}

.make-rows() {
  .row(@index) when (@index < 3) {
    .row@{index} {
      height: 10 * @index;
      .make-row();
    }
    .row(@index + 1);
  }
  .row(1);
}

.clearfix
{
  .clearfix();
}

.make-rows();
";

            var expected = @"
.clearfix:before,
.clearfix:after,
.row1:before,
.row2:before,
.row1:after,
.row2:after {
  content: "" "";
  display: table;
}
.clearfix:after,
.row1:after,
.row2:after {
  clear: both;
}
.row1 {
  height: 10;
}
.row2 {
  height: 20;
}
";
            AssertLess(input, expected);
        }

        [Test]
        public void PartialExtendMatchesExactSelectorElementsOnly() {
            var input = @"
.btn-test {
  border-radius: 5px;
}

.no-match:extend(.btn all) { }
";

            var expected = @"
.btn-test {
  border-radius: 5px;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void MediaScopedPartialExtendMatchesExactSelectorElementsOnly() {
            var input = @"
@media (screen) {
  .btn-test {
    border-radius: 5px;
  }

  .no-match:extend(.btn all) { }
}

";

            var expected = @"
@media (screen) {
  .btn-test {
    border-radius: 5px;
  }
}";

            AssertLess(input, expected);
        }
		

        [Test]
        public void ExactExtendWithRuleDefinedInMediaBlock()
        {
            var input = @"
@media print {
  .selector {
    color: black;
  }
}

.test:extend(.selector) { }
";

            var expected = @"
@media print {
  .selector,
  .test {
    color: black;
  }
} ";

            AssertLess(input, expected);
        }

        [Test]
        public void PartialExtendWithRuleDefinedInMediaBlock()
        {
            var input = @"
@media print {
  .parent {
    .selector {
      color: black;
    }
  }
}

.test:extend(.selector all) { }
";

            var expected = @"
@media print {
  .parent .selector,
  .parent .test {
    color: black;
  }
} ";

            AssertLess(input, expected);
        }


        [Test]
        public void PartialExtendWithElementSelector() {
            var input = @"
ul li {
  list-style-type: none; 
}

.foo:extend(li all) { }
";
            var expected = @"
ul li,
ul .foo {
  list-style-type: none;
}";

            AssertLess(input, expected);
        }
    }
}