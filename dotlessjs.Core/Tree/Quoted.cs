using dotless.Infrastructure;

namespace dotless.Tree
{
  public class Quoted : Node
  {
    public string Value { get; set; }
    public string Contents { get; set; }

    public Quoted(string value, string contents)
    {
      Value = value;
      Contents = contents;
    }

    public override string ToCSS(Env env)
    {
      return Value;
    }
  }
}