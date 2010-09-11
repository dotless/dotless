namespace dotless.Test.Unit.Engine
{
    using NUnit.Framework;
    using dotless.Core.Utils;
    public class PathAggregatorBug
    {
        
        [Test]
        public void StringExtension_AggregatePath_CantEscapeToParentDirectory()
        {
            var foo = new [] { @"..\site.less" };
            var aggregatePaths = foo.AggregatePaths();

            Assert.AreEqual(@"..\site.less", aggregatePaths);
        }
    }
}