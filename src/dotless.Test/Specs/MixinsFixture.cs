using dotless.Core.Parser.Infrastructure;
using dotless.Test.Plugins;

namespace dotless.Test.Specs
{
    using System;
    using System.Globalization;
    using System.Threading;
    using NUnit.Framework;

    public class MixinsFixture : SpecFixtureBase
    {
        [Test]
        public void Mixins()
        {
            // Todo: split into separate atomic tests.
            var input =
                @"
.mixin { border: 1px solid black; }
.mixout { border-color: orange; }
.borders { border-style: dashed; }

#namespace {
  .borders {
    border-style: dotted;
  }
  .biohazard {
    content: ""death"";
    .man {
      color: transparent;
    }
  }
}
#theme {
  > .mixin {
    background-color: grey;
  }
}
#container {
  color: black;
  .mixin;
  .mixout;
  #theme > .mixin;
}

#header {
  .milk {
    color: white;
    .mixin;
    #theme > .mixin;
  }
  #cookie {
    .chips {
      #namespace .borders;
      .calories {
        #container;
      }
    }
    .borders;
  }
}
.secure-zone { #namespace .biohazard .man; }
.direct {
  #namespace > .borders;
}

";

            var expected =
                @"
.mixin {
  border: 1px solid black;
}
.mixout {
  border-color: orange;
}
.borders {
  border-style: dashed;
}
#namespace .borders {
  border-style: dotted;
}
#namespace .biohazard {
  content: ""death"";
}
#namespace .biohazard .man {
  color: transparent;
}
#theme > .mixin {
  background-color: grey;
}
#container {
  color: black;
  border: 1px solid black;
  border-color: orange;
  background-color: grey;
}
#header .milk {
  color: white;
  border: 1px solid black;
  background-color: grey;
}
#header #cookie {
  border-style: dashed;
}
#header #cookie .chips {
  border-style: dotted;
}
#header #cookie .chips .calories {
  color: black;
  border: 1px solid black;
  border-color: orange;
  background-color: grey;
}
.secure-zone {
  color: transparent;
}
.direct {
  border-style: dotted;
}
";

            AssertLess(input, expected);
        }

        [Test, Ignore("Unsupported")]
        public void CommaSeparatedMixins()
        {
            // Note: http://github.com/cloudhead/less.js/issues/issue/8

            var input =
                @"
.mixina() {
  color: red;
}
.mixinb() {
  color: green;
}

.class {
  .mixina, .mixinb;
}
";

            var expected = @"
.class {
  color: red;
  color: green;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void ChildSelector()
        {
            var input =
                @"
#bundle {
  .mixin {
    padding: 20px;
    color: purple;
  }
}

#header {
  #bundle > .mixin;
}
";

            var expected =
                @"
#bundle .mixin {
  padding: 20px;
  color: purple;
}
#header {
  padding: 20px;
  color: purple;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void MixinNestedRules()
        {
            var input =
                @"
.bundle() {
  p {
    padding: 20px;
    color: purple;
    a { margin: 0; }
  }
}

#header {
  .bundle;
}
";

            var expected = @"
#header p {
  padding: 20px;
  color: purple;
}
#header p a {
  margin: 0;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void MultipleMixins()
        {
            var input = @"
.mixin{
    border:solid 1px red;
}
.mixin{
    color:blue;
}
.mix-me-in{
    .mixin;
}
";

            var expected =
                @"
.mixin {
  border: solid 1px red;
}
.mixin {
  color: blue;
}
.mix-me-in {
  border: solid 1px red;
  color: blue;
}
";

            AssertLess(input, expected);
        }


        [Test]
        public void MixinWithArgs()
        {
            var input =
                @".mixin (@a: 1px, @b: 50%) {
  width: @a * 5;
  height: @b - 1%;
}
 
.mixin-arg {
  .mixin(4px, 21%);
}";

            var expected =
                @".mixin-arg {
  width: 20px;
  height: 20%;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void CanPassNamedArguments()
        {
            var input =
                @".mixin (@a: 1px, @b: 50%) {
  width: @a * 5;
  height: @b - 1%;
}
 
