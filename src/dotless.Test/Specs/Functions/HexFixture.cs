namespace dotless.Test.Specs.Functions
{
	using NUnit.Framework;

	public class HexFixture : SpecFixtureBase
	{
		[Test]
		public void TestHex()
		{
			AssertExpression("00", "hex(0)");
			AssertExpression("99", "hex(153)");
			AssertExpression("F0", "hex(240)");
			AssertExpression("FF", "hex(255)");

			AssertExpression("00", "hex(-1)");
			AssertExpression("FF", "hex(999)");

			AssertExpressionError("Expected unitless number in function 'hex', found 5px", 4, "hex(5px)");
		}
	}
}