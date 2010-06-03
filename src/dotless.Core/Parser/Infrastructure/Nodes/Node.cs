namespace dotless.Core.Parser.Infrastructure.Nodes
{
    using System;

    public abstract class Node
  {
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

    public virtual string ToCSS()
    {
      throw new InvalidOperationException(string.Format("ToCSS() not valid on this type of node. '{0}'", GetType().Name));
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