.named-arg {
  color: blue;
  .mixin(@b: 100%);
}";

            var expected =
                @".named-arg {
  color: blue;
  width: 5px;
  height: 99%;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void CanPassNamedArgumentsInDifferentOrder()
        {
            var input =
                @".mixin (@a: 1px, @b: 50%) {
  width: @a * 5;
  height: @b - 1%;
}
 
.named-arg {
  color: blue;
  .mixin(@b: 100%, @a: 2px);
}";

            var expected =
                @".named-arg {
  color: blue;
  width: 10px;
  height: 99%;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void CanPassNamedArgumentsWithoutDefaults()
        {
            var input =
                @".mixin (@a, @b) {
  width: @a * 5;
  height: @b - 1%;
}
 
.named-arg {
  color: blue;
  .mixin(@a: 2px, @b: 100%);
}";

            var expected =
                @".named-arg {
  color: blue;
  width: 10px;
  height: 99%;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void CanPassVariablesAsPositionalArgs()
        {
            var input =
                @".mixin (@a: 1px, @b: 50%) {
  width: @a * 5;
  height: @b - 1%;
}
 
.class {
  @var: 20px;
  .mixin(@var);
}";

            var expected =
                @".class {
  width: 100px;
  height: 49%;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void CanPassVariablesAsNamedArgs()
        {
            var input =
                @".mixin (@a: 1px, @b: 50%) {
  width: @a * 5;
  height: @b - 1%;
}
 
.class {
  @var: 20%;
  .mixin(@b: @var);
}";

            var expected =
                @".class {
  width: 5px;
  height: 19%;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void MixedPositionalAndNamedArguments()
        {
            var input =
                @".mixin (@a: 1px, @b: 50%, @c: 50) {
  width: @a * 5;
  height: @b - 1%;
  color: #000000 + @c;
}
 
.mixed-args {
  .mixin(3px, @c: 100);
}";

            var expected =
                @".mixed-args {
  width: 15px;
  height: 49%;
  color: #646464;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void PositionalArgumentsMustAppearBeforeAllNamedArguments()
        {
            var input =
                @".mixin (@a: 1px, @b: 50%, @c: 50) {
  width: @a * 5;
  height: @b - 1%;
  color: #000000 + @c;
}
 
