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

using System;
using System.IO;
using System.Linq;
using dotless.Core;
using dotless.Core.Parser;

namespace dotlessjs.Compiler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(@"C:\dev\oss\less.js\test\less");

            var files = new[]
                            {
                                "colors",
                                "comments",
                                "css",
                                "css-3",
                                "mixins",
                                "mixins-args",
                                "operations",
                                "parens",
                                "rulesets",
                                "scope",
                                "selectors",
                                "strings",
                                "variables",
                                "whitespace"
                            }
                .Select(f => f + ".less");

            var contents = files
                .ToDictionary(f => f, f => new LessSourceObject {Content = File.ReadAllText(f)});

            const int rounds = 150;

            Func<string, ILessEngine, int> runTest =
                (file, engine) => Enumerable
                                      .Range(0, rounds)
                                      .Select(i =>
                                                  {
                                                      var starttime = DateTime.Now;
                                                      engine.TransformToCss(contents[file]);
                                                      var duration = (DateTime.Now - starttime);
                                                      return duration.Milliseconds;
                                                  })
                                      .Sum();

            Console.WriteLine("Press Enter to begin benchmark");
            Console.ReadLine();

            Console.WriteLine("Running each test {0} times\n", rounds);

            var engines = new ILessEngine[]
                              {
                                  new LessEngine()
                              };


            Console.Write("       File        |     Size     |");
            foreach (var engine in engines)
            {
                Console.Write(engine.GetType().Name.PadRight(24).PadLeft(29));
                Console.Write('|');
            }
            Console.WriteLine();
            Console.WriteLine(new string('-', 35 + 30*engines.Length));

            foreach (var file in files)
            {
                var size = rounds*contents[file].Content.Length/1024d;
                Console.Write("{0} | {1,8:#,##0.00} Kb  | ", file.PadRight(18), size);

                foreach (var engine in engines)
                {
                    try
                    {
                        var time = runTest(file, engine)/1000d;
                        Console.Write("{0,8:#.00} s  {1,10:#,##0.00} Kb/s | ", time, size/time);
                    }
                    catch
                    {
                        Console.Write("Failied                     | ");
                    }
                }
                Console.WriteLine();
            }

            //      Console.Read();
        }
    }
}