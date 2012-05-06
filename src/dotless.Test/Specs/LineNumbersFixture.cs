namespace dotless.Test.Specs
{
    using System.Collections.Generic;
    using Core.Importers;
    using Core.Parser;
    using NUnit.Framework;

    public class LineNumbersFixture : SpecFixtureBase
    {
        protected Dictionary<string, string> Imports { get; set; }

        [SetUp]
        public void SetUp()
        {
            var baseEnv = DefaultEnv;
            DefaultEnv = () =>
                {
                    var env = baseEnv();
                    env.Debug = true;
                    env.Compress = false;
                    return env;
                };

            Imports = new Dictionary<string, string>();
            DefaultImporter = () => new Importer(new DictionaryReader(Imports));

            DefaultParser = () => new Parser(Optimisation, DefaultStylizer(), DefaultImporter(), true);
        }

        [Test]
        public void LineNumbers()
        {
            var input = @"
.first {
  color: red;
}

.second {
  color: green;

  .inner {
    color: blue;
  }
}
";

            var expected= @"
/* test.less:L1 */
.first {
  color: red;
}
/* test.less:L5 */
.second {
  color: green;
}
/* test.less:L8 */
.second .inner {
  color: blue;
}
";

            AssertLess(input, expected);
        }

        [Test]
        public void LineNumbersAndFileNameWhenImporting()
        {
            Imports["import.less"] = @".import { color: blue; }";

            var input = @"
@import 'import.less';
";

            var expected = @"
/* import.less:L1 */
.import {
  color: blue;
}
";
            AssertLess(input, expected);
        }

        [Test]
        public void LineNumbersWhenCallingMixin()
        {
            var input = @"
.mixin() {
  color: red;

  .inner { // currently indicates this line
    color: blue;
  }
}

.test {
  .mixin(); // should maybe indicate this line! or both?
}
";

            var expected= @"
/* test.less:L9 */
.test {
  color: red;
}
/* test.less:L4 */
.test .inner {
  color: blue;
}
";

            AssertLess(input, expected);
        }
    }
}