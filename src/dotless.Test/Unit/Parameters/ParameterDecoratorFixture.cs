namespace dotless.Test.Unit.Parameters
{
    using System;
    using System.Collections.Generic;
    using Core;
    using Core.Parameters;
    using Moq;
    using NUnit.Framework;
    public class ParameterDecoratorFixture
    {
        [Test]
        public void PrependsParametersAsVariableDeclarationToInput()
        {
            ParameterDecorator parameterDecorator;
            Mock<ILessEngine> engine = SetupDecoratorForTest(out parameterDecorator, new Dictionary<string, string> {{"a", "15px"}});
            parameterDecorator.TransformToCss("width: @a;", "myfile");
            engine.Verify(p => p.TransformToCss(It.Is<string>(a => a.StartsWith("@a: 15px")), "myfile"));
        }

        private Mock<ILessEngine> SetupDecoratorForTest(out ParameterDecorator parameterDecorator, Dictionary<string, string> parameters)
        {
            var engine = new Mock<ILessEngine>();
            var parameterSource = new Mock<IParameterSource>();
            parameterSource.Setup(p => p.GetParameters()).Returns(parameters);

            parameterDecorator = new ParameterDecorator(engine.Object, parameterSource.Object);
            return engine;
        }

        [Test]
        public void PutsOneParameterPerLine()
        {
            ParameterDecorator parameterDecorator;
            var parameters = new Dictionary<string, string> {{"a", "15px"}, {"b", "12px"}};
            Mock<ILessEngine> engine = SetupDecoratorForTest(out parameterDecorator, parameters);

            parameterDecorator.TransformToCss("width: @a;", "myfile");
            engine.Verify(p => p.TransformToCss(It.Is<string>(a => a.Split('\n').Length == parameters.Count + 1), "myfile"));
        }
    }
}