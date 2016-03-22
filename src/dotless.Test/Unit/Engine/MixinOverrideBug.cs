using System;

namespace dotless.Test.Unit.Engine
{
    using NUnit.Framework;

    public class MixinOverrideBug : SpecFixtureBase
    {
        [Test]
        public void Mixin_override_stack_overflow()
        {

            var input = @"
.button
{
    background-color: black;
    color: white;
}

.red-skin
{
    .button
    {
        .button;

        background-color: red;
    }
}";

            var expected = @"
.button {
  background-color: black;
  color: white;
}
.red-skin .button {
  background-color: #000000;
  color: #ffffff;
  background-color: red;
}
";

            AssertLess(input, expected);
        }
    }
}
