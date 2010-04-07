using dotless.Infrastructure;

namespace dotless.Tree
{
  public class Url : Node
  {
    public Node Value { get; set; }

    public Url(Node value)
    {
      Value = value;
    }

    public string GetUrl(Env env)
    {
      if (Value is Quoted)
        return ((Quoted) Value).Contents;

      return Value.ToCSS(env);
    }

    public override string ToCSS(Env env)
    {
      return "url(" + Value.ToCSS(env) + ")";
    }
  }
}