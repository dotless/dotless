namespace dotless.Core.Parser.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Nodes;

    public class Output
    {
        private Env Env { get; set; }
        private StringBuilder Builder { get; set; }
        private Stack<StringBuilder> BuilderStack { get; set; }

        public Output(Env env)
        {
            Env = env;
            BuilderStack = new Stack<StringBuilder>();

            Push();
        }

        public Output Push()
        {
            Builder = new StringBuilder();

            BuilderStack.Push(Builder);

            return this;
        }

        public StringBuilder Pop()
        {
            if (BuilderStack.Count == 1)
                throw new InvalidOperationException();

            var sb = BuilderStack.Pop();

            Builder = BuilderStack.Peek();

            return sb;
        }

        public void Reset(string s)
        {
            Builder = new StringBuilder(s);

            BuilderStack.Pop();
            BuilderStack.Push(Builder);
        }

        public Output PopAndAppend()
        {
            return Append(Pop());
        }

        public Output Append(Node node)
        {
            if (node != null)
            {
                if (node.PreComments)
                    node.PreComments.AppendCSS(Env);

                node.AppendCSS(Env);

                if (node.PostComments)
                    node.PostComments.AppendCSS(Env);
            }

            return this;
        }

        public Output Append(string s)
        {
            Builder.Append(s);

            return this;
        }

        public Output Append(char? s)
        {
            Builder.Append(s);

            return this;
        }

        public Output Append(StringBuilder sb)
        {
            Builder.Append(sb);

            return this;
        }

        public Output AppendMany<TNode>(IEnumerable<TNode> nodes)
            where TNode : Node
        {
            return AppendMany(nodes, null);
        }

        public Output AppendMany<TNode>(IEnumerable<TNode> nodes, string join)
            where TNode : Node
        {
            return AppendMany(nodes, n => Env.Output.Append(n), join);
        }

        public Output AppendMany<T>(IEnumerable<T> list, Func<T, string> toString, string join)
        {
            return AppendMany(list, (item, sb) => sb.Append(toString(item)), join);
        }

        public Output AppendMany<T>(IEnumerable<T> list, Action<T> toString, string join)
        {
            return AppendMany(list, (item, sb) => toString(item), join);
        }

        public Output AppendMany<T>(IEnumerable<T> list, Action<T, StringBuilder> toString, string join)
        {
            var first = true;
            var hasJoinString = !string.IsNullOrEmpty(join);

            foreach (var item in list)
            {
                if (!first && hasJoinString)
                    Builder.Append(join);

                first = false;
                toString(item, Builder);
            }

            return this;
        }

        public Output AppendMany(IEnumerable<StringBuilder> buildersToAppend)
        {
            return AppendMany(buildersToAppend, null);
        }

        public Output AppendMany(IEnumerable<StringBuilder> buildersToAppend, string join)
        {
            return AppendMany(buildersToAppend, (b, output) => output.Append(b), join);
        }

        public Output AppendFormat(IFormatProvider formatProvider, string format, params object[] values)
        {
            Builder.AppendFormat(formatProvider, format, values);

            return this;
        }

        public Output Indent(int amount)
        {
            if (amount > 0)
            {
                var indentation = new string(' ', amount);
                Builder.Replace("\n", "\n" + indentation);
                Builder.Insert(0, indentation);
            }

            return this;
        }

        /// <summary>
        ///  Trims whitespace
        /// </summary>
        public Output Trim()
        {
            return this.TrimLeft(null).TrimRight(null);
        }

        /// <summary>
        /// Trims the character passed or whitespace if it has no value from the left
        /// </summary>
        public Output TrimLeft(char? c)
        {
            int trimLLength = 0;
            int length = Builder.Length;

            if (length == 0)
            {
                return this;
            }

            while (trimLLength < length && 
                    c.HasValue ? Builder[trimLLength] == c.Value : 
                                char.IsWhiteSpace(Builder[trimLLength]))
            {
                trimLLength++;
            }

            if (trimLLength > 0)
            {
                Builder.Remove(0, trimLLength);
            }

            return this;
        }

        /// <summary>
        /// Trims the character passed or whitespace if it has no value from the left
        /// </summary>
        public Output TrimRight(char? c)
        {
            int trimRLength = 0;
            int length = Builder.Length;

            if (length == 0)
            {
                return this;
            }

            while (trimRLength < length && 
                   c.HasValue ? Builder[length - (trimRLength + 1)] == c.Value :
                        char.IsWhiteSpace(Builder[length - (trimRLength + 1)]))
            {
                trimRLength++;
            }

            if (trimRLength > 0)
            {
                Builder.Remove(length - trimRLength, trimRLength);
            }

            return this;
        }


        public override string ToString()
        {
            return Builder.ToString();
        }
    }
}