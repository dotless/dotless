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

using System.Text.RegularExpressions;

namespace dotless.Core.engine
{
    public class String : Literal
    {
        public string Content { get; set; }
        public string Quotes { get; set; }

        private readonly Regex pattern = new Regex(@"
^
(?<quotes> "" (?<double>) | ' (?<single>) | )
(?<content>(?>
    "" (?(double)(?!))    # allow double quotes if not inside double quotes
  | 
    ' (?(single)(?!))     # allow single quotes if not inside single quotes
  |
    [^\\]                 # anything but escape character
  |
    \\.                   # escape sequence
)*)
( "" (?(single)(?!)) | ' (?(double)(?!)) | (?(double)(?!)) (?(single)(?!)) )
$", RegexOptions.IgnorePatternWhitespace);

        private readonly Regex unescape = new Regex(@"(^|[^\\])\\(.)");


        public String(string str)
        {
            var match = pattern.Match(str);

            Quotes = match.Groups["quotes"].ToString();
            Content = match.Groups["content"].ToString();
        }

        public override string ToString()
        {
            return unescape.Replace(Content, @"$1$2");
        }
        public override string ToCss()
        {
            return string.Format("{0}{1}{0}", Quotes, ToString());
        }
    }
}