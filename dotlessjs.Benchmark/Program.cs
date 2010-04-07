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

      var sizes = files.ToDictionary(f => f, f => contents[f].Length/1024d); // convert bytes to Kbytes

      var rounds = 60;

      var times = files
        .ToDictionary(f => f, f => Enumerable.Range(0, rounds)
                                     .Select(i =>
                                               {
                                                 var starttime = DateTime.Now;
                                                 parser.Parse(contents[f]).ToCSS(null);
                                                 var duration = (DateTime.Now - starttime);
                                                 return duration.Milliseconds;
                                               })
                                     .Average());
      Console.WriteLine();

      var rates = files.ToDictionary(f => f, f => 1000*sizes[f]/times[f]); // convert ms into s

      foreach (var file in files)
      {
        Console.WriteLine("{0}\t{1:0000} ms\t{2:0.000} Kb\t{3:0.00} Kb/s", file.PadRight(18), times[file], sizes[file], rates[file]);
      }

//      Console.Read();
    }
  }
}