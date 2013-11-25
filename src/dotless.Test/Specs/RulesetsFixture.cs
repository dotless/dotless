namespace dotless.Test.Specs
{
    using NUnit.Framework;

    public class RulesetsFixture : SpecFixtureBase
    {
        [Test]
        public void Rulesets()
        {
            // Todo: split into separate atomic tests.
            var input =
                @"
#first > .one {
  > #second .two > #deux {
    width: 50%;
    #third {
      &:focus {
        color: black;
        #fifth {
          > #sixth {
            .seventh #eighth {
              + #ninth {
                color: purple;
              }
            }
          }
        }
      }
      height: 100%;
    }
    #fourth, #five, #six {
      color: #110000;
      .seven, .eight > #nine {
        border: 1px solid black;
      }
      #ten {
        color: red;
      }
    }
  }
  font-size: 2em;
}
";

            var expected =
                @"
#first > .one {
  font-size: 2em;
}
#first > .one > #second .two > #deux {
  width: 50%;
}
#first > .one > #second .two > #deux #third {
  height: 100%;
}
#first > .one > #second .two > #deux #third:focus {
  color: black;
}
#first > .one > #second .two > #deux #third:focus #fifth > #sixth .seventh #eighth + #ninth {
  color: purple;
}
#first > .one > #second .two > #deux #fourth,
#first > .one > #second .two > #deux #five,
#first > .one > #second .two > #deux #six {
  color: #110000;
}
#first > .one > #second .two > #deux #fourth .seven,
#first > .one > #second .two > #deux #five .seven,
#first > .one > #second .two > #deux #six .seven,
#first > .one > #second .two > #deux #fourth .eight > #nine,
#first > .one > #second .two > #deux #five .eight > #nine,
#first > .one > #second .two > #deux #six .eight > #nine {
  border: 1px solid black;
}
#first > .one > #second .two > #deux #fourth #ten,
#first > .one > #second .two > #deux #five #ten,
#first > .one > #second .two > #deux #six #ten {
  color: red;
}
";

            AssertLess(input, expected);
        }

        [Test, Ignore("Unsupported")]
        public void ImplicitParentSelectorPseudoClass()
        {
            // Note: http://github.com/cloudhead/less.js/issues/issue/7

            var input = @"
a {
  color: black;
  :hover {
    text-decoration: underline;
  }
}
";
            var expected = @"
a {
  color: black;
}
a:hover {
  text-decoration: underline;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void IESlash9HackAllowedForEvaluatedValues()
        {
            // This has been a problem, where a value must be evaluated a ParsingException would be raised if that value was followed by the
            // IE-targeting "\9" hack
            var input = @"li {
  color: #f00 \9;
}
";
            var expected = @"li {
  color: red \9;
}
";
            AssertLess(input, expected);
        }

        [Test]
        public void IESlash9HackAllowedForNonEvaluatedValues()
        {
            // This has never been a problem but this test is here to illustrate the difference between this parsing and the work required
            // in IESlash9HackAllowedForEvaluatedValues
            var input = @"li {
  color: red \9;
}
";
            var expected = input;
            AssertLess(input, expected);
        }
    }
}