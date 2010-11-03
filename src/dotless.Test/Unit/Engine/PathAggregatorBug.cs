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

            // Even most of Windows accepts the forward slash as a path separator
            // so it should be OK to let it get changed
            Assert.AreEqual(@"../site.less", aggregatePaths);
        }
    }
}