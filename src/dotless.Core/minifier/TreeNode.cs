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

namespace dotless.Core.minifier
{
    using System.Collections.Generic;

    public class TreeNode : ITreeNode
    {
        public TreeNode(string descriptor, ITreeNode parent)
        {
            Descriptor = descriptor;
            Parent = parent;
        }
        private IList<IExpression> expressions = new List<IExpression>();
        private IList<ITreeNode> children = new List<ITreeNode>();

        public IEnumerable<ITreeNode> Children
        {
            get { return children; }
        }

        public IEnumerable<IExpression> Expressions
        {
            get { return expressions; }
        }

        public ITreeNode Parent { get; private set; }

        public string Descriptor { get; private set; }

        public void AppendExpression(IExpression expression)
        {
            expressions.Add(expression);
        }

        public void AppendChild(ITreeNode child)
        {
            children.Add(child);
        }
    }
}