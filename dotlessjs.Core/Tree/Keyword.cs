using dotless.Infrastructure;

namespace dotless.Tree
{
  public class Keyword : Node
  {
    public string Value { get; set; }

    public Keyword(string value)
    {
      Value = value;
    }

    public override string ToCSS(Env env)
    {
      return Value;
    }
  }
}