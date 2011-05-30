namespace dotless.Core.Parser.Infrastructure.Nodes
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using dotless.Core.Utils;

    public abstract class Node
    {
        public int Index { get; set; }

        protected static StringBuilder ToCSS(IEnumerable<Node> nodes, Env env)
        {
            return ToCSS(nodes, env, null);
        }

        protected static StringBuilder ToCSS(IEnumerable<Node> nodes, Env env, string joinString)
        {
            StringBuilder output = new StringBuilder();
            nodes.JoinStringBuilder(output, Node.StringBuilderAction(env), joinString);
            return output;
        }

        protected static Action<Node, StringBuilder> StringBuilderAction(Env env)
        {
            return (node, builder) => { node.ToCSS(env, builder); };
        }

        #region Boolean Operators

        public static implicit operator bool(Node node)
        {
            return node != null;
        }

        public static bool operator true(Node n)
        {
            return n != null;
        }

        public static bool operator false(Node n)
        {
            return n == null;
        }

        public static bool operator !(Node n)
        {
            return n == null;
        }

        public static Node operator &(Node n1, Node n2)
        {
            return n1 != null ? n2 : null;
        }

        public static Node operator |(Node n1, Node n2)
        {
            return n1 ?? n2;
        }

        #endregion

        public virtual void ToCSS(Env env, StringBuilder output)
        {
            throw new InvalidOperationException(string.Format("ToCSS() not valid on this type of node. '{0}'",
                                                              GetType().Name));
        }

        public virtual string ToCSS(Env env)
        {
            StringBuilder sb = new StringBuilder();
            ToCSS(env, sb);
            return sb.ToString();
        }

        public virtual Node Evaluate(Env env)
        {
            return this;
        }

        public bool IgnoreOutput()
        {
            return
                this is RegexMatchResult ||
                this is CharMatchResult;
        }
    }
}