namespace dotless.Test.Unit.Engine
{
    using NUnit.Framework;
	public class CombinedSelectorBug :SpecFixtureBase
	{

        [Test]
        public void Nested_combined_selector()
        {

        	var input = @"
#parentRuleSet {
	.selector1.selector2 { position: fixed; }
}";

        	var expected = @"#parentRuleSet .selector1.selector2 {
  position: fixed;
}";

			AssertLess(input, expected);
        }
	}
}
