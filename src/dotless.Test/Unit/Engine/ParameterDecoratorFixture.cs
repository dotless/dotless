using dotless.Core.Parser;

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

        private bool lastTransformationSuccessful;
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

            Engine.Verify(p => p.TransformToCss("@a: 15px;\r\nwidth: @a;", "myfile"));
        }

        [Test]
        public void PutsOneParameterPerLine()
        {
            Parameters["a"] = "15px";
            Parameters["b"] = "12px";

            ParameterDecorator.TransformToCss("width: @a;", "myfile");

            Engine.Verify(p => p.TransformToCss(It.Is<string>(s => s.Count(c => c == '\n') == 2), "myfile"));
        }

        [Test]
        public void IgnoresParamtersWithNullValue()
        {
            Parameters["a"] = null;
            ParameterDecorator.TransformToCss("", "myfile");

            Engine.Verify(p => p.TransformToCss(It.Is<string>(s => s == ""), "myfile"));
        }

        [Test]
        public void IgnoresParameterWithEmptyValue()
        {
            Parameters["a"] = "";
            ParameterDecorator.TransformToCss("", "myfile");

            Engine.Verify(p => p.TransformToCss(It.Is<string>(s => s == ""), "myfile"));
        }

        [Test]
        public void IgnoresParameterWithInvalidValue()
        {
            Parameters["a"] = "1-x";
            Parameters["b"] = "1px";

            ParameterDecorator.TransformToCss("", "myfile");

            var expectedResult = @"/* Omitting variable 'a'. The expression '1-x' is not valid. */
@b: 1px;
";

            Engine.Verify(p => p.TransformToCss(It.Is<string>(s => s == expectedResult), "myfile"));
        }
    }
}