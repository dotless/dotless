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
    public class Processor
    {
        private ITreeCompiler compiler = new TreeCompiler();
        private ITokenizer tokenizer = new Tokenizer();
        private string input;
        private readonly char[] output;

        public Processor(string input)
        {
            this.input = input;
            output = CleanInput().ToCharArray();
        }

        private string CleanInput()
        {
            input = WhiteSpaceFilter.ConvertToUnix(input);
            input = WhiteSpaceFilter.RemoveComments(input);
            input = WhiteSpaceFilter.RemoveMultipleWhiteSpaces(input);
            input = WhiteSpaceFilter.RemoveLeadingAndTrailingWhiteSpace(input);
            input = WhiteSpaceFilter.RemoveNewLines(input);
            input = WhiteSpaceFilter.RemoveExtendedComments(input);

            ITreeNode tree = tokenizer.BuildTree(input);
            input = compiler.CompileTree(tree);
            return input;
        }

        public char[] Output
        {
            get { return output; }
        }

        public ITreeCompiler Compiler
        {
            get { return compiler; }
            set { compiler = value; }
        }

        public ITokenizer Tokenizer
        {
            get { return tokenizer; }
            set { tokenizer = value; }
        }
    }
}