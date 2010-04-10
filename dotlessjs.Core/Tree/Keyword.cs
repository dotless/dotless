using dotless.Infrastructure;

namespace dotless.Tree
{
  public class Keyword : Node, IEvaluatable
  {
    public string Value { get; set; }

    public Keyword(string value)
    {
      Value = value;
    }

    public override Node Evaluate(Env env)
    {
      return (Node) Color.GetColorFromKeyword(Value) ?? this;
    }

    public override string ToCSS(Env env)
    {
      return Value;
    }
  }
}