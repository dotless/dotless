using System.Collections.Generic;
using dotless.Core.Exceptions;

namespace dotless.Core.Test.Specs
{
    using NUnit.Framework;

    public class SelectorsFixture : SpecFixtureBase
    {
        [Test]
        public void ParentSelector1()
        {
            var input =
                @"
h1, h2, h3 {
  a, p {
    &:hover {
      color: red;
    }
  }
}
";

            var expected =
                @"
h1 a:hover,
h2 a:hover,
h3 a:hover,
h1 p:hover,
h2 p:hover,
h3 p:hover {
  color: red;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void ParentSelector2()
        {
            // Note: http://github.com/cloudhead/less.js/issues/issue/9

            var input =
                @"
a {
  color: red;

  &:hover { color: blue; }

  div & { color: green; }

  p & span { color: yellow; }
}
";

            var expected =
                @"
a {
  color: red;
}
a:hover {
  color: blue;
}
div a {
  color: green;
}
p a span {
  color: yellow;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void ParentSelector3()
        {
            // Note: http://github.com/cloudhead/less.js/issues/issue/9

            var input =
                @"
.foo {
  .bar, .baz {
    & .qux {
      display: block;
    }
    .qux & {
      display:inline;
    }
  }
}
";

            var expected =
                @"
.foo .bar .qux,
.foo .baz .qux {
  display: block;
}
.qux .foo .bar,
.qux .foo .baz {
  display: inline;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void ParentSelector4()
        {
            var input =
                @"
.foo {
  .bar, .baz {
    .qux& {
      display:inline;
    }
  }
}
";

            var expected =
                @"
.qux.foo .bar,
.qux.foo .baz {
  display: inline;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void ParentSelector5()
        {
            //https://github.com/cloudhead/less.js/issues/774
            var input =
                @"
.b {
 &.c {
  .a& {
   color: red;
  }
 }
}
";

            var expected =
                @"
.a.b.c {
  color: red;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void ParentSelector6()
        {
            // https://github.com/cloudhead/less.js/issues/299

            var input =
                @"
.margin_between(@above, @below) {
     * + & { margin-top: @above; }
     legend + & { margin-top: 0; }
     & + * { margin-top: @below; }
}
h1 { .margin_between(25px, 10px); }
h2 { .margin_between(20px, 8px); }
h3 { .margin_between(15px, 5px); }";

            var expected =
                @"
* + h1 {
  margin-top: 25px;
}
legend + h1 {
  margin-top: 0;
}
h1 + * {
  margin-top: 10px;
}
* + h2 {
  margin-top: 20px;
}
legend + h2 {
  margin-top: 0;
}
h2 + * {
  margin-top: 8px;
}
* + h3 {
  margin-top: 15px;
}
legend + h3 {
  margin-top: 0;
}
h3 + * {
  margin-top: 5px;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void ParentSelector7()
        {
            // https://github.com/cloudhead/less.js/issues/749

            var input =
                @"
.b {
 .c & {
  &.a {
   color: red;
  }
 }
}
";

            var expected =
                @"
.c .b.a {
  color: red;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void ParentSelector8()
        {
            var input = @"
.p {
  .foo &.bar {
    color: red;
  }
}
";
            var expected = @"
.foo .p.bar {
  color: red;
}
";
            AssertLess(input, expected);
        }

        [Test]
        public void ParentSelector9()
        {
            var input = @"
.p {
  .foo&.bar {
    color: red;
  }
}
";
            var expected = @"
.foo.p.bar {
  color: red;
}
";
            AssertLess(input, expected);
        }

        [Test]
        public void ParentSelectorCombinators()
        {
            // Note: https://github.com/dotless/dotless/issues/171

            var input =
                @"
.foo {
  .foo + & {
    background: amber;
  }
  & + & {
    background: amber;
  }
}
";

            var expected =
                @"
.foo + .foo {
  background: amber;
}
.foo + .foo {
  background: amber;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void ParentSelectorMultiplied()
        {
            var input =
                @"
.foo, .bar {
  & + & {
    background: amber;
  }
}
";

            var expected =
                @"
.foo + .foo,
.foo + .bar,
.bar + .foo,
.bar + .bar {
  background: amber;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void ParentSelectorMultipliedDouble()
        {
            var input =
                @"
.foo, .bar {
  a, b {
    & > & {
      background: amber;
    }
  }
}
";

            var expected =
                @"
.foo a > .foo a,
.foo a > .bar a,
.foo a > .foo b,
.foo a > .bar b,
.bar a > .foo a,
.bar a > .bar a,
.bar a > .foo b,
.bar a > .bar b,
.foo b > .foo a,
.foo b > .bar a,
.foo b > .foo b,
.foo b > .bar b,
.bar b > .foo a,
.bar b > .bar a,
.bar b > .foo b,
.bar b > .bar b {
  background: amber;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void IdSelectors()
        {
            var input =
                @"
#all { color: blue; }
#the { color: blue; }
#same { color: blue; }
";

            var expected = @"
#all {
  color: blue;
}
#the {
  color: blue;
}
#same {
  color: blue;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void Tag()
        {
            var input = @"
td {
  margin: 0;
  padding: 0;
}
";

            var expected = @"
td {
  margin: 0;
  padding: 0;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void TwoTags()
        {
            var input = @"
td,
input {
  line-height: 1em;
}
";

            AssertLessUnchanged(input);
        }

        [Test]
        public void MultipleTags()
        {
            var input =
                @"
ul, li, div, q, blockquote, textarea {
  margin: 0;
}
";

            var expected = @"
ul,
li,
div,
q,
blockquote,
textarea {
  margin: 0;
}
";

            AssertLess(input, expected);
        }


        [Test]
        public void DecendantSelectorWithTabs()
        {
            var input = "td \t input { line-height: 1em; }";

            var expected = @"
td input {
  line-height: 1em;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void NestedCombinedSelector()
        {

            var input = @"
#parentRuleSet {
  .selector1.selector2 { position: fixed; }
}";

            var expected = @"
#parentRuleSet .selector1.selector2 {
  position: fixed;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void DynamicSelectors()
        {

            var input = @"
@a: 2;
a:nth-child(@a) {
  border: 1px;
}";

            var expected = @"
a:nth-child(2) {
  border: 1px;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void PseudoSelectors()
        {
            // from less.js bug 663

            var input = @"
.other ::fnord { color: red }
.other::fnord { color: red }
.other {
  ::bnord {color: red }
  &::bnord {color: red }
}";

            var expected = @"
.other ::fnord {
  color: red;
}
.other::fnord {
  color: red;
}
.other ::bnord {
  color: red;
}
.other::bnord {
  color: red;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void ParentSelectorWhenNoParentExists1()
        {
            //comes up in bootstrap
            var input = @"
.placeholder(@color) {
  &:-moz-placeholder {
    color: @color;
  }
  &:-ms-input-placeholder {
    color: @color;
  }
  &::-webkit-input-placeholder {
    color: @color;
  }
}
.placeholder(red);
";
            var expected = @"
:-moz-placeholder {
  color: red;
}
:-ms-input-placeholder {
  color: red;
}
::-webkit-input-placeholder {
  color: red;
}
";
            AssertLess(input, expected);
        }

        [Test]
        public void ParentSelectorWhenNoParentExists2()
        {
            var input = @"
.placeholder(@color) {
  .foo &.bar {
    color: @color;
  }
}
.placeholder(red);
";
            var expected = @"
.foo .bar {
  color: red;
}
";
            AssertLess(input, expected);
        }

        [Test]
        public void VariableSelector()
        {
            var input = @"// Variables
@mySelector: banner;

// Usage
.@{mySelector} {
  font-weight: bold;
  line-height: 40px;
  margin: 0 auto;
}";

            var expected = @".banner {
  font-weight: bold;
  line-height: 40px;
  margin: 0 auto;
}";
            AssertLess(input,expected);
        }

        [Test]
        public void VariableSelectorInRecursiveMixin()
        {
            var input = @"
.span (@index) {
  margin: @index;
}
.spanX (@index) when (@index > 0) {
    .span@{index} { .span(@index); }
    .spanX(@index - 1);
}
.spanX(2);
";
            var expected = @"
.span2 {
  margin: 2;
}
.span1 {
  margin: 1;
}
";
            AssertLess(input, expected);
        }

        [Test]
        public void AttributeCharacterTest()
        {
            var input = @"
.item[data-cra_zy-attr1b-ut3=bold] {
  foo: bar;
}";
            AssertLessUnchanged(input);
        }

        [Test]
        public void SelectorInterpolation1()
        {
            var input = @"
@num: 2;
:nth-child(@{num}):nth-child(@num) {
  foo: bar;
}";
            var expected = @"
:nth-child(2):nth-child(2) {
  foo: bar;
}";
            AssertLess(input, expected);
        }

        [Test]
        public void SelectorInterpolation2()
        {
            var input = @"
@theme: blood;
.@{theme} {
  foo: bar;
}";
            var expected = @"
.blood {
  foo: bar;
}";
            AssertLess(input, expected);
        }

        [Test]
        public void SelectorInterpolationAndParent()
        {
            var input = @"
@theme: blood;
.@{theme} {
  .red& {
    foo: bar;
  }
}";
            var expected = @"
.red.blood {
  foo: bar;
}";
            AssertLess(input, expected);
        }

        [Test]
        public void SelectorInterpolationAndParent2()
        {
            var input = @"
@theme: blood;
.@{theme} {
  .red& {
    foo: bar;
  }
}";
            var expected = @"
.red.blood {
  foo: bar;
}";
            AssertLess(input, expected);
        }

        [Test]
        public void EscapedSelector()
        {
            var input = @"
#odd\:id,
[odd\.attr] {
  foo: bar;
}";
            AssertLessUnchanged(input);
        }
        
        [Test]
        public void MixedCaseAttributeSelector()
        {
            var input = @"
img[imgType=""sort""] {
  foo: bar;
}";
            AssertLessUnchanged(input);
        }

        [Test]
        public void MultipleIdenticalSelectorsAreOutputOnlyOnce()
        {
            var input = @"
.sel1, .sel2 {
  float: left;
}

.test:extend(.sel1 all, .sel2 all) { }
";

            var expected = @"
.sel1,
.sel2,
.test {
  float: left;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void ParentSelectorWithVariableInterpolation() {
            var input = @"
.test {
  @var: 1;
  &-@{var} {
    color: black;
  }
}
";

            var expected = @"
.test-1 {
  color: black;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void ParentSelectorWithVariableInterpolationIsCallable() {
            var input = @"
.test {
  @var: 1;
  &-@{var} {
    color: black;
  }
}

.call {
  .test-1;
}
";

            var expected = @"
.test-1 {
  color: black;
}
.call {
  color: #000000;
}";

            AssertLess(input, expected);
        }
    }
}
