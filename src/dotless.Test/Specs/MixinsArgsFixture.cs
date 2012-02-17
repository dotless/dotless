namespace dotless.Test.Specs
{
    using NUnit.Framework;

    public class MixinsArgsFixture : SpecFixtureBase
    {
        [Test]
        public void MixinsArgsHidden()
        {
            var input =
                @"
.hidden() {
  color: transparent;
}

#hidden {
  .hidden();
  .hidden;
}
";

            var expected =
                @"
#hidden {
  color: transparent;
  color: transparent;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void MixinsArgsTwoArgs()
        {
            var input =
                @"
.mixin (@a: 1px, @b: 50%) {
  width: @a * 5;
  height: @b - 1%;
}

.mixina (@style, @width, @color: black) {
    border: @width @style @color;
}

.two-args {
  color: blue;
  .mixin(2px, 100%);
  .mixina(dotted, 2px);
}
";

            var expected =
                @"
.two-args {
  color: blue;
  width: 10px;
  height: 99%;
  border: 2px dotted black;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void MixinCallError()
        {
            var input =
                @"
.mixin (@a, @b) {
  width: @a;
  height: @b;
}

@c: 1px;

.one-arg {
  .mixin(@c);
}
";

            AssertError(@"
No matching definition was found for `.mixin(1px)` on line 9:
  [8]: .one-arg {
  [9]:   .mixin(@c);
       --^
 [10]: }", input);
        }

        [Test]
        public void MixinsArgsOneArg()
        {
            // Todo: split into separate atomic tests.
            var input =
                @"
.mixin (@a: 1px, @b: 50%) {
  width: @a * 5;
  height: @b - 1%;
}

.one-arg {
  .mixin(3px);
}
";

            var expected =
                @"
.one-arg {
  width: 15px;
  height: 49%;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void MixinsArgsNoParens()
        {
            var input =
                @"
.mixin (@a: 1px, @b: 50%) {
  width: @a * 5;
  height: @b - 1%;
}
.no-parens {
  .mixin;
}
";

            var expected =
                @"
.no-parens {
  width: 5px;
  height: 49%;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void MixinsArgsNoArgs()
        {
            var input =
                @"
.mixin (@a: 1px, @b: 50%) {
  width: @a * 5;
  height: @b - 1%;
}

.no-args {
  .mixin();
}
";

            var expected =
                @"
.no-args {
  width: 5px;
  height: 49%;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void MixinsArgsVariableArgs()
        {
            var input =
                @"
.mixin (@a: 1px, @b: 50%) {
  width: @a * 5;
  height: @b - 1%;
}

.var-args {
  @var: 9;
  .mixin(@var, @var * 2);
}
";

            var expected =
                @"
.var-args {
  width: 45;
  height: 17%;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void MixinsArgsMulti()
        {
            var input =
                @"
.mixin (@a: 1px, @b: 50%) {
  width: @a * 5;
  height: @b - 1%;
}

.mixiny
(@a: 0, @b: 0) {
  margin: @a;
  padding: @b;
}

.multi-mix {
  .mixin(2px, 30%);
  .mixiny(4, 5);
}
";

            var expected =
                @"
.multi-mix {
  width: 10px;
  height: 29%;
  margin: 4;
  padding: 5;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void MixinsArgs()
        {
            var input =
                @"
.maxa(@arg1: 10, @arg2: #f00) {
  padding: @arg1 * 2px;
  color: @arg2;
}

body {
  .maxa(15);
}
";

            var expected =
                @"
body {
  padding: 30px;
  color: red;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void MixinsArgsScopeMix()
        {
            var input =
                @"
@glob: 5;
.global-mixin(@a:2) {
  width: @glob + @a;
}

.scope-mix {
  .global-mixin(3);
}
";

            var expected =
                @"
.scope-mix {
  width: 8;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void MixinsArgsNestedRuleset()
        {
            var input =
                @"
.nested-ruleset (@width: 200px) {
    width: @width;
    .column { margin: @width; }
}
.content {
    .nested-ruleset(600px);
}
";

            var expected =
                @"
.content {
  width: 600px;
}
.content .column {
  margin: 600px;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void CanEvaluateCommaSepearatedExpressions()
        {
            var input =
                @"
.fonts (@main: 'Arial') {
    font-family: @main, sans-serif;
}

.class {
    .fonts('Helvetica');
}";

            var expected = @"
.class {
  font-family: 'Helvetica', sans-serif;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void CanUseVariablesInsideMixins()
        {
            var input = @"
@var: 5px;
.mixin() {
    width: @var, @var, @var;
}
.div {
    .mixin;
}";

            var expected = @"
.div {
  width: 5px, 5px, 5px;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void CanUseSameVariableName()
        {
            var input =
                @"
.same-var-name2(@radius) {
  radius: @radius;
}
.same-var-name(@radius) {
  .same-var-name2(@radius);
}
#same-var-name {
  .same-var-name(5px);
}
";

            var expected = @"
#same-var-name {
  radius: 5px;
}
";
            AssertLess(input, expected);
        }

        [Test]
        public void MixinArgsHashMixin()
        {
            var input = @"
#id-mixin () {
    color: red;
}
.id-class {
    #id-mixin();
    #id-mixin;
}
";

            var expected = @"
.id-class {
  color: red;
  color: red;
}
";
            AssertLess(input, expected);
        }

        [Test]
        public void MixinArgsArgumentsGiven()
        {
            var input =
                @"
.mixin-arguments (@width: 0px) {
    border: @arguments;
}

.arguments {
    .mixin-arguments(1px, solid, black);
}
";

            var expected = @"
.arguments {
  border: 1px solid black;
}
";
            AssertLess(input, expected);
        }

        [Test]
        public void MixinArgsArgumentsEmpty()
        {
            var input =
                @"
.mixin-arguments (@width: 0px) {
    border: @arguments;
}
.arguments2 {
    .mixin-arguments();
}";

            var expected = @"
.arguments2 {
  border: 0px;
}";
            AssertLess(input, expected);
        }

        [Test]
        public void MixinArgsExceptCurlyBracketString()
        {
            var input =
                @"
.mixin-arguments (@width: 0px) {
    border: @arguments;
}
.arguments2 {
    .mixin-arguments(""{"");
}";

            var expected = @"
.arguments2 {
  border: ""{"";
}";
            AssertLess(input, expected);
        }

    }
}