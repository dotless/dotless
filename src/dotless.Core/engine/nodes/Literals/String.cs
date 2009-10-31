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

namespace dotless.Core.engine
{
    public class String : Literal
    {
        public string Content { get; set; }
        public string Quotes { get; set; }
        
        public String(string str)
        {
            
            switch(str.Substring(0,1))
            {
                case "\"":
                    Quotes = "\"";
                    Content = str.Replace("\"", "");
                    break;
                case "'":
                    Quotes = "'";
                    Content = str.Replace("'", "");
                    break;
                default:
                    Quotes = string.Empty;
                    Content = string.Empty;
                    break;
            }
            Value = Content;
            //TODO: learn bloody RegEx
            //var pattern = new Regex(@"('|"")(.*?)()");
            //var match = pattern.Matches(str);
            //if (match.Count <= 1) return;
            //Quotes = match[0].Value;
            //Content = match[1].Value;
        }

        public override string ToString()
        {
            return Content;
        }
        public override string ToCss()
        {
            return string.Format("{0}{1}{0}", Quotes, Content);
        }
    }
}