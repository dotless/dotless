using System;
using System.Reflection.Emit;
using System.Security.Policy;

namespace dotless.Core.Test.Specs
{
    using System.Collections.Generic;
    using Core.Importers;
    using Core.Parser;
    using NUnit.Framework;
    using System.IO;
    using System.Reflection;

    class EmbeddedPathResolver : dotless.Core.Input.IPathResolver
    {
        public string GetFullPath(string path)
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), path);
        }
    }

    public class ImportFixture : SpecFixtureBase
    {
        private static Parser GetEmbeddedParser(bool isUrlRewritingDisabled, bool importAllFilesAsLess, bool importCssInline)
        {
            var fileReader = new dotless.Core.Input.FileReader(new EmbeddedPathResolver());

            return new Parser
            {
                Importer = new Importer(fileReader)
                {
                    IsUrlRewritingDisabled = isUrlRewritingDisabled,
                    ImportAllFilesAsLess = importAllFilesAsLess,
                    InlineCssFiles = importCssInline
                }
            };
        }

        private static Parser GetParser()
        {
            return GetParser(false, false, false);
        }

        private static Parser GetParser(bool isUrlRewritingDisabled, bool importAllFilesAsLess, bool importCssInline)
        {
            var imports = new Dictionary<string, string>();

            imports[@"c:/absolute/file.less"] = @"
.windowz .dos {
  border: none;
}
";
            imports[@"import/error.less"] = @"
.windowz .dos {
  border: none;
}
.error_mixin {
  .throw_error();
}
";
            imports[@"import/error2.less"] = @"
.windowz .dos {
  border: none;
}
.error_mixin() {
  .throw_error();
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
            imports["import/twice/with/different/paths.less"] = @"
@import-once ""../twice.less"";
@import-once ""../other.less"";
";

            imports["import/twice/with/other.less"] = @"
@import-once ""twice.less"";
";

            imports["import/twice/with/twice.less"] = @"
body { background-color: foo; }
";

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

            imports["import/define-variables.less"] = @"@color: 'blue';";
            imports["import/use-variables.less"] = @".test { background-color: @color; }";

            imports["empty.less"] = @"";
            imports["rule.less"] = @".rule { color: black; }";

            imports["../import/relative-with-parent-dir.less"] = @"body { background-color: foo; }";

            imports["foo.less"] = @"@import ""foo/bar.less"";";
            imports["foo/bar.less"] = @"@import ""../lib/color.less"";";
            imports["lib/color.less"] = "body { background-color: foo; }";

            imports["247-2.less"] = @"
@color: red;
text {
  color: @color;
}";
            imports["247-1.less"] = @"
#nsTwoCss {
  .css() {
    @import '247-2.less';
  }
}";
            imports["foourl.less"] = @"@import url(""foo/barurl.less"");";
            imports["foo/barurl.less"] = @"@import url(""../lib/colorurl.less"");";
            imports["lib/colorurl.less"] = "body { background-color: foo; }";

            imports["something.css"] = @"body { background-color: foo; invalid ""; }";

            imports["isless.css"] = @"
@a: 9px;
body { margin-right: @a; }";

            imports["vardef.less"] = @"@var: 9px;";

            imports["css-as-less.css"] = @"@var1: 10px;";
            imports["arbitrary-extension-as-less.ext"] = @"@var2: 11px;";

            imports["simple-rule.less"] = ".rule { background-color: black; }";
            imports["simple-rule.css"] = ".rule { background-color: black; }";

            imports["media-scoped-rules.less"] = @"@media (screen) { 
    .rule { background-color: black; }
    .another-rule { color: white; }
}";

            imports["nested-rules.less"] = @"
.parent-selector {
    .rule { background-color: black; }
    .another-rule { color: white; }
}";

            imports["imports-simple-rule.less"] = @"
@import ""simple-rule.less"";
.rule2 { background-color: blue; }";

            imports["two-level-import.less"] = @"
@import ""simple-rule.less"";
.rule3 { background-color: red; }";

            imports["reference/main.less"] = @"
@import ""mixins/test.less"";

.mixin(red);
";

            imports["reference/mixins/test.less"] = @"
.mixin(@arg) {
    .test-ruleset {
        background-color: @arg;
    }
}
";

            imports["reference/ruleset-with-child-ruleset-and-rules.less"] = @"
.parent {
    .child {
        background-color: black;
    }

    background-color: blue;
}
";

            imports["two-level-import.less"] = @"
@import ""simple-rule.less"";
.rule3 { background-color: red; }";

            imports["directives.less"] = @"
@font-face {
  font-family: 'Glyphicons Halflings';
}
";

            imports["mixin-loop.less"] = @"
@grid-columns: 12;
.float-grid-columns(@class) {
  .col(@index) { // initial    
    .col((@index + 1), """");
  }
  .col(@index, @list) when (@index =< @grid-columns) { // general
    .col((@index + 1), """");
  }
  .col(@index, @list) when (@index > @grid-columns) { // terminal

  }
  .col(1); // kickstart it
}

// Create grid for specific class
.make-grid(@class) {
  .float-grid-columns(@class);
}


@media (screen) {
  .make-grid(sm);
}
";

            imports["partial-reference-extends-another-reference.less"] = @"
.parent {
  .test {
    color: black;
  }
}

.ext {
  &:extend(.test all);
}
";

            imports["exact-reference-extends-another-reference.less"] = @"
.test {
  color: black;
}

.ext {
  &:extend(.test);
}
";

            imports["reference-with-multiple-selectors.less"] = @"
.test,
.test2 {
  color: black;
}
";

            imports["comments.less"] = @"
/* This is a comment */
";

            imports["math.less"] = @"
.rule {
    width: calc(10px + 2px);
}
";

            imports["generated-selector.less"] = @"
@selector: ~"".rule"";
@{selector} {
  color: black;
}
";

            imports["multiple-generated-selectors.less"] = @"
@grid-columns: 12;

.float-grid-columns(@class) {
  .col(@index) { // initial
    @item: ~"".col-@{class}-@{index}"";
    .col((@index + 1), @item);
  }
  .col(@index, @list) when (@index =< @grid-columns) { // general
    @item: ~"".col-@{class}-@{index}"";
    .col((@index + 1), ~""@{list}, @{item}"");
  }
  .col(@index, @list) when (@index > @grid-columns) { // terminal
    @{list} {
      float: left;
    }
  }
  .col(1); // kickstart it
}

.float-grid-columns(xs);
";

            imports["import-in-mixin/mixin-definition.less"] = @"
.import() {
  @import ""relative-import.less"";
}
";

            imports["import-in-mixin/relative-import.less"] = @"
.rule {
  color: black;
}
";

            imports["reference-mixin-issue.less"] = @"
.mix-me
{
    color: red;
    @media (min-width: 100px)
    {
        color: blue;
    }
    .mix-me-child
    {
        background-color: black;
    }
}";

            imports["nested-import-interpolation-1.less"] = @"
@var: ""2"";
@import ""nested-import-interpolation-@{var}.less"";
";

            imports["nested-import-interpolation-2.less"] = @"
body { background-color: blue; }
";

            return new Parser { 
                Importer = new Importer(new DictionaryReader(imports)) { 
                    IsUrlRewritingDisabled = isUrlRewritingDisabled,
                    ImportAllFilesAsLess = importAllFilesAsLess,
                    InlineCssFiles = importCssInline} };
        }

        [Test]
        public void Test247()
        {
            var input = @"
@import '247-1.less';
#nsTwo {
  #nsTwoCss > .css();
}";
            var expected = @"
#nsTwo text {
  color: red;
}";
            var parser = GetParser();

            AssertLess(input, expected, parser);
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
  color: #ff0000;
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
            Assert.AreEqual(1, dictionaryReader.DoesFileExistCalls.Count, "We should not ask the file reader if a protocol file exists");
            Assert.AreEqual(1, dictionaryReader.GetFileContentsCalls.Count, "We should not ask the file reader if a protocol file exists");

            Assert.AreEqual(@"import/other-protocol-test.less", dictionaryReader.DoesFileExistCalls[0], "We should not ask the file reader if a protocol file exists");
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
        public void ErrorInImport()
        {
            var input = @"
@import ""import/error.less"";";

            var parser = GetParser();

            AssertError(@"
.throw_error is undefined on line 6 in file 'import/error.less':
  [5]: .error_mixin {
  [6]:   .throw_error();
       --^
  [7]: }", input, parser);
        }

        [Test]
        public void ErrorInImport2()
        {
            var input = @"
@import ""import/error2.less"";
.a {
  .error_mixin();
}";

            var parser = GetParser();

            AssertError(@"
.throw_error is undefined on line 6 in file 'import/error2.less':
  [5]: .error_mixin() {
  [6]:   .throw_error();
       --^
  [7]: }
from line 3 in file 'test.less':
  [3]:   .error_mixin();", input, parser);
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

            AssertError(".less cannot import non local less files [http://www.someone.com/external1.less].", input, parser);
        }

        [Test]
        public void ImportForMissingLessFileThrowsError2()
        {
            var input = @"@import ""external1.less"";";

            var parser = GetParser();

            AssertError("You are importing a file ending in .less that cannot be found [external1.less].", input, parser);
        }

        [Test]
        public void ImportForMissingLessFileThrowsError3()
        {
            var input = @"@import ""http://www.someone.com/external1.less"";";

            var parser = GetParser();

            AssertError(".less cannot import non local less files [http://www.someone.com/external1.less].", input, parser);
        }

        [Test]
        public void ImportForMissingLessFileThrowsError4()
        {
            var input = @"@import ""dll://someassembly#missing.less"";";

            var parser = GetParser();

            AssertError("Unable to load resource [missing.less] in assembly [someassembly]", input, parser);
        }

        [Test]
        public void ImportForMissingCssFileAsLessThrowsError()
        {
            var input = @"@import ""dll://someassembly#missing.css"";";

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
        public void LessImportWithMediaSpecificationsConvertedWithOnce()
        {
            var input = @"
@import-once url(foo.less) screen and (color) and (max-width: 600px);";

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
@import ""dll://dotless.Core.Test.dll#dotless.Core.Test.Resource.Embedded.less"";
@import ""dll://dotless.Core.Test.dll#dotless.Core.Test.Resource.Embedded2.less"";";

            var expected = @"
#import {
  color: red;
}
#import {
  color: blue;
}";
            var parser = GetEmbeddedParser(false, false, false);

            AssertLess(input, expected, parser);
        }

        //[Test]
        //public void LessImportFromEmbeddedResourceWithDynamicAssembliesInAppDomain() {
        //    Assembly assembly =  AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("dotless.dynamic.dll"),
        //        AssemblyBuilderAccess.RunAndSave);

        //    Assembly.Load(new AssemblyName("dotless.Core.Test.EmbeddedResource"));

        //    try {
        //        Evaluate(@"@import ""dll://dotless.Core.Test.EmbeddedResource.dll#dotless.Core.Test.EmbeddedResource.Embedded.less"";",
        //            GetEmbeddedParser(false, false, false));
        //    } catch (FileNotFoundException ex) {
        //        // If the import fails for the wrong reason (i.e., having the dynamic assembly loaded),
        //        // the failure will have a different exception message
        //        Assert.That(ex.Message, Is.EqualTo("You are importing a file ending in .less that cannot be found [nosuch.resource.less]."));
        //    }
        //}

        [Test]
        public void CssImportFromEmbeddedResource()
        {
            var input = @"
@import ""dll://dotless.Core.Test.dll#dotless.Core.Test.Resource.Embedded.css"";";

            var expected = @"
.windowz .dos {
  border: none;
}";
            var parser = GetEmbeddedParser(false, false, true);

            AssertLess(input, expected, parser);
        }

        [Test]
        public void ImportTwiceImportsOnce()
        {
            var input = @"
@import ""lib/color.less"";
@import ""lib/color.less"";";

            var expected = @"
body {
  background-color: foo;
}
";
            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void ImportOnceTwiceImportsOnce()
        {
            var input = @"
@import-once ""lib/color.less"";
@import-once ""lib/color.less"";";

            var expected = @"
body {
  background-color: foo;
}
";
            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void ImportTwiceWithDifferentRelativePathsImportsOnce()
        {
            var input = @"
@import-once ""import/twice/with/different/paths.less"";";

            var expected = @"
body {
  background-color: foo;
}
";
            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void VariablesFromImportedFileAreAvailableToAnotherImportedFileWithinMediaBlock()
        {
            var input = @"
@import ""import/define-variables.less"";

@media only screen and (max-width: 700px)
{
    @import ""import/use-variables.less"";
}
";

            var expected = @"
@media only screen and (max-width: 700px) {
  .test {
    background-color: 'blue';
  }
}";

            AssertLess(input, expected, GetParser());
        }

        [Test]
        public void EmptyImportDoesNotBreakSubsequentImports()
        {
            var input = @"
@import ""empty.less"";
@import ""rule.less"";

.test {
  .rule;
}
";

            var expected = @"
.rule {
  color: black;
}
.test {
  color: #000000;
}
";
            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void ExtendingNestedRulesFromReferenceImportsWorks()
        {
            var input = @"
@import (reference) ""nested-rules.less"";

.test:extend(.rule all) { }
";

            var expected = @"
.parent-selector .test {
  background-color: black;
}
";
            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void ExtendingNestedReferenceRulesIgnoresRulesFromParentRuleset() {
            var input = @"
@import (reference) ""reference/ruleset-with-child-ruleset-and-rules.less"";

.test:extend(.child all) { }
";

            var expected = @"
.parent .test {
  background-color: black;
}
";

            AssertLess(input, expected, GetParser());
        }

//        [Test]
//        public void VariableInterpolationInQuotedCssImport()
//        {
//            var input =
//                @"
//@var: ""foo"";

//@import ""@{var}/bar.css"";
//";

//            var expected =
//                @"
//@import ""foo/bar.css"";
//";

//        }

        [Test]
        public void VariableInterpolationInQuotedLessImport()
        {
            var input =
                @"
@component: ""color"";

@import ""lib/@{component}.less"";
";

            var expected =
                @"
body {
  background-color: foo;
}";

            AssertLess(input, expected, GetParser());
        }

        [Test]
        public void VariableInterpolationInNestedLessImport()
        {
            var input =
                @"
@import ""nested-import-interpolation-1.less"";
";

            var expected =
                @"
body {
  background-color: blue;
}";

            AssertLess(input, expected, GetParser());
        }

        [Test]
        public void ImportMultipleImportsMoreThanOnce()
        {
            var input = @"
@import ""vardef.less"";
@var: 10px;
@import (multiple) ""vardef.less"";
.rule { width: @var; }
";

            var expected = @"
.rule {
  width: 9px;
}
";
            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void ImportOptionalIgnoresFilesThatAreNotFound()
        {
            var input = @"
@var: 10px;
@import (optional) ""this-file-does-not-exist.less"";
.rule { width: @var; }
";

            var expected = @"
.rule {
  width: 10px;
}
";
            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void ImportCssGeneratesImportDirective()
        {
            var input = @"
@import (css) ""this-file-does-not-exist.less"";
";

            var expected = @"
@import ""this-file-does-not-exist.less"";
";
            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void ImportLessParsesAnyExtensionAsLess()
        {
            var input = @"
@import (less) ""css-as-less.css"";
@import (less) ""arbitrary-extension-as-less.ext"";

.rule {
  width: @var1;
  height: @var2;
}
";

            var expected = @"
.rule {
  width: 10px;
  height: 11px;
}";
            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void ImportInlineIncludesContentsOfCssFile()
        {
            var input = @"
@import (inline) ""something.css"";
";

            var expected = @"
body { background-color: foo; invalid ""; }
";
            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void ImportReferenceAloneDoesNotProduceOutput()
        {
            var input = @"
@import (reference) ""simple-rule.less"";
";

            var expected = @"";
            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void ImportReferenceDoesNotOutputMediaBlocks()
        {
            var input = @"
@import (reference) ""media-scoped-rules.less"";
";

            var expected = @"";
            var parser = GetParser();

            AssertLess(input, expected, parser);
        }


        [Test]
        public void ImportReferenceDoesNotOutputRulesetsThatCallLoopingMixins()
        {
            var input = @"
@import (reference) ""mixin-loop.less"";
";

            var expected = @"";
            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void PartialReferenceExtenderDoesNotCauseReferenceRulesetToBeOutput()
        {
            var input = @"
@import (reference) ""partial-reference-extends-another-reference.less"";
";

            AssertLess(input, @"", GetParser());
        }

        [Test]
        public void ExactReferenceExtenderDoesNotCauseReferenceRulesetToBeOutput()
        {
            var input = @"
@import (reference) ""exact-reference-extends-another-reference.less"";
";

            AssertLess(input, @"", GetParser());
        }

        [Test]
        public void ImportReferenceDoesNotOutputDirectives()
        {
            var input = @"
@import (reference) ""directives.less"";
";

            AssertLess(input, @"", GetParser());
        }

        [Test]
        public void ImportReferenceOutputsExtendedRulesFromMediaBlocks()
        {
            var input = @"
@import (reference) ""media-scoped-rules.less"";

.test:extend(.rule all) { }
";

            var expected = @"
@media (screen) {
  .test {
    background-color: black;
  }
}
";

            AssertLess(input, expected, GetParser());
        }

        [Test]
        public void ImportReferenceDoesNotOutputMixinCalls()
        {
            var input = @"
@import (reference) ""reference/main.less"";
";

            AssertLess(input, @"", GetParser());
        }

        [Test]
        public void ExtendingReferencedImportOnlyOutputsExtendedSelector()
        {
            var input = @"
@import (reference) ""reference-with-multiple-selectors.less"";

.ext:extend(.test all) { }
";

            var expected = @"
.ext {
  color: black;
}";

            AssertLess(input, expected, GetParser());
        }

        [Test]
        public void ImportReferenceDoesNotOutputComments()
        {
            var input = @"
@import (reference) ""comments.less"";
";

            AssertLess(input, @"", GetParser());
        }

        [Test]
        public void ImportReferenceWithMixinCallProducesOutput()
        {
            var input = @"
@import (reference) ""simple-rule.less"";

.caller {
  .rule
}
";

            var expected = @"
.caller {
  background-color: #000000;
}
";
            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void ImportReferenceDoesNotPreventNonReferenceImport()
        {
            var input = @"
@import (reference) ""simple-rule.less"";
@import  ""simple-rule.less"";
";

            var expected = @"
.rule {
  background-color: black;
}
";
            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void ExtendingReferenceImportsWorks()
        {
            var input = @"
@import (reference) ""simple-rule.less"";
.test:extend(.rule all) { }
";

            var expected = @"
.test {
  background-color: black;
}
";
            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void ImportsFromReferenceImportsAreTreatedAsReferences()
        {
            var input = @"
@import (reference) ""imports-simple-rule.less"";

.test {
  .rule2;
}
";

            var expected = @"
.test {
  background-color: #0000ff;
}
";
            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void RecursiveImportsFromReferenceImportsAreTreatedAsReferences()
        {
            var input = @"
@import (reference) ""two-level-import.less"";

.test {
  background-color: blue;
}
";

            var expected = @"
.test {
  background-color: blue;
}
";
            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void ImportingReferenceAsLessWorks()
        {
            var input = @"
@import (reference, less) ""simple-rule.css"";
.test {
  .rule
}
";

            var expected = @"
.test {
  background-color: #000000;
}
";
            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void ImportingReferenceAsCssFails()
        {
            var input = @"
@import (reference, css) ""simple-rule.css"";
";

            var expectedError = @"
invalid combination of @import options (reference, css) -- specify either reference or css, but not both on line 1 in file 'test.less':
   []: /beginning of file
  [1]: @import (reference, css) ""simple-rule.css"";
       --------^
  [2]: /end of file";
            AssertError(expectedError, input);
        }

        [Test]
        public void ImportingAsBothCssAndLessFails()
        {
            var input = @"
@import (css, less) ""simple-rule.css"";
";

            var expectedError = @"
invalid combination of @import options (css, less) -- specify either css or less, but not both on line 1 in file 'test.less':
   []: /beginning of file
  [1]: @import (css, less) ""simple-rule.css"";
       --------^
  [2]: /end of file";
            AssertError(expectedError, input);
        }

        [Test]
        public void ImportingAsBothInlineAndReferenceFails()
        {
            var input = @"
@import (inline, reference) ""simple-rule.css"";
";

            var expectedError = @"
invalid combination of @import options (inline, reference) -- specify either inline or reference, but not both on line 1 in file 'test.less':
   []: /beginning of file
  [1]: @import (inline, reference) ""simple-rule.css"";
       --------^
  [2]: /end of file";
            AssertError(expectedError, input);
        }

        [Test]
        public void ImportingAsBothInlineAndCssFails()
        {
            var input = @"
@import (inline, css) ""simple-rule.css"";
";

            var expectedError = @"
invalid combination of @import options (inline, css) -- specify either inline or css, but not both on line 1 in file 'test.less':
   []: /beginning of file
  [1]: @import (inline, css) ""simple-rule.css"";
       --------^
  [2]: /end of file";
            AssertError(expectedError, input);
        }

        [Test]
        public void ImportingAsBothInlineAndLessFails()
        {
            var input = @"
@import (inline, less) ""simple-rule.css"";
";

            var expectedError = @"
invalid combination of @import options (inline, less) -- specify either inline or less, but not both on line 1 in file 'test.less':
   []: /beginning of file
  [1]: @import (inline, less) ""simple-rule.css"";
       --------^
  [2]: /end of file";
            AssertError(expectedError, input);
        }

        [Test]
        public void ImportingAsBothOnceAndMultipleFails()
        {
            var input = @"
@import (once, multiple) ""simple-rule.css"";
";

            var expectedError = @"
invalid combination of @import options (once, multiple) -- specify either once or multiple, but not both on line 1 in file 'test.less':
   []: /beginning of file
  [1]: @import (once, multiple) ""simple-rule.css"";
       --------^
  [2]: /end of file";
            AssertError(expectedError, input);
        }

        [Test]
        public void UnrecognizedImportOptionFails()
        {
            var input = @"
@import (invalid-option) ""simple-rule.css"";
";

            var expectedError = @"
unrecognized @import option 'invalid-option' on line 1 in file 'test.less':
   []: /beginning of file
  [1]: @import (invalid-option) ""simple-rule.css"";
       --------^
  [2]: /end of file";
            AssertError(expectedError, input);
        }
		

        [Test]
        public void ImportProtocolCssInsideMixinsWithNestedGuards()
        {
            var input = @"
.generateImports(@fontFamily) {
  & when (@fontFamily = Lato) {
    @import url(https://fonts.googleapis.com/css?family=Lato);
  }
  & when (@fontFamily = Cabin) {
    @import url(https://fonts.googleapis.com/css?family=Cabin);
  }
}
.generateImports(Lato);
.generateImports(Cabin);
";
            
            var expected = @"
@import url(https://fonts.googleapis.com/css?family=Lato);


@import url(https://fonts.googleapis.com/css?family=Cabin);
";
            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void ImportProtocolCssInsideMixinsWithGuards()
        {
            var input = @"
.generateImports(@fontFamily) when (@fontFamily = Lato) {
  @import url(https://fonts.googleapis.com/css?family=Lato);
}
.generateImports(@fontFamily) when (@fontFamily = Cabin) {
  @import url(https://fonts.googleapis.com/css?family=Cabin);
}
.generateImports(Lato);
.generateImports(Cabin);
";

            var expected = @"
@import url(https://fonts.googleapis.com/css?family=Lato);
@import url(https://fonts.googleapis.com/css?family=Cabin);
";
            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void StrictMathIsHonoredInImports()
        {
            var input = @"
@import ""math.less"";
";

            var expected = @"
.rule {
  width: calc(10px + 2px);
}
";
            var parser = GetParser();
            parser.StrictMath = true;

            AssertLess(input, expected, parser);
        }

        [Test]
        public void ImportsWithinRulesets()
        {
            var input = @"
.test {
  @import ""math.less"";
}
";

            var expected = @"
.test .rule {
  width: calc(12px);
}
";
            AssertLess(input, expected, GetParser());
        }

        [Test]
        public void ImportsWithGeneratedSelectorsWithinRulesets()
        {
            var input = @"
.namespace {
  @import ""generated-selector.less"";
}
";

            var expected = @"
.namespace .rule {
  color: black;
}
";
            AssertLess(input, expected, GetParser());
        }

        [Test]
        public void NestedImportsWithinRulesets()
        {
            var input =
                @"
.namespace {
  @import url(""import/import-test-a.less"");

  #import-test {
    .mixin;
    width: 10px;
    height: @a + 10%;
  }
}
";

            var expected =
                @"
@import ""import-test-d.css"";
.namespace #import {
  color: red;
}
.namespace .mixin {
  height: 10px;
  color: red;
}
.namespace #import-test {
  height: 10px;
  color: #ff0000;
  width: 10px;
  height: 30%;
}
";

            var parser = GetParser();

            AssertLess(input, expected, parser);
        }

        [Test]
        public void ImportsWithinRulesetsGenerateCallableMixins() {

            var input = @"
.namespace {
  @import ""reference/mixins/test.less"";
  .mixin(red);
}";

            var expected = @"
.namespace .test-ruleset {
  background-color: red;
}";

            AssertLess(input, expected, GetParser());
        }

        [Test]
        public void ExtendedReferenceImportWithMultipleGeneratedSelectorsOnlyOutputsExtendedSelectors() {

            var input = @"
@import (reference) ""multiple-generated-selectors.less"";

.test:extend(.col-xs-12 all) { }
";

            var expected = @"
.test {
  float: left;
}";

            AssertLess(input, expected, GetParser());
        }

        [Test]
        public void RelativeImportsHonorCurrentDirectory() {
            var input = @"
@import ""import-test-a.less"";
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
}";

            var parser = GetParser();
            parser.CurrentDirectory = "import";
            AssertLess(input, expected, parser);
        }

        [Test]
        public void AbsolutePathImportsHonorsCurrentDirectory()
        {
            var input = @"
@import 'c:/absolute/file.less';";
            var expected = @"
.windowz .dos {
  border: none;
}
";
            var parser = GetParser();
            parser.CurrentDirectory = "import";

            AssertLess(input, expected, parser);
        }

        [Test]
        public void CssImportsAreHoistedToBeginningOfFile() {
            var input = @"
@font-face {
  font-family: ""Epsilon"";
  src: url('data:font/x-woff;base64,...')
}
@import url(//fonts.googleapis.com/css?family=PT+Sans+Narrow:400,700&subset=latin,cyrillic);
";


            var expected =
                @"
@import url(//fonts.googleapis.com/css?family=PT+Sans+Narrow:400,700&subset=latin,cyrillic);
@font-face {
  font-family: ""Epsilon"";
  src: url('data:font/x-woff;base64,...');
}";

            AssertLess(input, expected);
        }

        [Test]
        public void RelativeImportInMixinDefinition() {
            var input = @"
@import ""import-in-mixin/mixin-definition.less"";
.import();
";

            var expected = @"
.rule {
  color: black;
}";

            AssertLess(input, expected, GetParser());
        }

        [Test]
        public void ReferenceImportDoesNotOutputUnreferencedStyles() {
            var input = @"
@import (reference) ""reference-mixin-issue.less"";

/* Styles */
.my-class
{
    .mix-me();
}";

            var expected = @"
/* Styles */

.my-class {
  color: #ff0000;
}
@media (min-width: 100px) {
  .my-class {
    color: blue;
  }
}
.my-class .mix-me-child {
  background-color: black;
}";

            AssertLess(input, expected, GetParser());
        }

        [Test]
        public void MixinWithMediaBlock() {
            var input = @"
.mixin() {
  @media (min-width: 100px) {
    color: blue;
  }
}

.test {
  .mixin();
}
";

            var expected = @"
@media (min-width: 100px) {
  .test {
    color: blue;
  }
}";

            AssertLess(input, expected, GetParser());
        }
    }
}