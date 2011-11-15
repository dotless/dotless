namespace dotless.Test.Specs
{
    using System.Collections.Generic;
    using Core.Importers;
    using Core.Parser;
    using NUnit.Framework;

    public class ImportFixture : SpecFixtureBase
    {
        private static Parser GetParser()
        {
            var imports = new Dictionary<string, string>();

            imports["import/import-test-a.less"] = @"
@import ""import-test-b.less"";
@a: 20%;
";
            imports["import/import-test-b.less"] =
                @"
@import 'import-test-c';

@b: 100%;

.mixin {
  height: 10px;
  color: @c;
}
";
            imports["import/import-test-c.less"] =
                @"
@import ""import-test-d.css"";
@c: red;

#import {
  color: @c;
}
";
          
            imports["import/first.less"] =
                @"
@import ""sub1/second.less"";

#first {
  background: url('../image.gif');
  background: url(../image.gif);
}
";          
            imports["import/sub1/second.less"] =
                @"
#second {
  background: url(../image.gif);
  background: url(image.gif);
  background: url(sub2/image.gif);
}
";

            imports["/import/absolute.less"] = @"body { background-color: black; }";
            imports["../import/relative-with-parent-dir.less"] = @"body { background-color: foo; }";

            return new Parser {Importer = new Importer(new DictionaryReader(imports))};
        }

        [Test]
        public void Imports()
        {
            var input =
                @"
@import url(""import/import-test-a.less"");
//@import url(""import/import-test-a.less"");

#import-test {
  .mixin;
  width: 10px;
  height: @a + 10%;
}
";

            var expected =
                @"
@import ""import-test-d.css"";
#import {
  color: red;
}
.mixin {
  height: 10px;
  color: red;
}
#import-test {
  height: 10px;
  color: red;
  width: 10px;
  height: 30%;
}
";

            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void RelativeUrls()
        {
            var input =
                @"
@import url(""import/first.less"");
";

            var expected =
                @"
#second {
  background: url(import/image.gif);
  background: url(import/sub1/image.gif);
  background: url(import/sub1/sub2/image.gif);
}
#first {
  background: url('image.gif');
  background: url(image.gif);
}
";

            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void AbsoluteUrls()
        {
            var input =
                @"
@import url(""/import/absolute.less"");
";

            var expected = @"body {
  background-color: black;
}";

            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void RelativeUrlWithParentDirReference()
        {
            var input =
                @"
@import url(""../import/relative-with-parent-dir.less"");
";

            var expected = @"body {
  background-color: foo;
}";

            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void ImportFileExtensionNotNecessary()
        {
            var input = @"@import url(""import/import-test-c"");";

            var expected = @"
@import ""import-test-d.css"";
#import {
  color: red;
}";

            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void ImportForUrlGetsOutput()
        {
            var input =
                @"
@import url(""http://www.someone.com/external1.css"");
@import ""http://www.someone.com/external2.css"";
";

            var parser = GetParser();

            AssertLessUnchanged(input, parser);
        }

        [Test]
        public void ImportForMissingLessFileThrowsError1()
        {
            var input = @"@import url(""http://www.someone.com/external1.less"");";

            var parser = GetParser();

            AssertError("You are importing a file ending in .less that cannot be found", input, parser);
        }

        [Test]
        public void ImportForMissingLessFileThrowsError2()
        {
            var input = @"@import ""external1.less"";";

            var parser = GetParser();

            AssertError("You are importing a file ending in .less that cannot be found", input, parser);
        }

        [Test]
        public void ImportForMissingLessFileThrowsError3()
        {
            var input = @"@import ""http://www.someone.com/external1.less"";";

            var parser = GetParser();

            AssertError("You are importing a file ending in .less that cannot be found", input, parser);
        }
    }
}