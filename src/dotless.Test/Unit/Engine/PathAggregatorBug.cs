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
            var aggregatePaths = foo.AggregatePaths(@"C:\dir\");

            // Even most of Windows accepts the forward slash as a path separator
            // so it should be OK to let it get changed
            Assert.AreEqual(@"../site.less", aggregatePaths);
        }

        [Test]
        public void StringExtension_AggregatePath_CantGoUpMultipleLevels()
        {
            var foo = new [] { @"../../site.less" };
            var aggregatePaths = foo.AggregatePaths(@"C:\dir\");

            Assert.AreEqual(@"../../site.less", aggregatePaths);
        }

        [Test]
        public void StringExtension_AggregatePath_ReplacesAllBackwardSlashesWithForwardSlashes()
        {
            var foo = new[] { @"../..\../something.less" };
            var aggregatePaths = foo.AggregatePaths(@"C:\dir\");

            Assert.AreEqual(@"../../../something.less", aggregatePaths);
        }

        [Test]
        public void StringExtension_AggregatePath_WillFixParentDeclarationIfPossible()
        {
            var foo = new[] {@"/site/../something.less"};
            var aggregatePaths = foo.AggregatePaths(@"C:\dir\");

            Assert.AreEqual("/something.less", aggregatePaths);
        }
    }
}