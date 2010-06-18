namespace dotless.Test.Specs.Functions
{
    using NUnit.Framework;

    public class ComplementFixture : SpecFixtureBase
    {
        [Test]
        public void TestComplement()
        {
            AssertExpression("#ccbbaa", "complement(#abc)");
            AssertExpression("aqua", "complement(#f00)");
            AssertExpression("red", "complement(#0ff)");
            AssertExpression("white", "complement(#fff)");
            AssertExpression("black", "complement(#000)");
        }

        [Test]
        public void TestComplementTestsTypes()
        {
            AssertExpressionError("Expected color in function 'complement', found \"foo\"", 11, "complement(\"foo\")");
        }
    }
}