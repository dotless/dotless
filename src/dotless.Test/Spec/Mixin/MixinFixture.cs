using NUnit.Framework;

namespace dotless.Test.Spec.Mixin
{
    [TestFixture]
    public class MixinFixture : SpecFixtureBase
    {
        [Test]
        public void MixinWithArgs()
        {
            var less =
@".mixin (@a: 1px, @b: 50%) {
  width: @a * 5;
  height: @b - 1%;
}
 
.mixin-arg {
  .mixin(4px, 21%);
}";

            var css =
@".mixin-arg {
  width: 20px;
  height: 20%;
}";

            AssertLess(css, less);
        }

        [Test]
        public void CanPassNamedArguments()
        {
            var less =
@".mixin (@a: 1px, @b: 50%) {
  width: @a * 5;
  height: @b - 1%;
}
 
.named-arg {
  color: blue;
  .mixin(@b: 100%);
}";

            var css =
@".named-arg {
  color: blue;
  width: 5px;
  height: 99%;
}";

            AssertLess(css, less);
        }

        [Test]
        public void CanPassVariablesAsPositionalArgs()
        {
            var less =
@".mixin (@a: 1px, @b: 50%) {
  width: @a * 5;
  height: @b - 1%;
}
 
.class {
  @var: 20px;
  .mixin(@var);
}";

            var css =
@".class {
  width: 100px;
  height: 49%;
}";

            AssertLess(css, less);
        }

        [Test]
        public void CanPassVariablesAsNamedArgs()
        {
            var less =
@".mixin (@a: 1px, @b: 50%) {
  width: @a * 5;
  height: @b - 1%;
}
 
.class {
  @var: 20%;
  .mixin(@b: @var);
}";

            var css =
@".class {
  width: 5px;
  height: 19%;
}";

            AssertLess(css, less);
        }

        [Test]
        public void MixedPositionalAndNamedArguments()
        {
            var less =
@".mixin (@a: 1px, @b: 50%, @c: 50) {
  width: @a * 5;
  height: @b - 1%;
  color: #000000 + @c;
}
 
.mixed-args {
  .mixin(3px, @c: 100);
}";

            var css =
@".mixed-args {
  width: 15px;
  height: 49%;
  color: #646464;
}";

            AssertLess(css, less);
        }

        [Test]
        public void PositionalArgumentsMustAppearBeforeAllNamedArguments()
        {
            var less =
@".mixin (@a: 1px, @b: 50%, @c: 50) {
  width: @a * 5;
  height: @b - 1%;
  color: #000000 + @c;
}
 
.oops {
  .mixin(@c: 100, 3px);
}";

            Assert.That(() => Evaluate(less), Throws.Exception.Message.EqualTo("Positional arguments must appear before all named arguments. in '.mixin(@c: 100, 3px)'"));
        }

        [Test]
        public void ThrowsIfArumentNotFound()
        {
            var less =
@".mixin (@a: 1px, @b: 50%) {
  width: @a * 3;
  height: @b - 1%;
}
 
.override-inner-var {
  .mixin(@var: 6);
}";

            Assert.That(() => Evaluate(less), Throws.Exception.Message.EqualTo("Argument '@var' not found. in '.mixin(@var: 6)'"));
        }

        [Test]
        public void ThrowsIfTooManyArguments()
        {
            var less =
                @"
.mixin (@a: 5) {  width: @a * 5; }

.class { .mixin(1, 2, 3); }";

            Assert.That(() => Evaluate(less), Throws.Exception.Message.EqualTo("Expected at most 1 arguments in '.mixin(1, 2, 3)', found 3"));
        }


        [Test]
        public void MixinWithArgsInsideNamespace()
        {
            var less =
@"#namespace {
  .mixin (@a: 1px, @b: 50%) {
    width: @a * 5;
    height: @b - 1%;
  }
}

.namespace-mixin {
  #namespace .mixin(5px);
}";

            var css =
@".namespace-mixin {
  width: 25px;
  height: 49%;
}";

            AssertLess(css, less);
        }


        [Test, Ignore("Unsupported")]
        public void NestedParameterizedMixins()
        {
            var less =
@".outer(@a: 5) {
  .inner (@b: 10) {
    width: @a + @b;
  }
}

.class1 {
  .outer;
}

.class2 {
  .outer;
  .inner;
}

.class3 {
  .outer .inner;
}

.class4 {
  .outer(1);
  .inner(2);
}

.class5 {
  .outer(2) .inner(4);
}";

            var css =
@".class2 .class3 { width: 15; }
.class4 { width: 3; }
.class5 { width: 6; }";

            AssertLess(css, less);
        }

        [Test]
        public void NestedRulesInsideParameterizedMixinsAreNotOutput()
        {
            var less =
@".mixin(@a: 5) {
  nested-rule {
    width: 5;
  }
}
";

            var css = @"";

            AssertLess(css, less);
        }

        [Test]
        public void CanNestRulesInParameterizedMixins()
        {
            var less =
@".mixin(@a: 5) {
  nested-rule {
    width: 5;
  }
}

.class {
  .mixin();
}
";

            var css = @"
.class nested-rule { width: 5; }
";

            AssertLess(css, less);
        }
        
        [Test]
        public void CanUseVariablesAsDefaultArgumentValues()
        {
            var less =
@"@var: 5px;

.mixin (@a: @var, @b: 50%) {
  width: @a * 5;
  height: @b - 1%;
}


.class {
  .mixin;
}";

            var css =
@".class {
  width: 25px;
  height: 49%;
}";

            AssertLess(css, less);
        }

        [Test]
        public void ArgumentsOverridesVariableInSameScope()
        {
            var less =
@"@a: 10px;

.mixin (@a: 5px, @b: 50%) {
  width: @a * 5;
  height: @b - 1%;
}


.class {
  .mixin;
}";

            var css =
@".class {
  width: 25px;
  height: 49%;
}";

            AssertLess(css, less);
        }

        [Test]
        public void CanUseArgumentsWithSameNameAsVariable()
        {
            var less =
@"@a: 5px;

.mixin (@a: @a, @b: 50%) {
  width: @a * 5;
  height: @b - 1%;
}


.class {
  .mixin;
}";

            var css =
@".class {
  width: 25px;
  height: 49%;
}";

            AssertLess(css, less);
        }

        [Test]
        public void CanNestParameterizedMixins()
        {
            var less = @"
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

            var css = @"
.class {
  width: 12px;
  font-size: 10px;
}";
        
            AssertLess(css, less);
        }

        [Test]
        public void CanNestParameterizedMixinsWithDefaults()
        {
            var less = @"
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

            var css = @"
.class {
  width: 20px;
  font-size: 12px;
}";

            AssertLess(css, less);
        }


        [Test]
        public void CanNestParameterizedMixinsWithSameParameterNames()
        {
            var less = @"
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

            var css = @"
.class {
  width: 16px;
  font-size: 14px;
}";

            AssertLess(css, less);
        }
    }
}