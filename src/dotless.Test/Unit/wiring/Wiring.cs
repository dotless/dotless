namespace dotless.Test.Unit.wiring
{
    using Core;
    using Core.configuration;
    using NUnit.Framework;
    using Pandora;

    [TestFixture]
    public class Wiring
    {
        [Test]
        public void HandlerUsesByDefaultAspServerPathProvider()
        {
            var factory = new ContainerFactory();
            var container = (CommonServiceLocatorAdapter) factory.GetContainer();
            var instance = container.GetInstance(typeof(ILessSource));
            Assert.IsInstanceOf<AspServerPathSource>(instance);
        }

        [Test]
        public void CoreConfigExposesFileSource()
        {
            var factory = new ContainerFactory();
            var container = (CommonServiceLocatorAdapter)factory.GetCoreContainer(DotlessConfiguration.Default);
            var instance = container.GetInstance(typeof (ILessSource));
            Assert.IsInstanceOf<FileSource>(instance);
        }
    }
}
