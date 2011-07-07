namespace dotless.Core.Parser.Infrastructure.Nodes
{
    using System;

    public abstract class Node
    {
        public int Index { get; set; }

		public NodeList PreComments { get; set; }
		public NodeList PostComments { get; set; }

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

		public TN1 CopiedFrom<TN1>(Node node) where TN1 : Node
		{
            if (node == this)
            {
                return (TN1)node;
            }
			//TODO - only used in one place?
			Index = node.Index;

            if (node.PreComments)
            {
                if (PreComments)
                {
                    PreComments.AddRange(node.PreComments);
                }
                else
                {
                    PreComments = node.PreComments;
                }
            }

            if (node.PostComments)
            {
                if (PostComments)
                {
                    PostComments.AddRange(node.PostComments);
                }
                else
                {
                    PostComments = node.PostComments;
                }
            }
			
			return (TN1)this;
		}

        public virtual void AppendCSS(Env env)
        {
            throw new InvalidOperationException(string.Format("AppendCSS() not valid on this type of node. '{0}'",
                                                              GetType().Name));
        }

        public virtual string ToCSS(Env env)
        {
            env.Output.Push()
				.Append(this);
            return env.Output.Pop().ToString();
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