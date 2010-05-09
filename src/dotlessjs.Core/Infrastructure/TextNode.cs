namespace dotless.Infrastructure
{
  public class TextNode : Node
  {
    public string Value { get; set; }

    public TextNode(string contents)
    {
      Value = contents;
    }

    public static TextNode operator &(TextNode n1, TextNode n2)
    {
      return n1 != null ? n2 : null;
    }

    public static TextNode operator |(TextNode n1, TextNode n2)
    {
      return n1 ?? n2;
    }

    public override string ToCSS()
    {
      return Value;
    }

    public override string ToString()
    {
      return Value;
    }
  }
}