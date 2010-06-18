namespace dotless.Test
{
    using System;
    using System.Text.RegularExpressions;
    using Core.Importers;
    using Core.Parser;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Parser.Infrastructure;
    using NUnit.Framework;

    public class SpecFixtureBase
    {
        protected Func<Parser> DefaultParser = () => new Parser(0, new TestStylizer(), new Importer());
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

        public void AssertError(string message, string line, int lineNumber, int position, string input)
        {
            message = TestStylizer.GetErrorMessage(message, line, lineNumber, position);
            AssertError(message, input);
        }

        protected void AssertExpressionError(string message, string expression)
        {
            Assert.That(() => EvaluateExpression(expression), Throws.Exception.Message.EqualTo(message));
        }

        public void AssertExpressionError(string message, int position, string expression)
        {
            message = TestStylizer.GetErrorMessage(message, expression, 3, position);
            AssertExpressionError(message, expression);
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

            var less = string.Format(".def {{\nexpression:\n{0}\n;}}\n {1}", expression, variables);

            var css = Evaluate(less);

            if (string.IsNullOrEmpty(css))
                return "";

            var start = css.IndexOf("expression:");
            var end = css.LastIndexOf(";");

            return css.Substring(start + 11, end - start - 11).Trim();
        }

        public string Evaluate(string input)
        {
            return Evaluate(input, DefaultParser());
        }

        public string Evaluate(string input, Parser parser)
        {
            var tree = parser.Parse(input.Trim(), null);
            return tree.ToCSS(DefaultEnv());
        }
    }
}