namespace dotless.Test
{
    using System;
    using Core.Importers;
    using Core.Parser;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Parser.Infrastructure;
    using Core.Stylizers;
    using NUnit.Framework;
    using Plugins;
    using dotless.Core.Loggers;
    using System.Text;

    public class SpecFixtureBase
    {
        protected Func<IStylizer> DefaultStylizer;
        protected Func<IImporter> DefaultImporter { get; set; }
        protected Func<Parser> DefaultParser;
        protected Func<Env> DefaultEnv;
        protected int Optimisation { get; set; }
        public PassThroughBeforePlugin PassThroughBeforePlugin { get; private set; }
        public PassThroughAfterPlugin PassThroughAfterPlugin { get; private set; }

        [SetUp]
        public void SetupParser()
        {
            Optimisation = 1;
            DefaultStylizer = () => new PlainStylizer();
            DefaultImporter = () => new Importer();
            DefaultParser = () => new Parser(Optimisation, DefaultStylizer(), DefaultImporter());
            DefaultEnv = () =>
            {
                var env = new Env(DefaultParser());
                env.AddPlugin(PassThroughAfterPlugin = new PassThroughAfterPlugin());
                env.AddPlugin(PassThroughBeforePlugin = new PassThroughBeforePlugin());
                return env;
            };

        }

        protected void AssertLess(string input, string expected)
        {
            AssertLess(input, expected, DefaultParser());
        }

        protected void AssertLess(string input, string expected, Parser parser)
        {
            var output = Evaluate(input, parser).Trim().Replace("\r\n", "\n");

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

        protected void AssertMatchExpression(string pattern, string expression)
        {
            AssertMatchExpression(pattern, expression, null);
        }

        protected void AssertMatchExpression(string pattern, string expression, IDictionary<string, string> variables)
        {
            Assert.That(EvaluateExpression(expression, variables), Is.StringMatching(pattern));
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
            var oldStylizer = DefaultStylizer;
            DefaultStylizer = () => new TestStylizer();

            message = TestStylizer.GetErrorMessage(message, line, lineNumber, position, call, callLine);
            AssertError(message, input);

            DefaultStylizer = oldStylizer;
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
            var oldStylizer = DefaultStylizer;
            DefaultStylizer = () => new TestStylizer();

            message = TestStylizer.GetErrorMessage(message, expression, 3, position, null, 0);
            AssertExpressionError(message, expression, variables);

            DefaultStylizer = oldStylizer;
        }

        protected void AssertExpressionLogMessage(string message, string expression)
        {
            List<string> logMessages;
            EvaluateExpression(expression, null, out logMessages);

            StringBuilder formattedLogMessages = new StringBuilder();
            foreach (string logMessage in logMessages)
            {
                if (message == logMessage)
                {
                    return;
                }
                formattedLogMessages.AppendLine(logMessage);
            }

            Assert.Fail("Log messages did not contain '{0}'.\nLog Messages:\n{1}", message, formattedLogMessages.ToString().TrimEnd());
        }

        protected void AssertExpressionNoLogMessage(string message, string expression)
        {
            List<string> logMessages;
            EvaluateExpression(expression, null, out logMessages);

            foreach (string logMessage in logMessages)
            {
                if (message == logMessage)
                {
                    Assert.Fail("Message '{0}' found.", message);
                }
            }
        }

        protected string EvaluateExpression(string expression)
        {
            List<string> logMessages;
            return EvaluateExpression(expression, null, out logMessages);
        }

        protected string EvaluateExpression(string expression, IDictionary<string, string> variablesDictionary)
        {
            List<string> logMessages;
            return EvaluateExpression(expression, variablesDictionary, out logMessages);
        }

        protected string EvaluateExpression(string expression, IDictionary<string, string> variablesDictionary, out List<string> logMessages)
        {
            var variables = "";
            if (variablesDictionary != null)
                variables = variablesDictionary.Aggregate("",
                                                          (s, x) =>
                                                          string.Format("{0}  @{1}: {2};", s, x.Key, x.Value));

            var less = string.Format("{1} .def {{\nexpression:\n{0}\n;}}", expression, variables);
            TestLogger testLogger;
            var css = Evaluate(less, out testLogger);
            logMessages = testLogger.LogMessages;

            if (string.IsNullOrEmpty(css))
                return "";

            var start = css.IndexOf("expression:");
            var end =  css.LastIndexOf(DefaultEnv().Compress ? '}' : ';');

            return css.Substring(start + 11, end - start - 11).Trim();
        }

        public string Evaluate(string input)
        {
            return Evaluate(input, DefaultParser(), "test.less");
        }

        public string Evaluate(string input, string filename)
        {
            return Evaluate(input, DefaultParser(), filename);
        }

        public string Evaluate(string input, out TestLogger testLogger)
        {
            return Evaluate(input, DefaultParser(), out testLogger, "test.less");
        }

        public string Evaluate(string input, Parser parser)
        {
            TestLogger outParam;
            return Evaluate(input, parser, out outParam, "test.less");
        }

        public string Evaluate(string input, Parser parser, string filename)
        {
            TestLogger outParam;
            return Evaluate(input, parser, out outParam, filename);
        }

        public string Evaluate(string input, Parser parser, out TestLogger testLogger, string filename)
        {
            var tree = parser.Parse(input.Trim(), filename);
            var env = DefaultEnv();
            env.Parser = parser;
            env.Logger = testLogger = new TestLogger(LogLevel.Info);
            return tree.ToCSS(env);
        }
    }
}