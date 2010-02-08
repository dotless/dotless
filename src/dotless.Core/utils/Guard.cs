using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotless.Core.engine;
using dotless.Core.exceptions;

namespace dotless.Core.utils
{
    public static class Guard
    {
        public static void Expect(string expected, string actual, object @in)
        {
            if(actual == expected)
                return;

            var message = string.Format("Expected '{0}' in {1}, found '{2}'", expected, @in, actual);

            throw new ParsingException(message);
        }

        public static void ExpectAllNodes<TExpected>(IEnumerable<INode> actual, object @in) where TExpected : INode
        {
            foreach (var node in actual)
            {
                ExpectNode<TExpected>(node, @in);
            }
        }

        public static void ExpectNode<TExpected>(INode actual, object @in) where TExpected : INode
        {
            if (actual is TExpected)
                return;

            var expected = typeof(TExpected).Name.ToLowerInvariant();

            var message = string.Format("Expected {0} in {1}, found {2}", expected, @in, actual.ToCss());

            throw new ParsingException(message);
        }


        public static void ExpectArguments(int expected, int actual, object @in)
        {
            if (actual == expected)
                return;

            var message = string.Format("Expected {0} arguments in {1}, found {2}", expected, @in, actual);

            throw new ParsingException(message);
        }

        public static void ExpectMinArguments(int expected, int actual, object @in)
        {
            if (actual >= expected)
                return;

            var message = string.Format("Expected at least {0} arguments in {1}, found {2}", expected, @in, actual);

            throw new ParsingException(message);
        }

        public static void ExpectMaxArguments(int expected, int actual, object @in)
        {
            if (actual <= expected)
                return;

            var message = string.Format("Expected at most {0} arguments in {1}, found {2}", expected, @in, actual);

            throw new ParsingException(message);
        }
    }
}
