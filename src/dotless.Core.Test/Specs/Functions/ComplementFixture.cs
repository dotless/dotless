namespace dotless.Core.Test.Specs.Functions
{
    using NUnit.Framework;

    public class ComplementFixture : SpecFixtureBase
    {
        [Test]
        public void TestComplement()
        {
            AssertExpression("#ccbbaa", "complement(#abc)");
            AssertExpression("#00ffff", "complement(#f00)");
            AssertExpression("#ff0000", "complement(#0ff)");
            AssertExpression("#ffffff", "complement(#fff)");
            AssertExpression("#000000", "complement(#000)");
        }

        [Test]
        public void TestComplementInfo()
        {
            var complementInfo = "complement(color) is not supported by less.js, so this will work but not compile with other less implementations.";

            AssertExpressionLogMessage(complementInfo, "complement(#000)");
        }

        [Test]
        public void TestComplementTestsTypes()
        {
            AssertExpressionError("Expected color in function 'complement', found \"foo\"", 11, "complement(\"foo\")");
        }
    }
}