using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace dotless.Core.Test.Specs.Functions
{
    public class DefaultFixture : SpecFixtureBase
    {
        [Test]
        public void DefaultFunctionInRuleResultsInText() {
            AssertLessUnchanged("rule: default();");
        }
    }
}
