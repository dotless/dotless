/* Copyright 2009 dotless project, http://www.dotlesscss.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *     
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License. */

namespace dotless.Test.Unit.ConsoleRunner
{
    using System.IO;
    using Compiler;
    using NUnit.Framework;
    [TestFixture]
    public class CompilerFixture
    {
        [Test]
        public void TransformsFileCorrectly()
        {
            string inputFile = @"Unit\ConsoleRunner\variables.less";
            string outputFile = inputFile + ".css";
            RemoveIfFileExists(outputFile);
            string[] args = { inputFile };

            var writer = new StringWriter();
            System.Console.SetOut(writer);

            Program.Main(args);

            Assert.True(File.Exists(outputFile));

            var consoleOutput = writer.ToString();

            Assert.That(consoleOutput.Trim(), Is.StringEnding("[Done]"));
        }

        private void RemoveIfFileExists(string outputFile)
        {
            if (File.Exists(outputFile))
                File.Delete(outputFile);
        }

        [Test]
        public void CorrectlyHandlesImportWorkingDirectories()
        {
            string inputFile = @"Unit\ConsoleRunner\import.less";
            string outputFile = inputFile + ".css";
            RemoveIfFileExists(outputFile);
            string[] args = {inputFile};
            var writer = new StringWriter();
            System.Console.SetOut(writer);

            Program.Main(args);
        }
    }
}