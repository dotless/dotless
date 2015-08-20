using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dotless.CompatibilityTests
{
    public class TestPath
    {
        private const string LessDir = @"test\less\";
        private const string CssDir = @"test\css\";

        public static IEnumerable<TestPath> LoadAll(string projectDir, string differencesDir)
        {
            var fullLessDir = Path.Combine(projectDir, LessDir);
            var fullPaths = System.IO.Directory.EnumerateFiles(fullLessDir, "*.less", SearchOption.AllDirectories);
            var testPaths = fullPaths.Select(p => p.Replace(fullLessDir, "").Replace(".less", ""));

            return testPaths.Select(p => new TestPath(projectDir, p, differencesDir));
        }

        private readonly string _projectDir;
        private readonly string _testPath;
        private readonly string _differencesDir;

        public TestPath(string projectDir, string testPath, string differencesDir)
        {
            _projectDir = projectDir;
            _testPath = testPath;
            _differencesDir = differencesDir;
        }

        public string TestName
        {
            get { return _testPath; }
        }

        public string FileName
        {
            get { return Path.GetFileName(Less); }
        }

        public string Directory
        {
            get { return Path.GetDirectoryName(Less); }
        }

        public string Less
        {
            get { return Path.Combine(_projectDir, LessDir, _testPath + ".less"); }
        }

        public string Css
        {
            get { return Path.Combine(_projectDir, CssDir, _testPath + ".css"); }
        }

        public string Ignore
        {
            get { return _testPath + ".less"; }
        }

        public string DebugLess
        {
            get { return Path.Combine(_differencesDir, _testPath + ".less"); }
        }

        public string ActualCss
        {
            get { return Path.Combine(_differencesDir, _testPath + ".actual.css"); }
        }

        public string ExpectedCss
        {
            get { return Path.Combine(_differencesDir, _testPath + ".expected.css"); }
        }
    }
}