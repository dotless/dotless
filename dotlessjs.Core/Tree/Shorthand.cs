using dotless.Infrastructure;

namespace dotless.Tree
{
  public class Shorthand : Node
  {
    public Node First { get; set; }
    public Node Second { get; set; }

    public Shorthand(Node first, Node second)
    {
      First = first;
      Second = second;
    }

    public override string ToCSS(Env env)
    {
      return First.ToCSS(env) + "/" + Second.ToCSS(env);
    }
  }
}