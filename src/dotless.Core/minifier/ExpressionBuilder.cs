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
    using System;
    using System.Text;

    public class ExpressionBuilder : IExpressionBuilder
    {
        public IExpression BuildExpression(char[] input)
        {
            var builder = new StringBuilder();
            bool completedDescriptor = false;
            string descriptor = null;
            bool escaped = false;
            char escapeChar = '0';
            for (int index = 0; index < input.Length; index++)
            {
                char c = input[index];
                if (!completedDescriptor && c == ':')
                {
                    completedDescriptor = true;
                    descriptor = builder.ToString();
                    builder = new StringBuilder();
                    continue;
                }
                if (escaped)
                {
                    builder.Append(c);
                    escaped = (escapeChar != c);
                }
                else
                {
                    switch(c)
                    {
                        case '\'':
                        case '"':
                            escaped = true;
                            escapeChar = c;
                            break;
                        case ' ':
                            if (IsFollowedByOperator(index, input)) continue;
                            if (IsPrecededByOperator(index, input)) continue;
                            break;
                    }
                    builder.Append(c);
                }
            }
            
            var value = builder.ToString().Trim();
            if (value.StartsWith("/*!"))
                return new CommentExpression(value);
            if (descriptor == null || value == null || string.IsNullOrEmpty(descriptor) || string.IsNullOrEmpty(value))
                return null;
            return new StyleExpression(descriptor.Trim(), value.Trim());
        }

        private bool IsFollowedByOperator(int startIndex, char[] characters)
        {
            for (int index = startIndex; index < characters.Length; index++ )
            {
                char c = characters[index];
                if (c == ' ') continue;
                
                return IsOperator(c) && IsFollowedBySpace(index +1, characters);
            }
            return false;
        }

        private bool IsFollowedBySpace(int index, char[] characters)
        {
            if (characters.Length < index) return false;
            return characters[index] == ' ';
        }

        private bool IsOperator(char c)
        {
            return (c == '-' || c == '+' || c == '*' || c == '/' || c == ',');
        }

        private bool IsPrecededByOperator(int startIndex, char[] characters)
        {
            for (int index = startIndex; index > 0; index--)
            {
                char c = characters[index];
                if (c == ' ') continue;

                return IsOperator(c);
            }
            return false;
        }
    }
}