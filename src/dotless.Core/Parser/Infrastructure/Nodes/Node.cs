using dotless.Core.Plugins;

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

        /// <summary>
        ///  Copies common properties when evaluating multiple nodes into one
        /// </summary>
        /// <typeparam name="T">Type to return - for convenience</typeparam>
        /// <param name="nodes">The nodes this new node is derived from</param>
        /// <returns>The new node</returns>
        public T ReducedFrom<T>(params Node[] nodes) where T : Node
        {
            foreach (var node in nodes)
            {
                if (node == this)
                {
                    continue;
                }

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
            }
            
            return (T)this;
        }

        public virtual void AppendCSS(Env env)
        {
            //if there is no implementation then it will be a node that
            //evalutes to something.
            //ideally this shouldn't be called, but when creating error message it may be
            Evaluate(env).AppendCSS(env);
        }

        public virtual string ToCSS(Env env)
        {
            return env.Output
                .Push()
                .Append(this)
                .Pop()
                .ToString();
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

        public virtual void Accept(IVisitor visitor) {}

        /// <summary>
        /// Visits the node and throw an exception if the replacement mode isn't the right type, or the replacement is null
        /// </summary>
        public T VisitAndReplace<T>(T nodeToVisit, IVisitor visitor) where T : Node
        {
            return VisitAndReplace(nodeToVisit, visitor, false);
        }

        /// <summary>
        /// Visits the node and throw an exception if the replacement mode isn't the right type
        /// The allowNull parameter determines if a null is allowed to be returned
        /// </summary>
        public T VisitAndReplace<T>(T nodeToVisit, IVisitor visitor, bool allowNull) where T : Node
        {
            Node replacement = visitor.Visit(nodeToVisit);

            T typedReplacement = replacement as T;
            if (typedReplacement != null || (allowNull && replacement == null))
            {
                return typedReplacement;
            }

            throw new Exception();
        }
    }
}