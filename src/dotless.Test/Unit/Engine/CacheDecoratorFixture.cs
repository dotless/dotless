namespace dotless.Test.Unit.Engine
{
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Core.Cache;
    using Moq;
    using NUnit.Framework;

    public class CacheDecoratorFixture
    {
        protected Mock<ILessEngine> Engine { get; set; }
        protected Mock<ICache> Cache { get; set; }
        protected Dictionary<string, string> Parameters { get; set; }
        protected CacheDecorator CacheDecorator { get; set; }
        protected List<string> Imports { get; set; }
        protected string FileName { get; set; }
        protected string Less { get; set; }
        protected string Css { get; set; }
        protected string CacheKey { get; set; }
        protected bool Cached { get; set; }

        [SetUp]
        public void SetupDecoratorForTest()
        {
            Engine = new Mock<ILessEngine>();
            Cache = new Mock<ICache>();

            Imports = new List<string>();
            Less = "the less";
            Css = "the css";
            FileName = "file.less";
            CacheKey = FileName + "CEaZzayH8iHMO6dQWSalPiDS7TY=";
            Cached = false;

            Engine.Setup(e => e.GetImports()).Returns(Imports);
            Engine.Setup(e => e.TransformToCss(Less, FileName)).Returns(() => Css);

            Cache.Setup(c => c.Exists(CacheKey)).Returns(() => Cached);
            Cache.Setup(c => c.Retrieve(CacheKey))
                .Returns(() => Cached ? Css : "")
                .Callback(() => { if (!Cached) Assert.Fail("not cached"); });

            CacheDecorator = new CacheDecorator(Engine.Object, Cache.Object);
        }

        [Test]
        public void InsertsIfNotAlreadyCached()
        {
            CacheDecorator.TransformToCss(Less, FileName);

            Cache.Verify(c=> c.Insert(CacheKey, It.IsAny<IEnumerable<string>>(), Css));
        }

        [Test]
        public void DependanciesContainsFileName()
        {
            CacheDecorator.TransformToCss(Less, FileName);

            Cache.Verify(c => c.Insert(CacheKey, It.Is<IEnumerable<string>>(i => i.Contains(FileName)), Css));
        }

        [Test]
        public void OneDependancyIfNoImports()
        {
            CacheDecorator.TransformToCss(Less, FileName);

            Cache.Verify(c => c.Insert(CacheKey, It.Is<IEnumerable<string>>(i => i.Count() == 1), Css));
        }

        [Test]
        public void IncludesDependancyForAllImports()
        {
            Imports.Add("myfile.less");
            Imports.Add("myotherfile.less");

            CacheDecorator.TransformToCss(Less, FileName);

            Cache.Verify(c => c.Insert(CacheKey, It.Is<IEnumerable<string>>(i => Imports.All(s => i.Contains(s))), Css));
        }

        [Test]
        public void RetrievesIfAlreadyCached()
        {
            Cached = true;

            CacheDecorator.TransformToCss(Less, FileName);

            Cache.Verify(c=> c.Retrieve(CacheKey));
        }

    }
}