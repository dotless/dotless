namespace dotless.Core.Test.Specs
{
    using NUnit.Framework;

    public class ScriptFixture : SpecFixtureBase
    {
        [Test, Ignore("Unsupported")]
        public void ScriptExpressions()
        {
            AssertExpression("42", "`42`");
            AssertExpression("4", "`2 + 2`");
            AssertExpression("'hello world'", "`'hello world'`");
        }

        [Test]
        public void ScriptUnsupported()
        {
            AssertExpression("[script unsupported]", "`42`");
            AssertExpression("[script unsupported]", "`2 + 2`");
            AssertExpression("[script unsupported]", "`'hello world'`");
        }

        [Test, Ignore("Unsupported")]
        public void ScriptHasAccessToVariablesInScope1()
        {
            var input = @"
.scope {
  @foo: 42;
  var: `this.foo`;
}";

            var expected = @"
.scope {
  var: 42;
}";

            AssertLess(input, expected);
        }

        [Test, Ignore("Unsupported")]
        public void ScriptHasAccessToVariablesInScope2()
        {
            var input = @"
@foo: 42;
.scope {
  var: `this.foo`;
}";

            var expected = @"
.scope {
  var: 42;
}";

            AssertLess(input, expected);
        }
    }
}
