namespace dotless.Core.Test.Specs.Functions
{
    using NUnit.Framework;

    public class HexFixture : SpecFixtureBase
    {
        [Test]
        public void TestHexInfo()
        {
            var info1 = "hex(number) is not supported by less.js, so this will work but not compile with other less implementations.";

            AssertExpressionLogMessage(info1, "hex(0)");
        }

        [Test]
        public void TestHex()
        {
            AssertExpression("00", "hex(0)");
            AssertExpression("99", "hex(153)");
            AssertExpression("F0", "hex(240)");
            AssertExpression("FF", "hex(255)");
        }

        [Test]
        public void ValuesBelow_0_AreInterpretedAs_0()
        {
            AssertExpression("00", "hex(-1)");
        }

        [Test]
        public void ValuesAbove_255_AreInterpretedAs_FF()
        {
            AssertExpression("FF", "hex(999)");
        }

        [Test]
        public void ThrowsExpressionError_WhenNumberPassedHasUnit()
        {
            AssertExpressionError("Expected unitless number in function 'hex', found 5px", 4, "hex(5px)");
        }
    }
}