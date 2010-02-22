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

using System.Diagnostics;
using System.Linq;

namespace dotless.Test.PointInTime
{
    using System;
    using System.IO;
    using Core.engine;
    using NUnit.Framework;

    /// <summary>
    /// NOTE: This test is nonsense, just a quick  point in time proof
    /// </summary>
    [TestFixture]
    public class PointInTimeFixture
    {
        [Test]
        public void Expression_Eval_Benchmark()
        {
            var query = "@a: 10px;";
            var rules = Enumerable.Range(0, 10).Select(x => string.Format("class{0} {{ width: @a / 2 + 4; }}", x));
            query += string.Join(Environment.NewLine, rules.ToArray());
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var engine = new ExtensibleEngineImpl(query);
            //Console.WriteLine("Time elapsed: " + stopwatch.ElapsedMilliseconds);
            //Console.WriteLine(engine.Css);
            
        }
        [Test]
        public void AltEngine_Parse_Test_Data()
        {
            //PipelineFactory.LessParser = new LessTreePrinterParser();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var engine = new ExtensibleEngineImpl(File.ReadAllText(@"PointInTime/TestData.less"));
            //Console.WriteLine("Time elapsed: " + stopwatch.ElapsedMilliseconds);
            //Console.WriteLine(engine.Css);
        }
    }
}