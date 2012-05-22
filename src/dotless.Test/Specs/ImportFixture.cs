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
            return GetParser(false, false, false);
        }

        private static Parser GetParser(bool isUrlRewritingDisabled, bool importAllFilesAsLess, bool importCssInline)
        {
            var imports = new Dictionary<string, string>();

            imports["import/import-test-a.less"] = @"
@import ""import-test-b.less"";
@a: 20%;
";
            imports[@"c:/absolute/file.less"] = @"
.windowz .dos {
  border: none;
}
";
            imports["import/other-protocol-test.less"] = @"
.first {
    background-image: url('http://some.com/file.gif');
    background-image: url('https://some.com/file.gif');
    background-image: url('ftp://some.com/file.gif');
    background-image: url('data:xxyhjgjshgjs');
}
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

@path: ""../image.gif"";
#first {
  background: url('../image.gif');
  background: url(../image.gif);
  background: url(@path);
}
";          
            imports["import/sub1/second.less"] =
                @"
@pathsep: '/';
#second {
  background: url(../image.gif);
  background: url(image.gif);
  background: url(sub2/image.gif);
  background: url(/sub2/image.gif);
  background: url(~""@{pathsep}sub2/image2.gif"");
}
";

            imports["/import/absolute.less"] = @"body { background-color: black; }";
            imports["../import/relative-with-parent-dir.less"] = @"body { background-color: foo; }";

            imports["foo.less"] = @"@import ""foo/bar.less"";";
            imports["foo/bar.less"] = @"@import ""../lib/color.less"";";
            imports["lib/color.less"] = "body { background-color: foo; }";

            imports["foourl.less"] = @"@import url(""foo/barurl.less"");";
            imports["foo/barurl.less"] = @"@import url(""../lib/colorurl.less"");";
            imports["lib/colorurl.less"] = "body { background-color: foo; }";

            imports["something.css"] = @"body { background-color: foo; invalid ""; }";

            imports["isless.css"] = @"
