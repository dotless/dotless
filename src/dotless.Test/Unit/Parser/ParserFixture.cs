using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using dotless.Core.Parser.Infrastructure.Nodes;
using dotless.Core.Parser.Tree;
using NUnit.Framework;

namespace dotless.Test.Unit.Parser
{
    [TestFixture]
    public class ParserFixture
    {
        [Test]
        public void OneLineRuleEndingInSemicolonIsParsedToNodes() {

            var input = ".no-semi-colon { border: 2px solid white; }";

            var parser = new Core.Parser.Parser();
            var ruleset = parser.Parse(input, "");

            var firstRuleset = (Ruleset)ruleset.Rules[0];
            var firstRule = (Rule)firstRuleset.Rules[0];

            Assert.That(firstRule.Value, Is.InstanceOf<Value>());

            var value = (Value) firstRule.Value;
            var valueExpression = (Expression)value.Values[0];

            var valueNodes = valueExpression.Value;

            Assert.That(valueNodes.Count, Is.EqualTo(3));

            Assert.That(valueNodes[0], Is.InstanceOf<Number>());
            Assert.That(valueNodes[1], Is.InstanceOf<Keyword>());
            Assert.That(valueNodes[2], Is.InstanceOf<Keyword>());
        }
    }
}
