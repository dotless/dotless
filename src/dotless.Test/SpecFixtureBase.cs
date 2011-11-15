namespace dotless.Test
{
    using System;
    using System.Text.RegularExpressions;
    using Core.Importers;
    using Core.Parser;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Parser.Infrastructure;
    using Core.Stylizers;
    using NUnit.Framework;

    public class SpecFixtureBase
    {
        protected Func<IStylizer> DefaultStylizer;
        protected Func<Parser> DefaultParser;
        protected Func<Env> DefaultEnv;
        protected int Optimisation { get; set; }

        [SetUp]
        public void SetupParser()
        {
            Optimisation = 1;
            DefaultStylizer = () => new PlainStylizer();
            DefaultParser = () => new Parser(Optimisation, DefaultStylizer(), new Importer());
            DefaultEnv = () => new Env();
        }

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

        protected void AssertError(string message, string input, Parser parser)
        {
            Assert.That(() => Evaluate(input, parser), Throws.Exception.Message.EqualTo(message));
        }

        public void AssertError(string message, string line, int lineNumber, int position, string input)
        {
            AssertError(message, line, lineNumber, position, null, 0, input);
        }

        public void AssertError(string message, string line, int lineNumber, int position, string call, int callLine, string input)
        {
            DefaultStylizer = () => new TestStylizer();
            message = TestStylizer.GetErrorMessage(message, line, lineNumber, position, call, callLine);
            AssertError(message, input);
        }

        protected void AssertExpressionError(string message, string expression, IDictionary<string, string> variables)
        {
            Assert.That(() => EvaluateExpression(expression, variables), Throws.Exception.Message.EqualTo(message));
        }

        public void AssertExpressionError(string message, int position, string expression)
        {
            AssertExpressionError(message, position, expression, null);
        }

        public void AssertExpressionError(string message, int position, string expression, IDictionary<string, string> variables)
        {
            DefaultStylizer = () => new TestStylizer();
            message = TestStylizer.GetErrorMessage(message, expression, 3, position, null, 0);
            AssertExpressionError(message, expression, variables);
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
                                                          string.Format("{0}  @{1}: {2};", s, x.Key, x.Value));

            var less = string.Format("{1} .def {{\nexpression:\n{0}\n;}}", expression, variables);

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