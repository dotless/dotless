using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using dotless.Core;
using NUnit.Framework;

namespace dotless.CompatibilityTests
{
    [TestFixture]
    public class LessJsCompatiblity
    {
        // TODO(yln): this should be configurable with sensible defaults, how?
        const string LessJsProjectDir = @"..\..\..\..\..\less.js\";
        const string TestDebugDir = "test-debug";

        static readonly string LessJsTestDir = Path.Combine(LessJsProjectDir, @"test\less\");

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            try
            {
                Directory.Delete(TestDebugDir, recursive: true);
            }
            catch (DirectoryNotFoundException) { /* That's okay! */ }
        }

        [Test, TestCaseSource("LoadTestCases")]
        public void TestCompatiblity(string lessPath, string cssPaths)
        {
            var less = File.ReadAllText(lessPath);
            var expectedCss = File.ReadAllText(cssPaths);

            var engine = new EngineFactory().GetEngine();
            engine.CurrentDirectory = Path.GetDirectoryName(lessPath);

            var lessFileName = Path.GetFileName(lessPath);
            var css = engine.TransformToCss(less, lessFileName);

            if (CompareOutput(css, expectedCss) != 0)
            {
                var testPath = GetNicePath(lessPath);
                Dump(testPath + ".actual", css);
                Dump(testPath + ".expected", expectedCss);
            }

            Assert.That(css, Is.EqualTo(expectedCss).Using<string>(CompareOutput));
        }

        private int CompareOutput(string actual, string expected)
        {
            // TODO(yln): compare this more elegantly, e.g., ignore formatting?
            // Do we want to reach formatting compatibility?
            return string.Compare(actual, expected, StringComparison.Ordinal);
        }

        private IEnumerable<ITestCaseData> LoadTestCases()
        {
            var lessPaths = Directory.EnumerateFiles(LessJsTestDir, "*.less", SearchOption.AllDirectories);
            var ignores = LoadIgnores("ignore.txt");

            return lessPaths.Select(file => CreateTestCase(file, ignores));
        }

        private ITestCaseData CreateTestCase(string lessPath, IDictionary<string, string> ignores)
        {
            var tmp = lessPath.Replace(@"\test\less\", @"\test\css\");
            var cssPath = Path.ChangeExtension(tmp, ".css");

            var testCase = new TestCaseData(lessPath, cssPath);
            testCase.SetName(GetNicePath(lessPath));

            if (ignores.ContainsKey(lessPath))
            {
                testCase.Ignore(ignores[lessPath]);
            }

            return testCase;
        }

        private string GetNicePath(string fullLessPath)
        {
            return fullLessPath.Replace(LessJsTestDir, "").Replace(".less", "");
        }

        private void Dump(string testPath, string content)
        {
            var path = Path.Combine(TestDebugDir, testPath);
            var directory = Path.GetDirectoryName(path);
            Directory.CreateDirectory(directory);
            File.WriteAllText(path, content);
        }

        private IDictionary<string, string> LoadIgnores(string ignoreFile)
        {
            var ignores = new Dictionary<string, string>();
            foreach (var line in File.ReadLines(ignoreFile))
            {
                var parts = line.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0) continue;

                var file = parts[0].Trim();
                if (file.Length == 0) continue;
                var reason = parts.Length > 1 ? parts[1] : null;

                ignores.Add(file, reason);
            }
            return ignores;
        }
    }
}
