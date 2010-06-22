namespace dotless.Test.Unit.Engine
{
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Core.Parameters;
    using Moq;
    using NUnit.Framework;

    public class ParameterDecoratorFixture
    {
        protected Mock<ILessEngine> Engine { get; set; }
        protected Mock<IParameterSource> ParameterSource { get; set; }
        protected Dictionary<string, string> Parameters { get; set; }
        protected ParameterDecorator ParameterDecorator { get; set; }

        [SetUp]
        public void SetupDecoratorForTest()
        {
            Engine = new Mock<ILessEngine>();
            ParameterSource = new Mock<IParameterSource>();
            Parameters = new Dictionary<string, string>();

            ParameterSource.Setup(p => p.GetParameters()).Returns(Parameters);

            ParameterDecorator = new ParameterDecorator(Engine.Object, ParameterSource.Object);
        }

        [Test]
        public void PrependsParametersAsVariableDeclarationToInput()
        {
            Parameters["a"] = "15px";

            ParameterDecorator.TransformToCss("width: @a;", "myfile");

            Engine.Verify(p => p.TransformToCss("@a: 15px;\nwidth: @a;", "myfile"));
        }

        [Test]
        public void PutsOneParameterPerLine()
        {
            Parameters["a"] = "15px";
            Parameters["b"] = "12px";

            ParameterDecorator.TransformToCss("width: @a;", "myfile");

            Engine.Verify(p => p.TransformToCss(It.Is<string>(s => s.Count(c => c == '\n') == 2), "myfile"));
        }
    }
}