using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using dotless.Core;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Reflection;
using System.Net;
using System.IO.Compression;

namespace dotless.CompatibilityTests
{
    [TestFixture]
    public class LessJsCompatiblity
    {
        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            DeleteDifferencesDirectory();
        }

        [Test, TestCaseSource("LoadTestCases")]
        public void TestCompatiblity(TestPath path)
        {
            var less = File.ReadAllText(path.Less);
            var expectedCss = File.ReadAllText(path.Css);

            var css = Transform(less, path);

            if (CompareOutput(css, expectedCss) != 0)
            {
                Dump(path.DebugLess, less);
                Dump(path.ActualCss, css);
                Dump(path.ExpectedCss, expectedCss);
            }

            Assert.That(css, Is.EqualTo(expectedCss).Using<string>( new Comparison<string>(CompareOutput)));
        }

        private int CompareOutput(string actual, string expected)
        {
            // TODO(yln): compare this more elegantly, e.g., ignore formatting?
            // Do we want to reach formatting compatibility?
            return string.Compare(actual, expected, StringComparison.Ordinal);
        }

        private string Transform(string less, TestPath path)
        {
            var engine = new EngineFactory().GetEngine();
            engine.CurrentDirectory = path.Directory;

            return engine.TransformToCss(less, path.FileName);
        }

        private static IEnumerable<ITestCaseData> LoadTestCases()
        {
            var currentFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var projectDir = GetLessJsArchiveDirectory(currentFolder);

            var differencesDir = ConfigurationManager.AppSettings["differencesDirectory"];
            var ignoreFile = ConfigurationManager.AppSettings["ignoreFile"];

            if (!Path.IsPathRooted(projectDir))
                projectDir = Path.GetFullPath(projectDir);

            if (!Path.IsPathRooted(ignoreFile))
                ignoreFile = Path.Combine(currentFolder, ignoreFile);

            if (!Path.IsPathRooted(differencesDir))
                differencesDir = Path.Combine(currentFolder, differencesDir);

            var testPaths = TestPath.LoadAll(projectDir, differencesDir);
            var ignores = Ignore.Load(ignoreFile);

            return testPaths.Select(t => CreateTestCase(t, ignores));
        }

        private static string GetLessJsArchiveDirectory(string currentFolder)
        {
            var rootFolder = Path.Combine(currentFolder, "less.js");
            var archiveUri = ConfigurationManager.AppSettings["lessJsArchive"];
            var archiveFileName = Path.GetFileName(archiveUri);
            var archiveName = Path.GetFileNameWithoutExtension(archiveUri);
            var lessJsArchiveFile = Path.Combine(rootFolder, archiveFileName);
            var lessJsArchiveFolder = Path.Combine(rootFolder, archiveName);

            if (!Directory.Exists(rootFolder))
                Directory.CreateDirectory(rootFolder);

            if (!File.Exists(lessJsArchiveFile))
            {
                var client = new WebClient();
                client.UseDefaultCredentials = true;

                IWebProxy defaultProxy = WebRequest.DefaultWebProxy;
                if (defaultProxy != null)
                {
                    defaultProxy.Credentials = CredentialCache.DefaultCredentials;
                    client.Proxy = defaultProxy;
                }
             
                client.DownloadFile(archiveUri, lessJsArchiveFile);
            }

            if (!Directory.Exists(lessJsArchiveFolder))
            {
                ZipFile.ExtractToDirectory(lessJsArchiveFile, lessJsArchiveFolder);
            }

            // return first folder in the extracted folder
            var directories = Directory.GetDirectories(lessJsArchiveFolder);

            if (directories.Length == 1)
                return directories[0];

            return lessJsArchiveFolder;
        }

        private static ITestCaseData CreateTestCase(TestPath path, IDictionary<string, string> ignores)
        {
            var testCase = new TestCaseData(path).SetName(path.TestName);

            if (ignores.ContainsKey(path.Ignore))
            {
                testCase.Ignore(ignores[path.Ignore] ?? "From ignore.txt");
            }

            return testCase;
        }

        private void Dump(string path, string content)
        {
            var dir = Path.GetDirectoryName(path);
            Directory.CreateDirectory(dir);
            File.WriteAllText(path, content);
        }

        private void DeleteDifferencesDirectory()
        {
            var dir = ConfigurationManager.AppSettings["differencesDirectory"];
            try
            {
                Directory.Delete(dir, recursive: true);
            }
            catch (DirectoryNotFoundException)
            {
                // That's okay!
            }
        }
    }
}
