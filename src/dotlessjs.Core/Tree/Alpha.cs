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
      return this;
    }

    public override string ToCSS(Env env)
    {
      return string.Format("alpha(opacity={0})", Value.ToCSS(env));
    }
  }
}