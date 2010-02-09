using dotless.Core.engine;
using NUnit.Framework;

namespace dotless.Test.Spec
{
    public class SpecFixtureBase
    {
        protected static void AssertExpression(string output, string expression)
        {
            Assert.That(() => Evaluate(expression), Is.EqualTo(output));
        }

        protected static void AssertErrorMessage(string message, string expression)
        {
            Assert.That(() => Evaluate(expression), Throws.Exception.Message.EqualTo(message));
        }

        protected static string Evaluate(string expression)
        {
            var less = ".def { @c: #123456; prop: " + expression + "; }";

            var engine = new ExtensibleEngineImpl(less);

            var css = engine.Css;

            if (string.IsNullOrEmpty(css))
                Assert.Fail("expression '{0}' returned no output.", expression);

            var start = css.IndexOf("prop: ");
            var end = css.LastIndexOf("}");

            return css.Substring(start + 6, end - start - 8);
        }
    }
}