.oops {
  .mixin(@c: 100, 3px);
}";

            AssertError(
                "Positional arguments must appear before all named arguments.",
                "  .mixin(@c: 100, 3px);",
                8,
                18,
                "  .mixin(@c: 100, 3px);",
                8,
                input);
        }

        [Test]
        public void PassAllVariablesAsNamedArgumentsWhereNoDefaultValues()
        {
            var input = @"
.clb (@a, @b) {
  background-position: @a @b;
}
.cla {
  .clb(@a:23px, @b:12px);
}";

            var expected = @"
.cla {
  background-position: 23px 12px;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void SupportOnePositionArgumentOneDefaultVariableAndOneNamed()
        {
            var input = @"
.clb (@a, @b: 12px, @c) {
  background-position: @a @c;
}
.cla {
  .clb(23px, @c: 12px);
}";

            var expected = @"
.cla {
  background-position: 23px 12px;
}";
            AssertLess(input, expected);
        }

        [Test, Ignore("Unsupported")]
        public void ThrowsIfArumentNotFound()
        {
            var input =
                @".mixin (@a: 1px, @b: 50%) {
  width: @a * 3;
  height: @b - 1%;
}

.override-inner-var {
  .mixin(@var: 6);
}";

            AssertError("Argument '@var' not found. in '.mixin(@var: 6)'", input);
        }

        [Test]
        public void OverrideMixinToAddNonSimpleDefaultArguments()
        {
            // see https://github.com/dotless/dotless/issues/79

            var input = @"
.gradient(@from, @to)
{
    background: -moz-linear-gradient(@from, @to);
}

.gradient(@colour) {
    .gradient(@colour, darken(@colour, 10%));
}

.test {
    .gradient(#aaaaaa);
}";


            var expected = @"
.test {
  background: -moz-linear-gradient(#aaaaaa, #909090);
}
";
            AssertLess(input, expected);
        }

        [Test]
        public void MixinWithArgsInsideNamespace()
        {
            var input =
                @"#namespace {
  .mixin (@a: 1px, @b: 50%) {
    width: @a * 5;
    height: @b - 1%;
  }
}

.namespace-mixin {
  #namespace .mixin(5px);
}";

            var expected =
                @".namespace-mixin {
  width: 25px;
  height: 49%;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void NestedParameterizedMixins1()
        {
            var input =
                @"
.outer(@a: 5) {
  .inner (@b: 10) {
    width: @a + @b;
  }
}

.class {
  .outer;
}
";

            var expected = "";

            AssertLess(input, expected);
        }

        [Test]
        public void NestedParameterizedMixins2()
        {
            var input =
                @"
.outer(@a: 5) {
  .inner (@b: 10) {
    width: @a + @b;
  }
}

.class {
  .outer;
  .inner;
}
";

            var expected = @"
.class {
  width: 15;
}
";

            AssertLess(input, expected);
        }

        [Test, Ignore("Unsupported")]
        public void NestedParameterizedMixins3()
        {
            var input =
                @"
.outer(@a: 5) {
  .inner (@b: 10) {
    width: @a + @b;
  }
}

.class {
  .outer .inner;
}
";

            var expected = @"
.class {
  width: 15;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void NestedParameterizedMixins4()
        {
            var input =
                @"
.outer(@a: 5) {
  .inner (@b: 10) {
    width: @a + @b;
  }
}

.class {
  .outer(1);
  .inner(2);
}
";

            var expected = @"
.class {
  width: 3;
}
";

            AssertLess(input, expected);
        }

        [Test, Ignore("Unsupported")]
        public void NestedParameterizedMixins5()
        {
            var input =
                @"
.outer(@a: 5) {
  .inner (@b: 10) {
    width: @a + @b;
  }
}

.class {
  .outer(2) .inner(4);
}
";

            var expected = @"
.class {
  width: 6;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void NestedRulesInMixinsShouldRespectArguments()
        {
            var input =
                @"
.mixin(@a: 5) {
    .someClass {
        width: @a;
    }
}

.class1 { .mixin(1); }
.class2 { .mixin(2); }
";

            var expected = @"
.class1 .someClass {
  width: 1;
}
.class2 .someClass {
  width: 2;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void MultipleCallsToMixinsContainingMixinCalls()
        {
            var input =
                @"
.mixintest(@a :5px){
    height: @a;
    input{
        .mixintest2(@a);
    }
}

.mixintest2(@a : 10px){
    width: @a;
}

.test{
    .mixintest();
}

.test2{
    .mixintest(15px);
}";

            var expected =
                @"
.test {
  height: 5px;
}
.test input {
  width: 5px;
}
.test2 {
  height: 15px;
}
.test2 input {
  width: 15px;
}";

            AssertLess(input, expected);
        }


        [Test]
        public void CanUseVariablesAsDefaultArgumentValues()
        {
            var input =
                @"@var: 5px;

.mixin (@a: @var, @b: 50%) {
  width: @a * 5;
  height: @b - 1%;
}


.class {
  .mixin;
}";

            var expected =
                @".class {
  width: 25px;
  height: 49%;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void ArgumentsOverridesVariableInSameScope()
        {
            var input =
                @"@a: 10px;

.mixin (@a: 5px, @b: 50%) {
  width: @a * 5;
  height: @b - 1%;
}


.class {
  .mixin;
}";

            var expected =
                @".class {
  width: 25px;
  height: 49%;
}";

            AssertLess(input, expected);
        }

        [Test, Ignore("Infinite Loop - breaks tester")]
        public void CanUseArgumentsWithSameNameAsVariable()
        {
            var input =
                @"@a: 5px;

.mixin (@a: @a, @b: 50%) {
  width: @a * 5;
  height: @b - 1%;
}


.class {
  .mixin;
}";

            var expected =
                @".class {
  width: 25px;
  height: 49%;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void CanNestParameterizedMixins()
        {
            var input =
                @"
.inner(@size: 12px) {
  font-size: @size;
}

.outer(@width: 20px) {
  width: @width;
  .inner(10px);
}

.class {
 .outer(12px);
}";

            var expected = @"
.class {
  width: 12px;
  font-size: 10px;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void CanNestParameterizedMixinsWithDefaults()
        {
            var input =
                @"
.inner(@size: 12px) {
  font-size: @size;
}

.outer(@width: 20px) {
  width: @width;
  .inner();
}

.class {
 .outer();
}";

            var expected = @"
.class {
  width: 20px;
  font-size: 12px;
}";

            AssertLess(input, expected);
        }


        [Test]
        public void CanNestParameterizedMixinsWithSameParameterNames()
        {
            var input =
                @"
.inner(@size: 12px) {
  font-size: @size;
}

.outer(@size: 20px) {
  width: @size;
  .inner(14px);
}

.class {
 .outer(16px);
}";

            var expected = @"
.class {
  width: 16px;
  font-size: 14px;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void IncludesAllMatchedMixins2()
        {
            var input =
                @"
.mixout ('left') { left: 1; }

.mixout ('right') { right: 1; }

.left { .mixout('left'); }
.right { .mixout('right'); }
";

            var expected = @"
.left {
  left: 1;
}
.right {
  right: 1;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void ThrowsIfNoMatchFound()
        {
            var input =
                @"
.mixout ('left') { left: 1; }

.mixout ('right') { right: 1; }

.none { .mixout('top'); }
";

            AssertError(
                "No matching definition was found for `.mixout('top')`",
                ".none { .mixout('top'); }",
                5,
                8,
                input);
        }

        [Test]
        public void ThrowsIfNotDefined()
        {
            var input = ".none { .mixin(); }";

            AssertError(
                ".mixin is undefined",
                ".none { .mixin(); }",
                1,
                8,
                input);
        }

        [Test]
        public void CallSiteCorrectWhenMixinThrowsAnError()
        {
            var divideByZeroException = new DivideByZeroException();

            var input = @"
.mixin(@a: 5px) {
  width: 10px / @a;
}
.error {
  .mixin(0px);
}";

            AssertError(
                divideByZeroException.Message,
                "  width: 10px / @a;",
                2,
                14,
                "  .mixin(0px);",
                5,
                input);
        }

        [Test]
        public void IncludesAllMatchedMixins3()
        {
            var input =
                @"
.border (@side, @width) {
    color: black;
    .border-side(@side, @width);
}
.border-side (left, @w) {
    border-left: @w;
}
.border-side (right, @w) {
    border-right: @w;
}

.border-right {
    .border(right, 4px);    
}
.border-left {
    .border(left, 4px);    
}
";

            var expected =
                @"
.border-right {
  color: black;
  border-right: 4px;
}
.border-left {
  color: black;
  border-left: 4px;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void InnerMixinEvaluatedCorrectly()
        {
            var input =
                @"
.inner-mixin(@width) {
    width: @width;
}
.mixin() {
    span {
        color: red;
        .inner-mixin(30px);
    }
}

#header {
    .mixin();
}";

            var expected = @"
#header span {
  color: red;
  width: 30px;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void InnerMixinsFindInnerVariables()
        {
            var input =
                @"
.inner-mixin(@width) {
    width: @width;
}
.mixin() {
    span {
        @var: 20px;
        .inner-mixin(@var);
    }
}

#header {
    .mixin();
}";

            var expected = @"
#header span {
  width: 20px;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void ThrowsIfMixinNotFound()
        {
            var input =
                @"
.class {
  .mixin();
}
";
            AssertError(".mixin is undefined", "  .mixin();", 2, 2, input);
        }

        [Test]
        public void DontCacheFunctions()
        {
            var input =
                @"
.margin(@t, @r) {
  margin: formatString(""{0} {1}"", @t, @r);
}
ul.bla {
  .margin(10px, 15px);
}
ul.bla2 {
  .margin(0, 0);
}";

            var expected = @"
ul.bla {
  margin: 10px 15px;
}
ul.bla2 {
  margin: 0 0;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void MixinsKeepImportantKeyword()
        {
            var input =
                @"
.important-mixin(@colour: #FFFFFF) {
  color: @colour !important;
}

important-rule {
  .important-mixin(#3f3f3f);
}
";

            var expected = @"
important-rule {
  color: #3f3f3f !important;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void ShortMixinDoesntMatchLongerSelectors()
        {
            var input =
            @"
#test {
  .mixin();
}

.mixin { color: red; }
.mixin:after, .dummy { color: green; }
.mixin .inner, .dummy { color: blue; }
";

            var expected =
                @"
#test {
  color: red;
}
.mixin {
  color: red;
}
.mixin:after,
.dummy {
  color: green;
}
.mixin .inner,
.dummy {
  color: blue;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void CanCallMixinFromWithinInnerRuleset()
        {
            var input =
            @"
#mybox {
  .box;
}
.box {
  .square();
}
.square() {
  width: 10px;
  height: 10px;
}
";

            var expected =
                @"
#mybox {
  width: 10px;
  height: 10px;
}
.box {
  width: 10px;
  height: 10px;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void CanResolveMixinsInSameScopeAsMixinDefinition()
        {
            var input =
            @"
#ns {
  .square() {
    width: 10px;
    height: 10px;
  }
  .box() {
    .square();
  }
}

#mybox {
  #ns > .box();
}
";

            var expected =
                @"
#mybox {
  width: 10px;
  height: 10px;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void CanResolveVariablesInSameScopeAsMixinDefinition()
        {
            var input =
            @"
#ns {
  @width: 10px;
  .box() {
    width: @width;
  }
}

#mybox {
  #ns > .box();
}
";

            var expected =
                @"
#mybox {
  width: 10px;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void IncludeAllMixinsInSameScope()
        {
            var input =
            @"
#ns {
  .mixin() { color: red; }
}
#ns {
  .mixin() { color: blue; }
  .box {
    #ns > .mixin();
  }
}
";

            var expected =
                @"
#ns .box {
  color: red;
  color: blue;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void StringMixinArgument()
        {

            var input = @"
.mixin(@val) {
  property: formatString(@val);
}
.box {
  .mixin( ""5px 5px 10px rgba(0,0,0,0.3)"" );
}";

            var output = @"
.box {
  property: 5px 5px 10px rgba(0,0,0,0.3);
}";

            AssertLess(input, output);
        }

        [Test]
        public void MultipleCallsToMixinsUsingAndHoisting()
        {
            // bug https://github.com/dotless/dotless/issues/78
            var input =
                @"
@host: ""https://github.com/"";
@grey8: #f5f5f5;
@grey5: #ccc;
@colorA: #E5225B;
@colorB: #C7C823;

.buttonIcon(@filename) {
    &.fancy {
        @imgbg: formatString(""url({0}images/icons/{1})"", @host, @filename);
        &:hover {
            background-image: @imgbg, formatString(""-webkit-gradient(linear, 0% 0%, 0% 100%, from({0}), to({1}))"", @colorA + 30%, @colorA - 20%);
            background-image: @imgbg, formatString(""-moz-linear-gradient(0% 100% 90deg,{1}, {0})"", @colorA + 30%, @colorA - 20%);
        }
    }
}

.button, button, input[type=""submit""] {
    &.lefticon.icon-tick {
        .buttonIcon(""fugue/tick.png"");
    }
    &.lefticon.icon24-tick.extralarge {
        .buttonIcon(""fugue/icons-24/tick.png"");
    }
}";

            var expected =
                @"
.button.lefticon.icon-tick.fancy:hover,
button.lefticon.icon-tick.fancy:hover,
input[type=""submit""].lefticon.icon-tick.fancy:hover {
  background-image: url(https://github.com/images/icons/fugue/tick.png), -webkit-gradient(linear, 0% 0%, 0% 100%, from(#ff4079), to(#d10e47));
  background-image: url(https://github.com/images/icons/fugue/tick.png), -moz-linear-gradient(0% 100% 90deg,#d10e47, #ff4079);
}
.button.lefticon.icon24-tick.extralarge.fancy:hover,
button.lefticon.icon24-tick.extralarge.fancy:hover,
input[type=""submit""].lefticon.icon24-tick.extralarge.fancy:hover {
  background-image: url(https://github.com/images/icons/fugue/icons-24/tick.png), -webkit-gradient(linear, 0% 0%, 0% 100%, from(#ff4079), to(#d10e47));
  background-image: url(https://github.com/images/icons/fugue/icons-24/tick.png), -moz-linear-gradient(0% 100% 90deg,#d10e47, #ff4079);
}";

            AssertLess(input, expected);
        }

        [Test]
        public void MultipleCallsToMixinsUsingAndHoistingSimple()
        {
            // bug https://github.com/dotless/dotless/issues/78
            var input =
                @"
.test(@arg) {
    .outer {
        .inner {
            border: @arg;        
        }
    }
}

.one {
    .test(1);
}
.two {
    .test(2);
}
";

            var expected =
                @"
.one .outer .inner {
  border: 1;
}
.two .outer .inner {
  border: 2;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void MixinMatchingAllowsMultiples()
        {
            var input = @"
.bo,
.bar {
  width: 100%;
}
.bo {
  border: 1px;
}
.extended {
  .bo;
}
.foo .bar {
  .bar;
}
";
            var expected = @"
.bo,
.bar {
  width: 100%;
}
.bo {
  border: 1px;
}
.extended {
  width: 100%;
  border: 1px;
}
.foo .bar {
  width: 100%;
}
";
            AssertLess(input, expected);
        }

        [Test]
        public void MixinImportant()
        {
            var input = @"
.mixin (9) {
  border: 9 !important;  
}
.mixin (@a: 0) {
  border: @a;
  boxer: 1, @a;
}

.class {
  .mixin(1);
  .mixin(2) !important;
  .mixin(3);
  .mixin(4) !important;
  .mixin(5);
  .mixin !important;
  .mixin(9);
}";
            var expected = @"
.class {
  border: 1;
  boxer: 1, 1;
  border: 2 !important;
  boxer: 1, 2 !important;
  border: 3;
  boxer: 1, 3;
  border: 4 !important;
  boxer: 1, 4 !important;
  border: 5;
  boxer: 1, 5;
  border: 0 !important;
  boxer: 1, 0 !important;
  border: 9 !important;
  border: 9;
  boxer: 1, 9;
}";
            AssertLess(input, expected);
        }

        [Test]
        public void MixinImportantRecursive()
        {
            var input = @"
.x
{
    .test !important;
}

.test
{
	color: red;
    &.testinner
    {
        .anothermixin;
        color: blue;
        &.testinner2
        {
            color: green;
        }
        
        a
        {
            color: orange;
        }
    }
}

.anothermixin
{
    background-color: black;
}";
            var expected = @"
.x {
  color: red !important;
}
.x.testinner {
  background-color: black !important;
  color: blue !important;
}
.x.testinner.testinner2 {
  color: green !important;
}
.x.testinner a {
  color: orange !important;
}
.test {
  color: red;
}
.test.testinner {
  background-color: black;
  color: blue;
}
.test.testinner.testinner2 {
  color: green;
}
.test.testinner a {
  color: orange;
}
.anothermixin {
  background-color: black;
}";
            AssertLess(input, expected);
        }

        [Test]
        public void MixinCallingSameName()
        {
            // attempt to reproduce bug #136
            var input = @"
.clearfix() {
  // For IE 6/7 (trigger hasLayout)
  zoom:1; 
  // For modern browsers
  &:before {
    content:"""";
    display:table;
  }
  &:after {
    content:"""";
    display:table;
  }
  &:after {
    clear:both;
  }
}
.clearfix { 
  .clearfix();
}";
            var expected = @"
.clearfix {
  zoom: 1;
}
.clearfix:before {
  content: """";
  display: table;
}
.clearfix:after {
  content: """";
  display: table;
}
.clearfix:after {
  clear: both;
}
";
            AssertLess(input, expected);
        }

        [Test]
        public void DuplicatesRemovedFromMixinCall()
        {
            var input = @"
.test() {
  background: none;
  color: red;
  background: none;
}

.test2 {
  .test();
  .test;
}";

            var expected = @"
.test2 {
  background: none;
  color: red;
}";
            AssertLess(input, expected);
        }

        [Test]
        public void TestMixinCallIncorrectlyRecognisedLessJsBug901()
        {
            var input = @"
.mixin_def(@url, @position){
    background-image: @url;
    background-position: @position;
}
.error{
  @s: ""/"";
  .mixin_def( ""@{s}a.png"", center center);
}";
            var expected = @"
.error {
  background-image: ""/a.png"";
  background-position: center center;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void MixinUsedInsideSelectorWithInsideSameNameAndInsideSiblingSelector()
        {
            // This relates to https://github.com/dotless/dotless/issues/136, the bare minimum reproduce case appears to be a mixin (eg. ".clearfix()"),
            // followed by a selector (eg. ".panel-body") that imports that mixin follow by a selector whose name matches the mixin's name (".clearfix")
            // that also imports that mixin. Previously this would lead to a stack overflow when the ".panel-body" selector was evaluated. Note that if
            // only the ".clearfix()" mixin and the ".clearfix" selector are present then the stack overflow does not occur, it is the ".panel-body"
            // selector that triggers it.
            var input =
                @"
.clearfix() {
  color: red;
}

.panel-body {
  .clearfix();
}

.clearfix {
  .clearfix();
}
";

            var expected = @"
.panel-body {
  color: red;
}
.clearfix {
  color: red;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void MultipleElementSelectorMixin()
        {
            // Previously, dotLess would require that mixins be selectors with single elements (eg. ".dropdown-menu") since the matching algorithm
            // would try to match only a single element and then drill down into the selector's rules for any additional elements.. Bootstrap uses
            // multi-element mixin selectors (eg. ".pull-right > .dropdown"), the drilling down should only be done if the selector does not already
            // fully match the specified selector.
            var input = @".pull-right > .dropdown-menu {
  right: 0;
}

.navbar-right {
  .dropdown-menu {
    .pull-right > .dropdown-menu();
  }
}";

            var expected = @".pull-right > .dropdown-menu {
  right: 0;
}
.navbar-right .dropdown-menu {
  right: 0;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void ImplicitMixinWithSameNameAsExplicitUnaryMixinWorks()
        {
            var input = @"
.link-reset {
  text-decoration: none !important;
}

.link-reset (@border) when (@border = noborder) {
  border: 0 none;
}

.foo {
    .link-reset;
}

.bar {
    .link-reset(noborder);
}
";
            var expected = @"
.link-reset {
  text-decoration: none !important;
}
.foo {
  text-decoration: none !important;
}
.bar {
  border: 0 none;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void MixinCallsInNestedRulesetsHaveCorrectVariableScope()
        {
            var input = @"
.opacity(@opacity) {
  opacity: @opacity;
  // IE8 filter
  @opacity-ie: (@opacity * 100);
  filter: ~""alpha(opacity=@{opacity-ie})"";
}

.test {
  .opacity(.2);
  .nested {
    .opacity(.5);
  }
}";

            var expected = @"
.test {
  opacity: 0.2;
  filter: alpha(opacity=20);
}
.test .nested {
  opacity: 0.5;
  filter: alpha(opacity=50);
}";

            AssertLess(input, expected);
        }

        [Test]
        public void OutputMinificationDoesNotBreakMixinCalls()
        {
            var input = @"
.pull-right > .dropdown-menu {
  right: 0;
  left: auto;
}

@media (min-width: 768px) {
  .navbar-right {
    .dropdown-menu {
      .pull-right > .dropdown-menu();
    }
  }
}";

            var expected = @"
.pull-right>.dropdown-menu{right:0;left:auto}@media (min-width:768px){.navbar-right .dropdown-menu{right:0;left:auto}}";

            DefaultEnv = () =>
            {
                var env = new Env();
                env.Compress = true;
                return env;
            };

            AssertLess(input, expected);
        }

        [Test]
        public void SemicolonAsSeparatorAllowsListArguments()
        {
            var input = @"
.mix(@list1, @list2) {
    test: @list1;
    test2: @list2;
}

.test {
    .mix(1, 2, 3; 4, 5, 6)
}
";
            var expected = @"
.test {
  test: 1, 2, 3;
  test2: 4, 5, 6;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void DummySemicolonInArgumentListAllowsUnaryCallWithListArgument()
        {
            var input = @"
.mix(@list) {
    test: @list;
}

.test {
    .mix(1, 2, 3;)
}
";
            var expected = @"
.test {
  test: 1, 2, 3;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void SemicolonAsArgumentSeparator()
        {
            var input = @"
.mix(@p1, @p2) {
    test: @p1;
    test2: @p2;
}

.test {
    .mix(1; 2)
}
";

            var expected = @"
.test {
  test: 1;
  test2: 2;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void RulesetDefinedWithParentSelectorIsCallableAsMixin()
        {
            var input = @"
.foo {
  &-bar {
    color: blue;
  }
}

.test {
  .foo-bar;
}
";

            var expected = @"
.foo-bar {
  color: blue;
}
.test {
  color: blue;
}";

            AssertLess(input, expected);
        }
    }
}