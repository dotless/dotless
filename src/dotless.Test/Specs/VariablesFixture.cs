namespace dotless.Test.Specs
{
    using System.Collections.Generic;
    using NUnit.Framework;

    public class VariablesFixture : SpecFixtureBase
    {
        [Test]
        public void VariableOperators()
        {
            var input = @"
@a: 2;
@x: @a * @a;
@y: @x + 1;
@z: @x * 2 + @y;

.variables {
  width: @z + 1cm; // 14cm
}";
            var expected = @"
.variables {
  width: 14cm;
}";
            AssertLess(input, expected);
        }

        [Test]
        public void StringVariables()
        {
            var input =
                @"
@fonts: ""Trebuchet MS"", Verdana, sans-serif;
@f: @fonts;

.variables {
  font-family: @f;
}
";

            var expected =
                @"
.variables {
  font-family: ""Trebuchet MS"", Verdana, sans-serif;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void VariablesWithNumbers()
        {
            var input =
                @"@widget-container: widget-container-8675309;

#@{widget-container} {
    color: blue;
}";

            var expected =
                @"
#widget-container-8675309 {
  color: blue;
}
";

            AssertLess(input, expected);

        }

        [Test]
        public void VariablesChangingUnit()
        {
            var input =
                @"
@a: 2;
@x: @a * @a;
@b: @a * 10;

.variables {
  height: @b + @x + 0px; // 24px
}
";

            var expected =
                @"
.variables {
  height: 24px;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void VariablesColor()
        {
            var input =
                @"
@c: #888;

.variables {
  color: @c;
}
";

            var expected =
                @"
.variables {
  color: #888888;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void VariablesQuoted()
        {
            var input =
                @"
@quotes: ""~"" ""~"";
@q: @quotes;

.variables {
  quotes: @q;
}
";

            var expected =
                @"
.variables {
  quotes: ""~"" ""~"";
}
";

            AssertLess(input, expected);
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
  width: 20px;
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
  width: 20px;
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
  width: 20px;
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

        [Test]
        public void VariableValuesMulti()
        {
            var input = @"
.values {
    @a: 'Trebuchet';
    font-family: @a, @a, @a;
}";
            var expected = @"
.values {
  font-family: 'Trebuchet', 'Trebuchet', 'Trebuchet';
}";
            AssertLess(input, expected);
        }

        [Test]
        public void VariableValuesUrl()
        {
            var input = @"
.values {
    @a: 'Trebuchet';
    url: url(@a);
}";
            var expected = @"
.values {
  url: url('Trebuchet');
}";
            AssertLess(input, expected);
        }

        [Test]
        public void VariableValuesImportant()
        {
            var input = @"
@c: #888;
.values {
    color: @c !important;
}";
            var expected = @"
.values {
  color: #888888 !important;
}";
            AssertLess(input, expected);
        }

        [Test]
        public void VariableValuesMultipleValues()
        {
            var input = @"
.values {
    @a: 'Trebuchet';
    @multi: 'A', B, C;
    multi: something @multi, @a;
}";
            var expected = @"
.values {
  multi: something 'A', B, C, 'Trebuchet';
}";
            AssertLess(input, expected);
        }

        [Test]
        public void VariablesNames()
        {
            var input = @".variable-names {
    @var: 'hello';
    @name: 'var';
    name: @@name;
}";
            var expected = @"
.variable-names {
  name: 'hello';
}";
            AssertLess(input, expected);
        }

        [Test]
        public void VariableSelector()
        {
            string input = @"// Variables
@mySelector: banner;

// Usage
.@{mySelector} {
  font-weight: bold;
  line-height: 40px;
  margin: 0 auto;
}
";

            string expected = @".banner {
  font-weight: bold;
  line-height: 40px;
  margin: 0 auto;
}
";
            AssertLess(input,expected);
        }

        [Test]
        public void SimpleRecursiveVariableDefinition()
        {
            string input = @"
@var: 1px;
@var: @var + 1;

.rule {
    left: @var;
}
";

            string expectedError = @"
Recursive variable definition for @var on line 2 in file 'test.less':
  [1]: @var: 1px;
  [2]: @var: @var + 1;
       ------^
  [3]: ";

            AssertError(expectedError, input);
        }

        [Test]
        public void IndirectRecursiveVariableDefinition()
        {
            string input = @"
@var: 1px;
@var2: @var;
@var: @var2 + 1;

.rule {
    left: @var;
}
";

            string expectedError = @"
Recursive variable definition for @var on line 2 in file 'test.less':
  [1]: @var: 1px;
  [2]: @var2: @var;
       -------^
  [3]: @var: @var2 + 1;";

            AssertError(expectedError, input);
        }
		
        [Test]
        public void VariableDeclarationWithMissingSemicolon() {
            var input = @"
@v1:Normal;
@v2:
";

            AssertError("missing semicolon in expression", "@v2:", 2, 3, input);
        }
		

        [Test]
        public void VariablesInAttributeSelectorValue() {
            var input = @"
@breakpoint-alias: ""desktop"";
[class*=""@{breakpoint-alias}-rule""] {
    margin-top: 0;
    zoom: 1; 
}";

            var expected = @"
[class*=""desktop-rule""] {
  margin-top: 0;
  zoom: 1;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void VariablesAsAttributeName() {
            var input = @"
@key: ""desktop"";
[@{key}=""value""] {
    margin-top: 0;
    zoom: 1; 
}";

            var expected = @"
[""desktop""=""value""] {
  margin-top: 0;
  zoom: 1;
}";

            AssertLess(input, expected);
        }

        [Test]
        public void VariablesAsPartOfAttributeNameNotAllowed() {
            var input = @"
@key: ""desktop"";
[@{key}-something=""value""] {
    margin-top: 0;
    zoom: 1; 
}";

            var expected = @"
[""desktop""=""value""] {
  margin-top: 0;
  zoom: 1;
}";

            AssertError("Expected ']' but found '\"'", "[@{key}-something=\"value\"] {", 2, 17, input);
        }		
    }
}