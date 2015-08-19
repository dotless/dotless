using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dotless.CompatibilityTests
{
    public class TestPath
    {
        public const string DifferencesDir = "differences";

        // TODO(yln): Less.js project dir should be configurable with a sensible defaults?
        private const string ProjectDir = @"..\..\..\..\..\less.js\";
        private const string LessDir = ProjectDir + @"test\less\";
        private const string CssDir = ProjectDir + @"test\css\";

        public static IEnumerable<TestPath> LoadAll()
        {
            var fullPaths = System.IO.Directory.EnumerateFiles(LessDir, "*.less", SearchOption.AllDirectories);
            var paths = fullPaths.Select(p => p.Replace(LessDir, "").Replace(".less", ""));
            return paths.Select(p => new TestPath(p));
        }

        private readonly string _path;

        public TestPath(string path)
        {
            _path = path;
        }

        public string TestName
        {
            get { return _path; }
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
            get { return Path.Combine(LessDir, _path + ".less"); }
        }

        public string Css
        {
            get { return Path.Combine(CssDir, _path + ".css"); }
        }

        public string Ignore
        {
            get { return _path + ".less"; }
        }

        public string ActualCss
        {
            get { return Path.Combine(DifferencesDir, _path + ".actual"); }
        }

        public string ExpectedCss
        {
            get { return Path.Combine(DifferencesDir, _path + ".expected"); }
        }
    }
}