namespace dotless.Core.Test.Unit.Engine
{
    using Core;
    using dotless.Core.configuration;
    using dotless.Core.Input;
    using dotless.Core.Loggers;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using NUnit.Framework;

    public class ResetImportsBug
    {
        private const string BaseFileName = "../base";
        private const string BaseFile = @"
.mixin {
    color: red;
}
";
        private const string File1Name = "file1";
        private const string File1 = @"
@import (reference) ""../base"";

.someClass1 {
    .mixin;
}
";
        private const string File2Name = "file2";
        private const string File2 = @"
@import (reference) ""../base"";

.someClass2 {
    .mixin;
}
";

        protected ILessEngine Engine { get; set; }
        protected Mock<IFileReader> FileReader { get; set; }
        protected Mock<ILogger> Logger { get; set; }

        [SetUp]
        public void SetupDecoratorForTest()
        {
            FileReader = new Mock<IFileReader>(MockBehavior.Strict);
            Logger = new Mock<ILogger>(MockBehavior.Strict);

            FileReader.Setup(e => e.GetFileContents(BaseFileName)).Returns(BaseFile);
            FileReader.Setup(e => e.GetFileContents(File1Name)).Returns(File1);
            FileReader.Setup(e => e.GetFileContents(File2Name)).Returns(File2);
            FileReader.Setup(e => e.DoesFileExist(It.IsIn(new[] { BaseFileName, File1Name, File2Name }))).Returns(true);

            Logger.Setup(x => x.Error(It.IsAny<string>()));

            Engine = new EngineFactory().GetEngine(new CustomContainerFactory(FileReader.Object, Logger.Object));
        }

        [Test]
        public void ResetImports_ReuseSameEngineToTransformDifferentFiles_ShouldBeAbleToImportSameFileByReference()
        {
            Engine.TransformToCss(File1, File1Name);
            Logger.Verify(c => c.Error(It.IsAny<string>()), Times.Never);

            Engine.ResetImports();
            Engine.TransformToCss(File2, File2Name);
            Logger.Verify(c => c.Error(It.IsAny<string>()), Times.Never);
        }

        private class CustomContainerFactory : ContainerFactory
        {
            private readonly IFileReader _fileReader;
            private readonly ILogger _logger;

            public CustomContainerFactory(IFileReader fileReader, ILogger logger)
            {
                _fileReader = fileReader;
                _logger = logger;
            }

            protected override void OverrideServices(IServiceCollection services, DotlessConfiguration configuration)
            {
                services.AddSingleton(_fileReader);
                services.AddSingleton(_logger);

                base.OverrideServices(services, configuration);
            }
        }
    }
}
