using NUnit.Framework;

namespace dotless.Test.Spec
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
 
.named-arg {
  .mixin(4px, 21%);
}";

            var css =
@".named-arg {
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

        [Test, Ignore]
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

            Assert.That(() => Evaluate(less), Throws.Exception.Message.EqualTo("Positional arguments must appear before all named arguments. \".mixin(@c: 100, 3px)\""));
        }

        [Test, Ignore]
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

            Assert.That(() => Evaluate(less), Throws.Exception.Message.EqualTo("Argument \"@var\" not found. \".mixin(@var: 6)\""));
        }


        [Test, Ignore]
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
    }
}