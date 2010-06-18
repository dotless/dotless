namespace dotless.Test.Specs
{
    using NUnit.Framework;

    public class VariablesFixture : SpecFixtureBase
    {
        [Test]
        public void Variables()
        {
            // Todo: split into separate atomic tests.
            var input =
                @"
@a: 2;
@x: @a * @a;
@y: @x + 1;
@z: @x * 2 + @y;

.variables {
  width: @z + 1cm; // 14cm
}

@b: @a * 10;
@c: #888;

@fonts: ""Trebuchet MS"", Verdana, sans-serif;
@f: @fonts;

@quotes: ""~"" ""~"";
@q: @quotes;

.variables {
  height: @b + @x + 0px; // 24px
  color: @c;
  font-family: @f;
  quotes: @q;
}
";

            var expected =
                @"
.variables {
  width: 14cm;
}
.variables {
  height: 24px;
  color: #888888;
  font-family: ""Trebuchet MS"", Verdana, sans-serif;
  quotes: ""~"" ""~"";
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void LazyEval()
        {
            var input = @"
@var: @a;
@a: 100%;

.lazy-eval {
  width: @var;
}
";

            var expected = @"
.lazy-eval {
  width: 100%;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void Redefine()
        {
            var input = @"
#redefine {
  @var: 4;
  @var: 2;
  @var: 3;
  width: @var;
}
";

            var expected = @"
#redefine {
  width: 3;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void NotFound()
        {
            AssertExpressionError("variable @var is undefined", 0, "@var");
        }
    }
}