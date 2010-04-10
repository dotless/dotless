using NUnit.Framework;

namespace dotless.Tests.Specs
{
  public class MixinsArgsFixture : SpecFixtureBase
  {
    [Test]
    public void MixinsArgs()
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

.mixiny
(@a: 0, @b: 0) {
  margin: @a;
  padding: @b;
}

.hidden() {
  color: transparent;
}

.two-args {
  color: blue;
  .mixin(2px, 100%);
  .mixina(dotted, 2px);
}

.one-arg {
  .mixin(3px);
}

.no-parens {
  .mixin;
}

.no-args {
  .mixin();
}

.var-args {
  @var: 9;
  .mixin(@var, @var * 2);
}

.multi-mix {
  .mixin(2px, 30%);
  .mixiny(4, 5);
}

.maxa(@arg1: 10, @arg2: #f00) {
  padding: @arg1 * 2px;
  color: @arg2;
}

body {
  .maxa(15);
}

@glob: 5;
.global-mixin(@a:2) {
  width: @glob + @a;
}

.scope-mix {
  .global-mixin(3);
}

.nested-ruleset (@width: 200px) {
    width: @width;
    .column { margin: @width; }
}
.content {
    .nested-ruleset(600px);
}
";

      var expected = @"
.two-args {
  color: blue;
  width: 10px;
  height: 99%;
  border: 2px dotted black;
}
.one-arg {
  width: 15px;
  height: 49%;
}
.no-parens {
  width: 5px;
  height: 49%;
}
.no-args {
  width: 5px;
  height: 49%;
}
.var-args {
  width: 45;
  height: 17%;
}
.multi-mix {
  width: 10px;
  height: 29%;
  margin: 4;
  padding: 5;
}
body {
  padding: 30px;
  color: red;
}
.scope-mix {
  width: 8;
}
.content {
  width: 600px;
}
.content .column {
  margin: 600px;
}
";

      AssertLess(input, expected);
    }
  }
}