@a: 9px;
body { margin-right: @a; }";

            return new Parser { 
                Importer = new Importer(new DictionaryReader(imports)) { 
                    IsUrlRewritingDisabled = isUrlRewritingDisabled,
                    ImportAllFilesAsLess = importAllFilesAsLess,
                    InlineCssFiles = importCssInline} };
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
        public void OtherProtocolImportTest1()
        {
            var input = @"
@import 'import/other-protocol-test.less';
";
            var expected = @"
.first {
  background-image: url('http://some.com/file.gif');
  background-image: url('https://some.com/file.gif');
  background-image: url('ftp://some.com/file.gif');
  background-image: url('data:xxyhjgjshgjs');
}
";
            var parser = GetParser();
            
            DictionaryReader dictionaryReader = (DictionaryReader)((Importer)parser.Importer).FileReader;
            
            AssertLess(input, expected, parser);

            // Calling the file reader with url's with a protocolis asking for trouble
            Assert.AreEqual(2, dictionaryReader.DoesFileExistCalls.Count, "We should not ask the file reader if a protocol file exists");
            Assert.AreEqual(1, dictionaryReader.GetFileContentsCalls.Count, "We should not ask the file reader if a protocol file exists");

            Assert.AreEqual(@"import/other-protocol-test.less", dictionaryReader.DoesFileExistCalls[0], "We should not ask the file reader if a protocol file exists");
            Assert.AreEqual(@"import/other-protocol-test.less", dictionaryReader.DoesFileExistCalls[1], "We should not ask the file reader if a protocol file exists");
            Assert.AreEqual(@"import/other-protocol-test.less", dictionaryReader.GetFileContentsCalls[0], "We should not ask the file reader if a protocol file exists");
        }

        [Test]
        public void OtherProtocolImportTest2()
        {
            var input = @"
@import url(http://fonts.googleapis.com/css?family=Open+Sans:regular,bold);";

            var parser = GetParser();
            
            DictionaryReader dictionaryReader = (DictionaryReader)((Importer)parser.Importer).FileReader;
            
            AssertLessUnchanged(input, parser);

            // Calling the file reader with url's with a protocolis asking for trouble
            Assert.AreEqual(0, dictionaryReader.DoesFileExistCalls.Count, "We should not ask the file reader if a protocol file exists");
            Assert.AreEqual(0, dictionaryReader.GetFileContentsCalls.Count, "We should not ask the file reader if a protocol file exists");
        }

        [Test]
        public void OtherProtocolImportTest3()
        {
            var input = @"
@import url('c:/absolute/file.less');";
            var expected = @"
.windowz .dos {
  border: none;
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
  background: url(/sub2/image.gif);
  background: url(/sub2/image2.gif);
}
#first {
  background: url('image.gif');
  background: url(image.gif);
  background: url(""image.gif"");
}
";

            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void RelativeUrlsWithRewritingOff()
        {
            var input =
                @"
@import url(""import/first.less"");
";

            var expected =
                @"
#second {
  background: url(../image.gif);
  background: url(image.gif);
  background: url(sub2/image.gif);
  background: url(/sub2/image.gif);
  background: url(/sub2/image2.gif);
}
#first {
  background: url('../image.gif');
  background: url(../image.gif);
  background: url(""../image.gif"");
}
";

            var parser = GetParser(true, false, false);

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

            AssertError(".less cannot import non local less files.", input, parser);
        }

        [Test]
        public void ImportForMissingLessFileThrowsError2()
        {
            var input = @"@import ""external1.less"";";

            var parser = GetParser();

            AssertError("You are importing a file ending in .less that cannot be found.", input, parser);
        }

        [Test]
        public void ImportForMissingLessFileThrowsError3()
        {
            var input = @"@import ""http://www.someone.com/external1.less"";";

            var parser = GetParser();

            AssertError(".less cannot import non local less files.", input, parser);
        }

        [Test]
        public void ImportForMissingLessFileThrowsError4()
        {
            var input = @"@import ""dll://someassembly/missing.less"";";

            var parser = GetParser();

            AssertError("Unable to load resource [missing.less] in assembly [someassembly]", input, parser);
        }

        [Test]
        public void ImportForMissingCssFileAsLessThrowsError()
        {
            var input = @"@import ""dll://someassembly/missing.css"";";

            var parser = GetParser(false, true, false);

            AssertError("Unable to load resource [missing.css] in assembly [someassembly]", input, parser);
        }

        [Test]
        public void ImportForMissingLessFileThrowsExceptionThatIncludesFileName()
        {
            var input = @"@import ""external1.less"";";

            var parser = GetParser();

            Assert.That(() => Evaluate(input, parser),
                Throws.InstanceOf<System.IO.FileNotFoundException>()
                    .With.Property("FileName").EqualTo("external1.less"));
        }

        [Test]
        public void ImportCanNavigateIntoAndOutOfSubDirectory()
        {
            // Testing https://github.com/cloudhead/less.js/pull/514

            var input = @"@import ""foo.less"";";
            var expected = @"body {
  background-color: foo;
}";
            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void ImportCanNavigateIntoAndOutOfSubDirectoryWithImport()
        {
            var input = @"@import url(""foourl.less"");";
            var expected = @"body {
  background-color: foo;
}";
            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void ImportWithMediaSpecificationsSupported()
        {
            var input = @"
@import url(something.css) screen and (color) and (max-width: 600px);";

            AssertLessUnchanged(input);
        }

        [Test]
        public void ImportInlinedWithMediaSpecificationsSupported()
        {
            var input = @"
@import url(something.css) screen and (color) and (max-width: 600px);";

            var expected = @"
@media screen and (color) and (max-width: 600px) {
  body { background-color: foo; invalid ""; }
}
";

            AssertLess(input, expected, GetParser(false, false, true));
        }

        [Test]
        public void CanImportCssFilesAsLess()
        {
            var input = @"
@import url(""isless.css"");
";
            var expected = @"
body {
  margin-right: 9px;
}
";
            AssertLess(input, expected, GetParser(false, true, false));
        }

        [Test]
        public void LessImportWithMediaSpecificationsConverted()
        {
            var input = @"
@import url(foo.less) screen and (color) and (max-width: 600px);";

            var expected = @"
@media screen and (color) and (max-width: 600px) {
  body {
    background-color: foo;
  }
}";
            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void LessImportWithMediaSpecificationsConvertedMultipleRequirements()
        {
            var input = @"
@import url(foo.less) screen and (color) and (max-width: 600px), handheld and (min-width: 20em);";

            var expected = @"
@media screen and (color) and (max-width: 600px), handheld and (min-width: 20em) {
  body {
    background-color: foo;
  }
}";
            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void ImportWithMediaSpecificationsSupportedWithVariable()
        {
            var input = @"
@maxWidth: 600px;
@requirement1: color;
@import url(something.css) screen and (@requirement1) and (max-width: @maxWidth);";

            var expected = @"
@import url(something.css) screen and (color) and (max-width: 600px);";

            AssertLess(input, expected);
        }

        [Test]
        public void LessImportWithMediaSpecificationsConvertedWithVariable()
        {
            var input = @"
@maxWidth: 600px;
@requirement1: color;
@import url(foo.less) screen and (@requirement1) and (max-width: @maxWidth);";

            var expected = @"
@media screen and (color) and (max-width: 600px) {
  body {
    background-color: foo;
  }
}";
            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void LessImportFromEmbeddedResource()
        {
            var input = @"
@import ""dll://dotless.Test.dll/dotless.Test.Embedded.less"";";

            var expected = @"
#import {
  color: red;
}";
            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void CssImportFromEmbeddedResource()
        {
            var input = @"
@import ""dll://dotless.Test.dll/dotless.Test.Embedded.css"";";

            var expected = @"
.windowz .dos {
  border: none;
}";
            var parser = GetParser(false, false, true);

            AssertLess(input, expected, parser);
        }
    }
}