using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotless.Core.Test.Specs
{
    using NUnit.Framework;

    public class GuardedRulesetsFixture : SpecFixtureBase
    {
        [Test]
        public void GuardedRulesetWithSelector()
        {
            var input =
                @"
@a: true;

.someClass when (@a = true) {
  color: white;
}
.someClass when (@a = false) {
  color: black;
}";

            var expected =
                @"
.someClass {
  color: white;
}";

            AssertLess(input, expected);
        }
        [Test]
        public void NestedGuardedRulesetWithSelector()
        {
            var input =
                @"
@a: true;

.someClass {
  #someId1 when (@a = true) {
    color: white;
  }
  #someId2 when (@a = false) {
    color: black;
  }
  font-weight: bold;
}";

            var expected =
                @"
.someClass {
  
  font-weight: bold;
}
.someClass #someId1 {
  color: white;
}";

            AssertLess(input, expected);
        }
    }
}
