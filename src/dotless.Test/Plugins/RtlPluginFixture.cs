namespace dotless.Test.Plugins
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Core.Parser.Infrastructure;
    using Core.Parser.Tree;
    using Core.Plugins;
    using NUnit.Framework;
    using System.Globalization;
    using System.Threading;

    public class RtlPluginFixture : SpecFixtureBase
    {
        public bool OnlyReversePrefixedRules
        {
            get;
            set;
        }

        public void SetCultureTextDirection(bool isRtl)
        {
            if (isRtl)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("ar-SA");
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            }
        }

        [SetUp]
        public void Setup()
        {
            DefaultEnv = () => {
                Env env = new Env(null);
                env.AddPlugin(new RtlPlugin() { OnlyReversePrefixedRules = OnlyReversePrefixedRules});
                return env;
            };
        }

        [Test]
        public void PropertyRemovalLtr()
        {
            OnlyReversePrefixedRules = true;
            SetCultureTextDirection(false);

            var input =
                @"
.cl {
  -rtl-background: black;
  -ltr-background: red;
}
";
            var expected = @"
.cl {
  background: red;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void PropertyRemovalRtl()
        {
            OnlyReversePrefixedRules = true;
            SetCultureTextDirection(true);

            var input =
                @"
.cl {
  -rtl-background: black;
  -ltr-background: red;
}
";
            var expected = @"
.cl {
  background: black;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void PropertyNonRemovalRtl1()
        {
            OnlyReversePrefixedRules = true;
            SetCultureTextDirection(true);

            var input =
                @"
.cl {
  -rtl-ltr-background: black;
  -ltr-rtl-border-color: red;
}
";
            var expected = @"
.cl {
  background: black;
  border-color: red;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void PropertyNonRemovalRtl2()
        {
            SetCultureTextDirection(true);

            var input =
                @"
.cl {
  -rtl-ltr-background: black;
  -ltr-rtl-border-color: red;
}
";
            var expected = @"
.cl {
  background: black;
  border-color: red;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void PropertyRemovalRemoveClass()
        {
            OnlyReversePrefixedRules = true;
            SetCultureTextDirection(false);

            var input =
                @"
.cl {
  -rtl-background: black;
  -rtl-border: 1px;
}
";
            var expected = "";

            AssertLess(input, expected);
        }

        [Test]
        public void PropertyNameReversalLtr1()
        {
            OnlyReversePrefixedRules = true;
            SetCultureTextDirection(false);

            var input =
                @"
.cl {
  -ltr-reverse-border-width-left: 2px;
  -ltr-margin-left: 1px;
  padding-right: 2px;
}
";
            var expected = @"
.cl {
  border-width-right: 2px;
  margin-left: 1px;
  padding-right: 2px;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void PropertyNameReversalLtr2()
        {
            OnlyReversePrefixedRules = false;
            SetCultureTextDirection(false);

            var input =
                @"
.cl {
  -ltr-reverse-border-width-left: 2px;
  -ltr-margin-left: 1px;
  padding-right: 2px;
}
";
            var expected = @"
.cl {
  border-width-right: 2px;
  margin-left: 1px;
  padding-right: 2px;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void PropertyNameReversalRtl1()
        {
            OnlyReversePrefixedRules = true;
            SetCultureTextDirection(true);

            var input =
                @"
.cl {
  -rtl-reverse-border-width-left: 2px;
  -rtl-margin-left: 1px;
  padding-right: 2px;
}
";
            var expected = @"
.cl {
  border-width-right: 2px;
  margin-left: 1px;
  padding-right: 2px;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void PropertyNameReversalRtl2()
        {
            OnlyReversePrefixedRules = false;
            SetCultureTextDirection(true);

            var input =
                @"
.cl {
  -rtl-reverse-border-width-left: 2px;
  -rtl-margin-left: 1px;
  padding-right: 2px;
  padding-left: 3px;
  right: 4px;
  left: 5px;
  border-left-width: 6px;
  border-top-width: 7px;
}
";
            var expected = @"
.cl {
  border-width-right: 2px;
  margin-left: 1px;
  padding-left: 2px;
  padding-right: 3px;
  left: 4px;
  right: 5px;
  border-right-width: 6px;
  border-top-width: 7px;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void PropertyReversal()
        {
            OnlyReversePrefixedRules = false;
            SetCultureTextDirection(true);

            var input =
                @"
.cl {
  padding: 1px 2px;
  border-width: 1px 2px 3px;
  border-width: 1px 2px thick 3px;
  border-width: 1px thick 2px 3px;
  margin: 1px 2px 3px 4px;
  float: left;
}
";
            var expected = @"
.cl {
  padding: 1px 2px;
  border-width: 1px 2px 3px;
  border-width: 1px 3px thick 2px;
  border-width: 1px 3px 2px thick;
  margin: 1px 4px 3px 2px;
  float: right;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void PropertyReversalComments()
        {
            OnlyReversePrefixedRules = false;
            SetCultureTextDirection(true);

            var input =
                @"
.cl {
  border-width: 1px/**/ /**/2px 3px/**/;
  border-width: 1px/**/ /**/2px thick 3px/**/;
  border-width: 1px/**/ thick /**/2px 3px/**/;
  margin: 1px 2px /**/ 3px 4px;
  float: /**/left/**/;
}
";
            var expected = @"
.cl {
  border-width: 1px/**//**/ 2px 3px/**/;
  border-width: 1px/**//**/ 3px/**/ thick 2px;
  border-width: 1px/**/ 3px/**/ 2px thick/**/;
  margin: 1px 4px 3px 2px/**/;
  float: right;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void FloatWithImportantIsReversed()
        {
            var input = @"
.test {
  float: left !important;
}
";

            var expected = @"
.test {
  float: right !important;
}
";

            OnlyReversePrefixedRules = false;
            SetCultureTextDirection(true);
            AssertLess(input, expected);
        }

        [Test]
        public void TextAlignIsReversed()
        {
            var input = @"
.test {
  text-align: left;
}
";

            var expected = @"
.test {
  text-align: right;
}
";

            OnlyReversePrefixedRules = false;
            SetCultureTextDirection(true);
            AssertLess(input, expected);
        }

        [Test]
        public void BorderRadiusIsReversed()
        {
            var input = @"
.test {
  border-top-left-radius: 1px;
  border-top-right-radius: 2px;
  border-bottom-left-radius: 3px;
  border-bottom-right-radius: 4px;
}
";

            var expected = @"
.test {
  border-top-right-radius: 1px;
  border-top-left-radius: 2px;
  border-bottom-right-radius: 3px;
  border-bottom-left-radius: 4px;
}
";

            OnlyReversePrefixedRules = false;
            SetCultureTextDirection(true);
            AssertLess(input, expected);
        }

        [Test]
        public void PatternMatchingMixinDoesNotCauseException() {
            var input = @"
.theme(""black"") { }
.theme(""black"");
";

            OnlyReversePrefixedRules = false;
            SetCultureTextDirection(true);
            AssertLess(input, "");
        }

        [Test]
        public void DoesNotLoseImportant() {
            var input = @"
.selector { border-width: 1px 2px 3px 4px !important; }
";

            var expected = @"
.selector {
  border-width: 1px 4px 3px 2px !important;
}";

            OnlyReversePrefixedRules = false;
            SetCultureTextDirection(true);
            AssertLess(input, expected);
        }
    }
}
