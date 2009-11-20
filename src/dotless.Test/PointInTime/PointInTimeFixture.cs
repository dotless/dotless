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
        public void Parse_Test_Data_And_Output_Tree()
        {
            var engine = new Engine(File.ReadAllText(@"PointInTime/TestData.less"), Console.Out);
            Console.Write(engine.Parse(true).Css);
        }
        [Test]
        public void Parse_Test_Data()
        {
            var engine = new Engine(File.ReadAllText(@"PointInTime/TestData.less"), Console.Out);
            Console.Write(engine.Parse().Css);
        }
        [Test]
        public void AltEngine_Parse_Test_Data()
        {
            var engine = new AltEngine(File.ReadAllText(@"PointInTime/TestData.less"));
            Console.Write(engine.Css);
        }
    }
}