namespace dotless.Test.Unit.Handler
{
    using Core;
    using Core.Abstractions;
    using NUnit.Framework;
    using Rhino.Mocks;

    [TestFixture]
    public class HandlerFixture
    {
        private IRequest request;
        private IResponse response;
        private ILessEngine engine;
        private ILessSource lessSource;

        [SetUp]
        public void SetUp()
        {
            request = MockRepository.GenerateStub<IRequest>();
            response = MockRepository.GenerateStub<IResponse>();
            engine = MockRepository.GenerateStub<ILessEngine>();
            lessSource = MockRepository.GenerateStub<ILessSource>();
        }

        [Test]
        public void LoadsFileThroughSource()
        {
            var mock = MockRepository.GenerateMock<ILessSource>();
            string path = "abc";
            request.Stub(p => p.LocalPath).Return(path);
            var impl = new HandlerImpl(request, response, engine, mock);

            impl.Execute();

            mock.AssertWasCalled(p => p.GetSource(path));
        }

        [Test]
        public void RetrievesPathFromRequest()
        {
            var mock = MockRepository.GenerateMock<IRequest>();

            var impl = new HandlerImpl(mock, response, engine, lessSource);

            impl.Execute();

            mock.AssertWasCalled(p => p.LocalPath);
        }

        [Test]
        public void WritesEngineOutputToResponse()
        {
            var mock = MockRepository.GenerateStub<IResponse>();
            string output = "myCss";
            engine.Stub(p => p.TransformToCss(null)).IgnoreArguments().Return(output);
            var impl = new HandlerImpl(request, mock, engine, lessSource);

            impl.Execute();

            mock.AssertWasCalled(p => p.WriteCss(output));
        }
    }
}