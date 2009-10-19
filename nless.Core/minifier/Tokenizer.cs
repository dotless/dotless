/*
 * Copyright 2009 Less.Net
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *  http://www.apache.org/licenses/LICENSE-2.0
 *  
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace nless.Core.minifier
{
    using System.Collections.Generic;
    using System.Linq;

    public class Tokenizer : ITokenizer
    {
        private IExpressionBuilder expressionBuilder = new ExpressionBuilder();
        private IDescriptorBuilder descriptorBuilder = new DescriptorBuilder();

        public IExpressionBuilder ExpressionBuilder
        {
            get { return expressionBuilder; }
            set { expressionBuilder = value; }
        }

        public IDescriptorBuilder DescriptorBuilder
        {
            get { return descriptorBuilder; }
            set { descriptorBuilder = value; }
        }

        public ITreeNode BuildTree(string input)
        {
            ITreeNode currentNode = new TreeNode("ROOT", null);
            char[] array = input.ToCharArray();

            IList<char> buffer = new List<char>();
            bool escaped = false;
            char escapeCharacter = '0';
            foreach (char c in array)
            {
                if (escaped)
                {
                    buffer.Add(c);
                    escaped = (c != escapeCharacter);
                    continue;
                }

                switch (c)
                {
                    case '{':
                        string descriptor = DescriptorBuilder.BuildDescriptor(buffer.ToArray());
                            
                        var childLevel = new TreeNode(descriptor, currentNode);
                        currentNode.AppendChild(childLevel);
                        buffer = new List<char>();
                        currentNode = childLevel;
                        break;
                    case ';':
                        currentNode.AppendExpression(ExpressionBuilder.BuildExpression(buffer.ToArray()));
                        buffer = new List<char>();
                        break;
                    case '}':
                        currentNode = currentNode.Parent;
                        break;
                    case '"':
                    case '\'':
                        buffer.Add(c);
                        escaped = true;
                        escapeCharacter = c;
                        break;
                    default:
                        buffer.Add(c);
                        break;
                }
            }

            return currentNode;
        }
    }
}