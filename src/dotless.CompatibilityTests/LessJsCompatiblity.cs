﻿using System;
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
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            DeleteDirectory(TestPath.DifferencesDir);
        }

        [Test, TestCaseSource("LoadTestCases")]
        public void TestCompatiblity(TestPath path)
        {
            var css = Transform(path);
            var expectedCss = File.ReadAllText(path.Css);

            if (CompareOutput(css, expectedCss) != 0)
            {
                Dump(path.ActualCss, css);
                Dump(path.ExpectedCss, expectedCss);
            }

            Assert.That(css, Is.EqualTo(expectedCss).Using<string>(CompareOutput));
        }

        private int CompareOutput(string actual, string expected)
        {
            // TODO(yln): compare this more elegantly, e.g., ignore formatting?
            // Do we want to reach formatting compatibility?
            return string.Compare(actual, expected, StringComparison.Ordinal);
        }

        private string Transform(TestPath path)
        {
            var engine = new EngineFactory().GetEngine();
            engine.CurrentDirectory = path.Directory;

            var input = File.ReadAllText(path.Less);

            return engine.TransformToCss(input, path.FileName);
        }

        private IEnumerable<ITestCaseData> LoadTestCases()
        {
            var testPaths = TestPath.LoadAll();
            var ignores = Ignore.Load("ignore.txt");

            return testPaths.Select(t => CreateTestCase(t, ignores));
        }

        private ITestCaseData CreateTestCase(TestPath path, IDictionary<string, string> ignores)
        {
            var testCase = new TestCaseData(path).SetName(path.TestName);

            if (ignores.ContainsKey(path.Ignore))
            {
                testCase.Ignore(ignores[path.Ignore]);
            }

            return testCase;
        }

        private void Dump(string path, string content)
        {
            var directory = Path.GetDirectoryName(path);
            Directory.CreateDirectory(directory);
            File.WriteAllText(path, content);
        }

        private static void DeleteDirectory(string directory)
        {
            try
            {
                Directory.Delete(directory, recursive: true);
            }
            catch (DirectoryNotFoundException)
            {
                // That's okay!
            }
        }
    }
}