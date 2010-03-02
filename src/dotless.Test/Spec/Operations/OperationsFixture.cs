using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace dotless.Test.Spec.Operations
{
    [TestFixture]
    public class OperationsFixture : SpecFixtureBase
    {
        // Note: these tests were modified from http://github.com/nex3/haml/blob/32b95d8996b57b8d2c6c210452339b66182b65b3/test/sass/script_test.rb

        [Test, Ignore]
        public void CanMultiplyPixelsByPercent()
        {
            AssertExpression("10px", "20px * 50%");
            AssertExpression("10px", "20% * 50px");
        }

        [Test]
        public void DividingPixelsByPixelsHasNoUnits()
        {
            AssertExpression("2", "100px / 50px");
        }

        [Test]
        public void ArithmeticOps()
        {
            AssertExpression("2", "1 + 1");
            AssertExpression("0", "1 - 1");
            AssertExpression("8", "2 * 4");
            AssertExpression(".5", "2 / 4");
            AssertExpression("2", "4 / 2");

            AssertExpression("-1", "-1");
        }

        [Test]
        public void TestFunctions()
        {
            AssertExpression("#80ff80", "hsl(120, 100%, 75%)");
            AssertExpression("#81ff81", "hsl(120, 100%, 75%) + #010001");
        }

        [Test, Ignore("Boolean Expressions")]
        public void TestStringOps()
        {
            AssertExpression("foo bar", "'foo' 'bar'");
            AssertExpression("true 1", "true 1");
            AssertExpression("foo, bar", "'foo' , 'bar'");
            AssertExpression("true, 1", "true , 1");
            AssertExpression("foobar", "'foo' + 'bar'");
            AssertExpression("true1", "true + 1");
            AssertExpression("foo-bar", "'foo'' - ''bar'");
            AssertExpression("true-1", "true - 1");
            AssertExpression("foo/bar", "'foo' / 'bar'");
            AssertExpression("true/1", "true / 1");

            AssertExpression("-bar", "- 'bar'");
            AssertExpression("-true", "- true");
            AssertExpression("/bar", "/ 'bar'");
            AssertExpression("/true", "/ true");
        }
        
        [Test]
        public void TestBooleans()
        {
            AssertExpression("true", "true");
            AssertExpression("false", "false");
        }

        [Test, Ignore("Boolean Expressions")]
        public void TestBooleanOps()
        {
            AssertExpression("true", "true and true");
            AssertExpression("true", "false or true");
            AssertExpression("true", "true or false");
            AssertExpression("true", "true or true");
            AssertExpression("false", "false or false");
            AssertExpression("false", "false and true");
            AssertExpression("false", "true and false");
            AssertExpression("false", "false and false");

            AssertExpression("true", "not false");
            AssertExpression("false", "not true");
            AssertExpression("true", "not not true");

            AssertExpression("1", "false or 1");
            AssertExpression("false", "false and 1");
            AssertExpression("2", "2 or 3");
            AssertExpression("3", "2 and 3");
        }

        [Test, Ignore("Boolean Expressions")]
        public void TestInequalities()
        {
            AssertExpression("false", "1 > 2");
            AssertExpression("false", "2 > 2");
            AssertExpression("true", "3 > 2");

            AssertExpression("false", "1 >= 2");
            AssertExpression("true", "2 >= 2");
            AssertExpression("true", "3 >= 2");

            AssertExpression("true", "1 < 2");
            AssertExpression("false", "2 < 2");
            AssertExpression("false", "3 < 2");

            AssertExpression("true", "1 <= 2");
            AssertExpression("true", "2 <= 2");
            AssertExpression("false", "3 <= 2");
        }

        [Test, Ignore("Boolean Expressions")]
        public void TestEquality()
        {
            var variables = new Dictionary<string, string> { { "foo", "bar" } };
            AssertExpression("true", "'bar' == @foo", variables);
            AssertExpression("true", "@foo == 'bar'", variables);
            AssertExpression("true", "1 == 1.0");
            AssertExpression("true", "false != true");
            AssertExpression("false", "1em == 1px");
            AssertExpression("false", "12 != 12");
        }

        [Test, Ignore("Boolean Expressions")]
        public void TestOperationPrecedence()
        {
            AssertExpression("false true", "true and false false or true");
            AssertExpression("true", "false and true or true and true");
            AssertExpression("true", "1 == 2 or 3 == 3");
            AssertExpression("true", "1 < 2 == 3 >= 3");
            AssertExpression("true", "1 + 3 > 4 - 2");
            AssertExpression("11", "1 + 2 * 3 + 4");
        }

        [Test, Ignore("Unit Conversion")]
        public void TestOperatorUnitConversion()
        {
            AssertExpression("1.1cm", "1cm + 1mm");
            AssertExpression("true", "2mm < 1cm");
            AssertExpression("true", "10mm == 1cm");
            AssertExpression("true", "1 == 1cm");
            AssertExpression("true", "1.1cm == 11mm");
        }

        [Test, Ignore]
        public void TestRgbaColorMath() {
            AssertExpression("rgba(50, 50, 100, 0.35)", "rgba(1, 1, 2, 0.35) * rgba(50, 50, 50, 0.35)");
            AssertExpression("rgba(52, 52, 52, 0.25)", "rgba(2, 2, 2, 0.25) + rgba(50, 50, 50, 0.25)");

            AssertErrorMessage("Alpha channels must be equal: rgba(1, 2, 3, 0.15) + rgba(50, 50, 50, 0.75)", "rgba(1, 2, 3, 0.15) + rgba(50, 50, 50, 0.75)");
            
            AssertErrorMessage("Alpha channels must be equal: #123456 * rgba(50, 50, 50, 0.75)", "#123456 * rgba(50, 50, 50, 0.75)");

            AssertErrorMessage("Alpha channels must be equal: #123456 / #123456", "rgba(50, 50, 50, 0.75) / #123456");            
        }

        [Test]
        public void TestRgbaNumberMath()
        {
            AssertExpression("rgba(49, 49, 49, .75)", "rgba(50, 50, 50, 0.75) - 1");
            AssertExpression("rgba(100, 100, 100, .75)", "rgba(50, 50, 50, 0.75) * 2");
        }
    }
}