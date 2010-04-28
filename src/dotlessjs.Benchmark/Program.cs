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
using dotless;

namespace dotlessjs.Compiler
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var parser = new Parser();

      Directory.SetCurrentDirectory(@"C:\dev\oss\less.js\test\less");

      var files = new[]
                    {
                      "colors",
                      "comments",
                      "css",
                      "css-3",
                      "lazy-eval",
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
        .ToDictionary(f => f, f => File.ReadAllText(f));

      const int rounds = 150;

      Func<string, int> runTest = file => Enumerable
                                            .Range(0, rounds)
                                            .Select(i =>
                                                      {
                                                        var starttime = DateTime.Now;
                                                        parser.Parse(contents[file]).ToCSS(null);
                                                        var duration = (DateTime.Now - starttime);
                                                        return duration.Milliseconds;
                                                      })
                                            .Sum();

      Console.WriteLine("Press Enter to begin benchmark");
      Console.ReadLine();

      Console.WriteLine("Running each test {0} times", rounds);

      foreach (var file in files)
      {
        var size = rounds*contents[file].Length/1024d;
        Console.Write("{0} : {1,7:#,##0.00} Kb  ", file.PadRight(18), size);
        var time = runTest(file)/1000d;
        Console.WriteLine("{0,6:#.00} s  {1,8:#,##0.00} Kb/s", time, size/time);
      }

      //      Console.Read();
    }
  }
}