namespace dotless.Test.Specs
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    public class OperationsFixture : SpecFixtureBase
    {
        [Test]
        public void Operations()
        {
            AssertExpression("#111111", "#110000 + #000011 + #001100");
            AssertExpression("9px", "10px / 2px + 6px - 1px * 2");
            AssertExpression("9px", "10px / 2px+6px-1px*2");
            AssertExpression("3em", "2 * 4 - 5em");
            AssertExpression("3em", "2  * 4-5em");
        }

        [Test]
        public void OperationsAndPrecisions()
        {
            AssertExpression("3.333333333px", "10px / 3"); // TODO - check px allowed precision
            AssertExpression("3.333333333px", "10 / 3px"); // TODO - check px allowed precision
            AssertExpression("1.875em", "15 / 8em");
            AssertExpression("1.875em", "15em / 8");
            AssertExpression("0.333333333%", "1 / 3%");
            AssertExpression("0.333333333%", "1% / 3");
        }

        [Test]
        public void WithVariables()
        {
            var variables = new Dictionary<string, string>();
            variables["x"] = "4";
            variables["y"] = "12em";

            AssertExpression("16em", "@x + @y", variables);
            AssertExpression("24em", "12 + @y", variables);
            AssertExpression("1cm", "5cm - @x", variables);
        }

        [Test]
        public void Negative()
        {
            var variables = new Dictionary<string, string>();
            variables["z"] = "-2";

            AssertExpression("0px", "2px + @z", variables);
            AssertExpression("4px", "2px - @z", variables);
        }

        [Test]
        public void Negations()
        {
            var variables = new Dictionary<string, string>();
            variables["z"] = "4px";

            AssertExpression("-4px", "-@z", variables);
            AssertExpression("0px", "-@z + @z", variables);
            AssertExpression("0px", "@z + -@z", variables);
            AssertExpression("8px", "@z - -@z", variables);
            AssertExpression("0px", "-@z - -@z", variables);

            AssertExpression("-4px", "-(@z)", variables);
            AssertExpression("16px", "-(2 + 2) * -@z", variables);
        }

        [Test]
        public void Shorthands()
        {
            AssertExpression("-1px 2px 0 -4px", "-1px 2px 0 -4px");
        }

        [Test]
        public void Colours()
        {
            AssertExpression("#123456", "#123456");
            AssertExpression("#334455", "#234 + #111111");
            AssertExpression("#000000", "#222222 - #fff");
            AssertExpression("#222222", "2 * #111");
            AssertExpression("#222222", "#333333 / 3 + #111");
        }

        [Test]
        public void OperationsAreLeftAssociative()
        {
            AssertExpression("1", "20 / 5 / 4");
            AssertExpression("0", "20 - 10 - 5 - 5");
        }

        [Test]
        public void CanIncludeFunctionsInOperations()
        {
            AssertExpression("5", "round(10.34) / 2");
            AssertExpression("2", "6 / round(2.8)");
            AssertExpression("50%", "lightness(white) / 2");
        }

        [Test]
        public void CanIncludeColorFunctionsInOperations()
        {
            AssertExpression("#646464", "rgb(200, 200, 200) / 2");
            AssertExpression("#ff8080", "2 * hsl(0, 50%, 50%)");
            AssertExpression("#c94a4a", "rgb(10, 10, 10) + hsl(0, 50%, 50%)");
        }

        [Test]
        public void CanUseColorKeywordsInOperations()
        {
            AssertExpression("#ff1111", "red + 17");
            AssertExpression("#808080", "white / 2");
            AssertExpression("#ff80ff", "red + green + blue");
        }

        [Test]
        public void Rounding()
        {
            var variables = new Dictionary<string, string>();
            variables["base"] = "16em";

            AssertExpression("3.75em", "60/@base", variables);
        }

        [Test]
        public void DivisionByZero()
        {
            var divideByZeroException = new DivideByZeroException();
            AssertExpressionError(divideByZeroException.Message, 5, "20px / 0");
            AssertExpressionError(divideByZeroException.Message, 14, "1 + 2 - 3 * 4 / 0");
            AssertExpressionError(divideByZeroException.Message, 6, "1 + 2 / 0 - 3 * 4 / 0");
        }

        [Test]
        public void DivideNumberByColor()
        {
            AssertExpressionError("Can't substract or divide a color from a number", 4, "100 / #fff");
        }

        [Test]
        public void ThrowsIfLeftHandSideIsNotOperable()
        {
            var variables = new Dictionary<string, string> { { "a", "1px dotted #cccccc" } };

            AssertExpressionError("Cannot apply operator + to the left hand side: 1px dotted #cccccc", 21, "(1px dotted #cccccc) + #252525");
            AssertExpressionError("Cannot apply operator + to the left hand side: 1px dotted #cccccc", 3, "@a + #252525", variables);
        }

        [Test]
        public void ThrowsIfUnableToConvertRightHandSideToColor()
        {
            var variables = new Dictionary<string, string> { { "a", "keyword" } };

            AssertExpressionError("Unable to convert right hand side of + to a color", 8, "#252525 + @a", variables);
        }

        [Test]
        public void ThrowsIfRightHandSideIsNotANumber()
        {
            var variables = new Dictionary<string, string> { { "a", "twenty" } };

            AssertExpressionError("Expected number in right hand side of +, found twenty", 3, "10 + @a", variables);
        }

        [Test]
        public void OperationsKeepImportantKeyword()
        {
            AssertExpressionUnchanged("-120px !important");
            AssertExpression("3 !important", "1 + 2 !important");
            AssertExpression("3 !important", "6 / 2 !important");
        }

        [Test]
        public void UnaryPlus()
        { 
            AssertExpression("1px", "+1px");
        }

        [Test]
        public void DivisionWithIdenticalUnitsChangesUnitPrecedence() {
            var input = @"
.test {
  width: (10px / 5px) + 100;
  height: (5px / 10px) * 100%;
}";

            var expected = @"
.test {
  width: 102px;
  height: 50%;
}";

            AssertLess(input, expected);
        }
    }
}