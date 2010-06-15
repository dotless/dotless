namespace dotless.Test
{
    using System;
    using Core.Parser;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Parser.Infrastructure;
    using NUnit.Framework;

    public class SpecFixtureBase
    {
        protected Func<Parser> DefaultParser = () => new Parser(0);
        protected Func<Env> DefaultEnv = () => new Env();

        protected void AssertLess(string input, string expected)
        {
            AssertLess(input, expected, DefaultParser());
        }

        protected void AssertLess(string input, string expected, Parser parser)
        {
            var output = Evaluate(input, parser).Trim();

            expected = expected.Trim().Replace("\r\n", "\n");

            Assert.That(output, Is.EqualTo(expected));
        }

        protected void AssertLessUnchanged(string input)
        {
            AssertLess(input, input);
        }

        protected void AssertLessUnchanged(string input, Parser parser)
        {
            AssertLess(input, input, parser);
        }

        protected void AssertExpression(string output, string expression)
        {
            AssertExpression(output, expression, null);
        }

        protected void AssertExpression(string output, string expression, IDictionary<string, string> variables)
        {
            Assert.That(EvaluateExpression(expression, variables), Is.EqualTo(output));
        }

        protected void AssertExpressionUnchanged(string expression)
        {
            AssertExpression(expression, expression);
        }

        protected void AssertError(string message, string input)
        {
            Assert.That(() => Evaluate(input), Throws.Exception.Message.EqualTo(message));
        }

        protected void AssertExpressionError(string message, string expression)
        {
            Assert.That(() => EvaluateExpression(expression), Throws.Exception.Message.EqualTo(message));
        }

        protected string EvaluateExpression(string expression)
        {
            return EvaluateExpression(expression, null);
        }

        protected string EvaluateExpression(string expression, IDictionary<string, string> variablesDictionary)
        {
            var variables = "";
            if (variablesDictionary != null)
                variables = variablesDictionary.Aggregate("",
                                                          (s, x) =>
                                                          string.Format("{0}\n  @{1}: {2};", s, x.Key, x.Value));

            var less = string.Format(".def {{{0}\n  expression: {1};\n}}", variables, expression);

            var css = Evaluate(less);

            if (string.IsNullOrEmpty(css))
                return "";

            var start = css.IndexOf("  expression: ");
            var end = css.LastIndexOf("}");

            return css.Substring(start + 14, end - start - 16);
        }

        public string Evaluate(string input)
        {
            return Evaluate(input, DefaultParser());
        }

        public string Evaluate(string input, Parser parser)
        {
            var tree = parser.Parse(input, null);
            return tree.ToCSS(DefaultEnv());
        }
    }
}