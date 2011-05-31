namespace dotless.Core.Parser.Infrastructure.Nodes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using dotless.Core.Utils;

    public static class NodeUtils
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
            return (node, builder) => { node.ToCSS(env, builder); };
        }
    }
}
