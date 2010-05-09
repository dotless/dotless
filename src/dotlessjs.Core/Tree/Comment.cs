using dotless.Infrastructure;

namespace dotless.Tree
{
  public class Comment : Node
  {
    public string Value { get; set; }

    public Comment(string value)
    {
      Value = value;
    }

    public override string ToCSS()
    {
      return Value;
    }
  }
}