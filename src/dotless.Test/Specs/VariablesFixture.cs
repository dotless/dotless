namespace dotless.Test.Specs
{
    using System.Collections.Generic;
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
        public void ThrowsWhenTryToEvaluateBeforeDefinition()
        {
            var input = @"
@var: @a;
@a: 100%;

.lazy-eval {
  width: @var;
}
";

            AssertError("variable @a is undefined", "@var: @a;", 1, 6, input);
        }

        [Test]
        public void ThrowsIfVariableDefinedAfterUse()
        {
            var input = @"
.late-bound {
  width: @var;
}

@var: 10px;
";

            AssertError("variable @var is undefined", "  width: @var;", 2, 9, input);
        }

        [Test]
        public void VariableOverridesPreviousValue1()
        {
            var input = @"
@var: 10px;
.init {
  width: @var;
}

@var: 20px;
.overridden {
  width: @var;
}
";
            
            var expected = @"
.init {
  width: 10px;
}
.overridden {
  width: 20px;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void VariableOverridesPreviousValue2()
        {
            var input = @"
@var: 10px;
.test {
  width: @var;

  @var: 20px;
  height: @var;
}
";
            
            var expected = @"
.test {
  width: 10px;
  height: 20px;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void VariableOverridesPreviousValue3()
        {
            var input = @"
@var: 10px;
.test {
  @var: 15px;
  width: @var;

  @var: 20px;
  height: @var;
}
";
            
            var expected = @"
.test {
  width: 15px;
  height: 20px;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void VariableOverridesPreviousValue4()
        {
            var input = @"
@var: 10px;
.test1 {
  @var: 20px;
  width: @var;
}
.test2 {
  width: @var;
}
";
            
            var expected = @"
.test1 {
  width: 20px;
}
.test2 {
  width: 10px;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void VariableOverridesPreviousValue5()
        {
            var input = @"
.mixin(@a) {
  width: @a;
}
.test {
  @var: 15px;
  .mixin(@var);

  @var: 20px;
  .mixin(@var);
}
";
            
            var expected = @"
.test {
  width: 15px;
  width: 20px;
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
        public void ThrowsIfNotFound()
        {
            AssertExpressionError("variable @var is undefined", 0, "@var");
        }

        [Test]
        public void VariablesKeepImportantKeyword()
        {
            var variables = new Dictionary<string, string>();
            variables["a"] = "#335577";
            variables["b"] = "#335577 !important";

            AssertExpression("#335577 !important", "@a !important", variables);
            AssertExpression("#335577 !important", "@b", variables);
        }

        [Test]
        public void VariablesKeepImportantKeyword2()
        {
            var input = @"
@var: 0 -120px !important;

.mixin(@a) {
  background-position: @a;
}

.class1 { .mixin( @var ); }
.class2 { background-position: @var; }
";

            var expected = @"
.class1 {
  background-position: 0 -120px !important;
}
.class2 {
  background-position: 0 -120px !important;
}
";
            AssertLess(input, expected);
        }

    }
}