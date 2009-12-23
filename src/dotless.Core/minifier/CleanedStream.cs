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

namespace dotless.Core.minifier
{
    using System.Text.RegularExpressions;

    public class WhiteSpaceFilter
    {
        private static Regex CreateRegex(string pattern)
        {
            return new Regex(pattern, RegexOptions.Multiline);
        }

        public static string RemoveLeadingAndTrailingWhiteSpace(string input)
        {
            //Leading Whitespace: ^\s+
            //Trailing Whitespace: \s+$
            var regex = CreateRegex(@"^\s+|\s+$");
            return regex.Replace(input, "");
        }

        public static string ConvertToUnix(string input)
        {
            return CreateRegex(@"\r").Replace(input, "");
        }

        public static string RemoveComments(string input)
        {
            Regex regex = CreateRegex("(?<!:)//(.)*$");
            return regex.Replace(input, "");
        }

        public static string RemoveExtendedComments(string input)
        {
            // Clear Regex: (?<!\".*)/\*(?!!)(.|\n)*\*/(?!.*\")
            //Note: Does not remove forced comments /*!
            return CreateRegex("(?<!\\\".*)/\\*(?!!)(.|\\n)*\\*/(?!.*\\\")").Replace(input, "");
        }

        public static string RemoveMultipleWhiteSpaces(string input)
        {
            return CreateRegex(" {2,}").Replace(input, " ");
        }

        public static string RemoveNewLines(string input)
        {
            return CreateRegex(@"\r?\n?").Replace(input, "");
        }
    }
}