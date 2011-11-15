namespace dotless.Test.Specs.Compression
{
    using System.Collections.Generic;
    using dotless.Core.Importers;
    using dotless.Core.Parser;
    using NUnit.Framework;

    public class ImportFixture : CompressedSpecFixtureBase
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

            var expected = "@import \"import-test-d.css\";#import{color:red;}.mixin{height:10px;color:red;}#import-test{height:10px;color:red;width:10px;height:30%;}";

            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void ImportFleExtensionNotNecessary()
        {
            var input =
                @"
@import url(""import/import-test-c"");
";

            var expected = "@import \"import-test-d.css\";#import{color:red;}";

            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void ImportForUrlGetsOutput()
        {
            var input =
                @"
@import url(""http://www.someone.com/external1.css"");@import ""http://www.someone.com/external2.css"";
";

            var parser = GetParser();

            AssertLessUnchanged(input, parser);
        }

        [Test]
        public void ImportForMissingLessFileThrowsError1()
        {
            var input =
                @"
@import url(""http://www.someone.com/external1.less"");
";

            var parser = GetParser();

            AssertError("You are importing a file ending in .less that cannot be found", input, parser);
        }

        [Test]
        public void ImportForMissingLessFileThrowsError2()
        {
            var input =
                @"
@import ""external1.less"";
";

            var parser = GetParser();

            AssertError("You are importing a file ending in .less that cannot be found", input, parser);
        }

        [Test]
        public void ImportForMissingLessFileThrowsError3()
        {
            var input =
                @"
@import ""http://www.someone.com/external1.less"";
";

            var parser = GetParser();

            AssertError("You are importing a file ending in .less that cannot be found", input, parser);
        }

    }
}