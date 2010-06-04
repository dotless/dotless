namespace dotless.Test.Specs
{
    using NUnit.Framework;

    public class ScopeFixture : SpecFixtureBase
    {
        [Test]
        public void Scope()
        {
            // Todo: split into separate atomic tests.
            var input =
                @"
@x: blue;
@z: transparent;
@mix: none;

.mixin {
  @mix: #989;
}

.tiny-scope {
  color: @mix; // #989
  .mixin;
}

.scope1 {
  @y: orange;
  @z: black;
  color: @x; // blue
  border-color: @z; // black
  .hidden {
    @x: #131313;
  }
  .scope2 {
    @y: red;
    color: @x; // blue
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
.tiny-scope {
  color: #998899;
}
.scope1 {
  color: blue;
  border-color: black;
}
.scope1 .scope2 {
  color: blue;
}
.scope1 .scope2 .scope3 {
  color: red;
  border-color: black;
  background-color: white;
}
";

            AssertLess(input, expected);
        }
    }
}