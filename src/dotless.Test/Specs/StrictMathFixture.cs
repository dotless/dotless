using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace dotless.Test.Specs
{
    [TestFixture]
    public class StrictMathFixture : SpecFixtureBase
    {

        [Test]
        public void StrictMathNoParenthesesLeavesExpressionUntouched()
        {
            var parser = DefaultParser();
            parser.StrictMath = true;

            AssertLessUnchanged("border-radius: 10px / 12px;", parser);
            AssertLessUnchanged("width: calc(10px + 12px);", parser);
        }

        [Test]
        public void StrictMathParenthesesEvaluatesExpression()
        {
            var parser = DefaultParser();
            parser.StrictMath = true;

            AssertLess("border-radius: (12px / 10px) / (8px / 4px);", "border-radius: 1.2px / 2px;", parser);
        }

        [Test]
        public void NonStrictMathWithoutParenthesesEvaluatesExpression()
        {
            var parser = DefaultParser();
            parser.StrictMath = false;

            AssertLess("border-radius: 12px / 10px;", "border-radius: 1.2px;", parser);
            AssertLess("border-radius: (12px / 10px) / (8px / 4px);", "border-radius: 0.6px;", parser);
        }
    }
}
