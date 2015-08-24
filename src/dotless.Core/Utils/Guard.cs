using System;

namespace dotless.Core.Utils
{
    using System.Collections.Generic;
    using Exceptions;
    using Parser.Infrastructure;
    using Parser.Infrastructure.Nodes;
    using dotless.Core.Parser;

    public static class Guard
    {
        public static void Expect(string expected, string actual, object @in, NodeLocation location)
        {
            if (actual == expected)
                return;

            var message = string.Format("Expected '{0}' in {1}, found '{2}'", expected, @in, actual);

            throw new ParsingException(message, location);
        }

        public static void Expect(bool condition, string message, NodeLocation location)
        {
            if (condition)
                return;

            throw new ParsingException(message, location);
        }

        public static TExpected ExpectNode<TExpected>(Node actual, object @in, NodeLocation location) where TExpected : Node
        {
            if (actual is TExpected)
                return (TExpected) actual;

            var expected = typeof (TExpected).Name.ToLowerInvariant();

            var message = string.Format("Expected {0} in {1}, found {2}", expected, @in, actual.ToCSS(new Env(null)));

            throw new ParsingException(message, location);
        }

        public static void ExpectNodeToBeOneOf<TExpected1, TExpected2>(Node actual, object @in, NodeLocation location) where TExpected1 : Node where TExpected2 : Node
        {
            if (actual is TExpected1 || actual is TExpected2)
                return;

            var expected1 = typeof(TExpected1).Name.ToLowerInvariant();
            var expected2 = typeof(TExpected2).Name.ToLowerInvariant();

            var message = string.Format("Expected {0} or {1} in {2}, found {3}", expected1, expected2, @in, actual.ToCSS(new Env(null)));

            throw new ParsingException(message, location);
        }

        public static void ExpectAllNodes<TExpected>(IEnumerable<Node> actual, object @in, NodeLocation location) where TExpected : Node
        {
            foreach (var node in actual)
            {
                ExpectNode<TExpected>(node, @in, location);
            }
        }


        public static void ExpectNumArguments(int expected, int actual, object @in, NodeLocation location)
        {
            if (actual == expected)
                return;

            var message = string.Format("Expected {0} arguments in {1}, found {2}", expected, @in, actual);

            throw new ParsingException(message, location);
        }

        public static void ExpectMinArguments(int expected, int actual, object @in, NodeLocation location)
        {
            if (actual >= expected)
                return;

            var message = string.Format("Expected at least {0} arguments in {1}, found {2}", expected, @in, actual);

            throw new ParsingException(message, location);
        }

        public static void ExpectMaxArguments(int expected, int actual, object @in, NodeLocation location)
        {
            if (actual <= expected)
                return;

            var message = string.Format("Expected at most {0} arguments in {1}, found {2}", expected, @in, actual);

            throw new ParsingException(message, location);
        }

         
    }
}