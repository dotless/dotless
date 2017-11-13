namespace dotless.Core.Test.Specs.Functions
{
    using NUnit.Framework;

    public class MixFixture : SpecFixtureBase
    {
        [Test]
        public void TestMix()
        {
            AssertExpression("#800080", "mix(#f00, #00f)");
            AssertExpression("#808080", "mix(#f00, #0ff)");
            AssertExpression("#809155", "mix(#f70, #0aa)");
            AssertExpression("#4000bf", "mix(#f00, #00f, 25%)");
            AssertExpression("rgba(64, 0, 191, 0.75)", "mix(rgba(255, 0, 0, .5), #00f)");
            AssertExpression("#ff0000", "mix(#f00, #00f, 100%)");
            AssertExpression("#0000ff", "mix(#f00, #00f, 0%)");
            AssertExpression("rgba(255, 0, 0, 0.5)", "mix(#f00, rgba(#00f, 0))");
            AssertExpression("rgba(0, 0, 255, 0.5)", "mix(rgba(#f00, 0), #00f)");
            AssertExpression("#ff0000", "mix(#f00, rgba(#00f, 0), 100%)");
            AssertExpression("#0000ff", "mix(rgba(#f00, 0), #00f, 0%)");
            AssertExpression("rgba(0, 0, 255, 0)", "mix(#f00, rgba(#00f, 0), 0%)");
            AssertExpression("rgba(255, 0, 0, 0)", "mix(rgba(#f00, 0), #00f, 100%)");
        }

        [Test]
        public void TestMixTestsTypes()
        {
            AssertExpressionError("Expected color in function 'mix', found \"foo\"", 0, "mix(\"foo\", #f00, 10%)");
            AssertExpressionError("Expected color in function 'mix', found \"foo\"", 0, "mix(#f00, \"foo\", 10%)");
            AssertExpressionError("Expected number in function 'mix', found \"foo\"", 0, "mix(#f00, #baf, \"foo\")");
        }

        [Test]
        public void TestMixTestsBounds()
        {
            AssertExpression("#445566", "mix(#123, #456, -0.001)");
            AssertExpression("#112233", "mix(#123, #456, 100.001)");
        }

        [Test]
        public void TestTintAndShade()
        {
            AssertExpression("#898989", "tint(#777777, 13)");
            AssertExpression("#ffffff", "tint(#777777, 100)");
            AssertExpression("#898989", "tint(#777777, 13%)");
            AssertExpression("#686868", "shade(#777777, 13)");
            AssertExpression("#000000", "shade(#777777, 100)");
            AssertExpression("#686868", "shade(#777777, 13%)");
        }
    }
}