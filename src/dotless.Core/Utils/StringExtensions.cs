using dotless.Core.Parser.Infrastructure;
using dotless.Core.Parser.Infrastructure.Nodes;

namespace dotless.Core.Utils
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System;

    public static class StringExtensions
    {
        public static StringBuilder AppendCSS(this StringBuilder output, Node node, Env env)
        {
            return node.ToCSS(env, output);
        }

        public static StringBuilder ToCSS<T1>(this IEnumerable<T1> nodes, Env env) where T1 : Node
        {
            return AppendCSS(null, nodes, env, null);
        }

        public static StringBuilder ToCSS<T1>(this IEnumerable<T1> nodes, Env env, string joinString) where T1 : Node
        {
            return AppendCSS(null, nodes, env, joinString);
        }

        public static StringBuilder AppendCSS<T1>(this StringBuilder builder, IEnumerable<T1> nodes, Env env, string joinString) where T1 : Node
        {
            return nodes.JoinStringBuilder(builder, StringBuilderAction<T1>(env), joinString);
        }

        public static Action<T1, StringBuilder> StringBuilderAction<T1>(Env env) where T1 : Node
        {
            return (node, builder) => node.ToCSS(env, builder);
        }

        public static StringBuilder JoinStringBuilder<T1>(this IEnumerable<T1> list, StringBuilder output, Action<T1, StringBuilder> toString)
        {
            return list.JoinStringBuilder(output, toString, null);
        }

        public static StringBuilder JoinStringBuilder<T1>(this IEnumerable<T1> list, StringBuilder output, Action<T1, StringBuilder> toString, string join)
        {
            var first = true;
            var hasJoinString = !string.IsNullOrEmpty(join);

            output = output ?? new StringBuilder();

            foreach(var item in list)
            {
                if (!first && hasJoinString)
                    output.Append(join);

                first = false;
                toString(item, output);
            }

            return output;
        }

        public static StringBuilder AppendJoin(this StringBuilder builderToAppendTo, IEnumerable<StringBuilder> buildersToAppend)
        {
            return builderToAppendTo.AppendJoin(buildersToAppend, null); 
        }

        public static StringBuilder AppendJoin(this StringBuilder builderToAppendTo, IEnumerable<StringBuilder> buildersToAppend, string joinString)
        {
            return buildersToAppend.JoinStringBuilder(builderToAppendTo, (builderToAppend, output) => output.Append(builderToAppend), joinString);
        }

        public static StringBuilder Indent(this StringBuilder builder, int amount)
        {
            if (amount > 0)
            {
                string indentation = new string(' ', amount);
                builder.Replace("\n", "\n" + indentation);
                builder.Insert(0, indentation);
            }

            return builder;
        }

        public static StringBuilder Trim(this StringBuilder builder)
        {
            int trimLLength = 0;
            int length = builder.Length;

            while (trimLLength < length && char.IsWhiteSpace(builder[trimLLength]))
            {
                trimLLength++;
            }

            if (trimLLength > 0)
            {
                builder.Remove(0, trimLLength);
                length -= trimLLength;
            }

            int trimRLength = 0;
            while (trimRLength < length && char.IsWhiteSpace(builder[length - (trimRLength + 1)]))
            {
                trimRLength++;
            }
            if (trimRLength > 0)
            {
                builder.Remove(length - trimRLength, trimRLength);
            }

            return builder;
        }

        public static string Indent(this string str, int indent)
        {
            if(indent == 0) return str;

            var space = new string(' ', indent);
            return space + str.Replace("\n", "\n" + space);
        }

        public static string JoinStrings(this IEnumerable<string> source, string separator)
        {
            return string.Join(separator, source.ToArray());
        }

        public static string AggregatePaths(this IEnumerable<string> source)
        {
            if (!source.Any())
                return "";
  
            var path = source.Aggregate("", Path.Combine);

            return CanonicalizePath(path);
        }

        /// <summary>
        /// Splits the given path into segments and resolves any parent path references
        /// it can (eg. "foo/../bar" becomes "bar" whereas "../foo" is left as-is).
        /// </summary>
        private static string CanonicalizePath(string path)
        {
            var pathStack = new Stack<string>();

            // Split assuming we might get any combination of backward and forward slashes
            string[] segments = path.Split('\\', '/');
            foreach (string segment in segments)
            {
                // If the parent reference is the first one on the stack, do nothing 
                // (because there's nothing we can do, and removing it would break the path)
                if (segment.Equals("..") && pathStack.Count > 0 && pathStack.Peek() != "..")
                    pathStack.Pop();
                else
                    pathStack.Push(segment);
            }

            // Recombine the path segments. Note that there is a difference between doing this
            // and pathStack.Reverse().Aggregate("", Path.Combine), which would discard empty path
            // segments (and therefore strip leading slashes)
            return string.Join("/", pathStack.Reverse().ToArray());
        }
    }
}