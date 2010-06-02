using System.Collections.Generic;
using System.Linq;
using dotless.Infrastructure;
using dotless.Tree;
using NUnit.Framework;

namespace dotless.Tests
{
  public class SpecFixtureBase
  {
    protected static void AssertLess(string input, string expected)
    {
      AssertLess(input, expected, new Parser(0));
    }

    protected static void AssertLess(string input, string expected, Parser parser)
    {
      var output = Evaluate(input, parser).Trim();

      expected = expected.Trim().Replace("\r\n", "\n");

      Assert.That(output, Is.EqualTo(expected));
    }

    protected static void AssertLessUnchanged(string input)
    {
      AssertLess(input, input);
    }

    protected static void AssertExpression(string output, string expression)
    {
      AssertExpression(output, expression, null);
    }

    protected static void AssertExpression(string output, string expression, IDictionary<string, string> variables)
    {
      Assert.That(EvaluateExpression(expression, variables), Is.EqualTo(output));
    }

    protected void AssertExpressionUnchanged(string expression)
    {
      AssertExpression(expression, expression);
    }

    protected static void AssertError(string message, string input)
    {
      Assert.That(() => Evaluate(input), Throws.Exception.Message.EqualTo(message));
    }

    protected static void AssertExpressionError(string message, string expression)
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
        variables = variablesDictionary.Aggregate("", (s, x) => string.Format("{0}\n  @{1}: {2};", s, x.Key, x.Value));

      var less = string.Format(".def {{{0}\n  expression: {1};\n}}", variables, expression);

      var css = Evaluate(less);

      if (string.IsNullOrEmpty(css))
        return "";

      var start = css.IndexOf("  expression: ");
      var end = css.LastIndexOf("}");

      return css.Substring(start + 14, end - start - 16);
    }

    public static string Evaluate(string input)
    {
      return Evaluate(input, new Parser(0));
    }

    public static string Evaluate(string input, Parser parser)
    {
      var tree = parser.Parse(input);
      return tree.ToCSS(new Env());
    }
  }
}