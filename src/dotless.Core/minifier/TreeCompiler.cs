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
    using System.Text;

    public class TreeCompiler : ITreeCompiler
    {
        public string CompileTree(ITreeNode rootNode)
        {
            StringBuilder builder = new StringBuilder();
            Compile(rootNode, builder);
            return builder.ToString();
        }

        private void Compile(ITreeNode node, StringBuilder builder)
        {
            if (node.Descriptor != "ROOT")
            {
                builder.Append(node.Descriptor);
                builder.Append('{');
            }
            foreach(var child in node.Children)
            {
                Compile(child, builder);
            }

            foreach(var expression in node.Expressions)
            {
                builder.Append(expression.Expression.Key);
                builder.Append(':');
                builder.Append(expression.Expression.Value);
                builder.Append(';');
            }

            if (node.Descriptor != "ROOT")
            {
                builder.Append('}');
            }
        }
    }
}