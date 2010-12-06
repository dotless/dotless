using System.Collections.Generic;
using dotless.Core;
using dotless.Core.Parameters;
using Moq;
using NUnit.Framework;

namespace dotless.Test.Unit.Engine
{
	public class NullVariableValueBug
	{

		[Test]
		public void NullVariableValuesProduceSensibleErrorMessage()
		{
			var parameterSourceMock = new Mock<IParameterSource>();

			parameterSourceMock.Setup(s => s.GetParameters()).Returns(new Dictionary<string, string> {{"color", null}});

			var decorator = new ParameterDecorator(new LessEngine(), parameterSourceMock.Object);

			Assert.DoesNotThrow(() => decorator.TransformToCss("", "test.less"));
		}
	}
}
