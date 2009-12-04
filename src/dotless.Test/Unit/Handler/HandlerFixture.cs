namespace dotless.Test.Unit.Handler
{
    using Core;
    using Core.Abstractions;
    using NUnit.Framework;
    using Rhino.Mocks;

    [TestFixture]
    public class HandlerFixture
    {
        private IPathProvider provider;
        private IRequest request;
        private IResponse response;
        private ILessEngine engine;

        [SetUp]
        public void SetUp()
        {
            provider = MockRepository.GenerateStub<IPathProvider>();
            request = MockRepository.GenerateStub<IRequest>();
            response = MockRepository.GenerateStub<IResponse>();
            engine = MockRepository.GenerateStub<ILessEngine>();
        }

        [Test]
        public void RetrievesPhysicalPathFromPathProvider()
        {
            var mock = MockRepository.GenerateMock<IPathProvider>();
            string path = "abc";
            request.Stub(p => p.LocalPath).Return(path);
            var impl = 
                new HandlerImpl(mock, request, response, engine);

            impl.Execute();

            mock.AssertWasCalled(p => p.MapPath(path));
        }

        [Test]
        public void RetrievesPathFromRequest()
        {
            var mock = MockRepository.GenerateMock<IRequest>();
            string path = "abc";

            var impl = new HandlerImpl(provider, mock, response, engine);

            impl.Execute();

            mock.AssertWasCalled(p => p.LocalPath);
        }

        [Test]
        public void CallsEngineWithFilePath()
        {
            var mock = MockRepository.GenerateMock<ILessEngine>();
            string lessFile = "myLessfile.less";
            provider.Stub(p => p.MapPath(null)).IgnoreArguments().Return(lessFile);
            var impl = new HandlerImpl(provider, request, response, mock);

            impl.Execute();

            mock.AssertWasCalled(p => p.TransformToCss(lessFile));
        }

        [Test]
        public void WritesEngineOutputToResponse()
        {
            var mock = MockRepository.GenerateStub<IResponse>();
            string output = "myCss";
            engine.Stub(p => p.TransformToCss(null)).IgnoreArguments().Return(output);
            var impl = new HandlerImpl(provider, request, mock, engine);

            impl.Execute();

            mock.AssertWasCalled(p => p.WriteCss(output));
        }
    }
}