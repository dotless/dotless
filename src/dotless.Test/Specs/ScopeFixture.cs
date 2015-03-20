namespace dotless.Test.Specs
{
    using NUnit.Framework;

    public class ScopeFixture : SpecFixtureBase
    {
        [Test]
        public void TestVariablesFromDifferentScopes()
        {
            var input =
                @"
@z: transparent;

.scope1 {
  @y: orange;
  @z: black;
  .scope2 {
    @y: red;
    .scope3 {
      @local: white;
      color: @y; // red
      border-color: @z; // black
      background-color: @local; // white
    }
  }
}
";

            var expected =
                @"
.scope1 .scope2 .scope3 {
  color: red;
  border-color: black;
  background-color: white;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void VariableAssignmentDoesntEscapeScope()
        {
            var input =
                @"
@x: blue;

.scope1 {
  .hidden {
    @x: #131313;
  }
  .scope2 {
    color: @x; // blue
  }
}
";

            var expected =
                @"
.scope1 .scope2 {
  color: blue;
}
";

            AssertLess(input, expected);
        }


        [Test]
        public void VariableInClosestScopeUsed()
        {
            var input =
                @"
@x: blue;
@z: transparent;

.scope1 {
  @y: orange;
  @z: black;
  color: @x; // blue
  border-color: @z; // black
}
";

            var expected =
                @"
.scope1 {
  color: blue;
  border-color: black;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void MixinVariableKeepsScope()
        {
            var input =
                @"
@mix: none;

.mixin {
  @mix: #989;
}

.tiny-scope {
  color: @mix; // none
  .mixin;
  color: @mix; // #998899
}
";

            var expected =
                @"
.tiny-scope {
  color: #998899;
}
";

            AssertLess(input, expected);
        }
    }
}