using dotless.Infrastructure;

namespace dotless.Tree
{
  public class Alpha : Call
  {
    public Node Value { get; set; }

    public Alpha(Node value)
    {
      Value = value;
    }

    public override Node Evaluate(Env env)
    {
      Value = Value.Evaluate(env);

      return this;
    }

    public override string ToCSS()
    {
      return string.Format("alpha(opacity={0})", Value.ToCSS());
    }
  }
}