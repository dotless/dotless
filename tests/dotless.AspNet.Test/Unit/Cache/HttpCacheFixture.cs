namespace dotless.Test.Unit.Cache
{
    using System.Web;
    using Core.Cache;
    using Core.Input;
    using Moq;
    using NUnit.Framework;

    public class HttpCacheFixture : HttpFixtureBase
    {
        protected HttpCache TheCache { get; set; }

        [SetUp]
        public void Setup()
        {
            var pathResolver = new Mock<IPathResolver>();

            pathResolver.Setup(p => p.GetFullPath(It.IsAny<string>())).Returns<string>(s => s);

            TheCache = new HttpCache(Http.Object, pathResolver.Object);
        }

        [Test]
        public void CacheDoesntExistIfNotInserted()
        {
            Assert.That(TheCache.Exists("cache-key"), Is.False); // Null Reference Exception ... How on earth do you test Cache??
        }
    }
}