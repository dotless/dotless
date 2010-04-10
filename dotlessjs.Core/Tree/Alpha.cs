using dotless.Infrastructure;

namespace dotless.Tree
{
  public class Alpha : Node
  {
    public Node Value { get; set; }

    public Alpha(Node value)
    {
      Value = value;
    }

    public override string ToCSS(Env env)
    {
      return string.Format("alpha(opacity={0})", Value.ToCSS(env));
    }
  }
}