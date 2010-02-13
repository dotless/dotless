using System.Collections.Generic;
using System.Linq;
using dotless.Core.engine;
using NUnit.Framework;

namespace dotless.Test.Spec
{
    public class SpecFixtureBase
    {
        protected static void AssertExpression(string output, string expression)
        {
            AssertExpression(output, expression, null);
        }

        protected static void AssertExpression(string output, string expression, IDictionary<string, string> variables)
        {
            Assert.That(EvaluateExpression(expression, variables), Is.EqualTo(output));
        }

        protected static void AssertErrorMessage(string message, string expression)
        {
            Assert.That(() => EvaluateExpression(expression), Throws.Exception.Message.EqualTo(message));
        }

        protected static string EvaluateExpression(string expression)
        {
            return EvaluateExpression(expression, null);
        }

        protected static string EvaluateExpression(string expression, IDictionary<string, string> variablesDictionary)
        {
            var variables = "";
            if (variablesDictionary != null)
                variables = variablesDictionary.Aggregate("", (s, x) => string.Format("{0} @{1}: {2};", s, x.Key, x.Value));

            var less = string.Format(".def {{ {0} prop: {1}; }}", variables, expression);

            var css = Evaluate(less);

            if (string.IsNullOrEmpty(css))
                return "";

            var start = css.IndexOf("prop: ");
            var end = css.LastIndexOf("}");

            return css.Substring(start + 6, end - start - 8);
        }

        public static string Evaluate(string less)
        {
            var engine = new ExtensibleEngineImpl(less);

            return engine.Css;
        }
    }
}