namespace dotless.Test.Specs.Functions
{
    using NUnit.Framework;

    public class BlendModesFixture : SpecFixtureBase
    {
        [Test]
        public void TestFunctions()
        {
            AssertExpression("#ed0000", "multiply(#f60000, #f60000)");
            AssertExpression("#f600f6", "screen(#f60000, #0000f6)");
            AssertExpression("#ed0000", "overlay(#f60000, #0000f6)");
            AssertExpression("#ff0000", "softlight(#f60000, #ffffff)");
            AssertExpression("#0000ed", "hardlight(#f60000, #0000f6)");
            AssertExpression("#f600f6", "difference(#f60000, #0000f6)");
            AssertExpression("#f600f6", "exclusion(#f60000, #0000f6)");
            AssertExpression("#7b007b", "average(#f60000, #0000f6)");
            AssertExpression("#d73131", "negation(#f60000, #313131)");

            AssertExpression("#efefef", "multiply(white, rgba(96, 96, 96, .1))");
        }
    }
}