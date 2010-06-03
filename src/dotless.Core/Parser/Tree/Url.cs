namespace dotless.Core.Parser.Tree
{
    using Infrastructure.Nodes;

    public class Url : Node
  {
    public Node Value { get; set; }

    public Url(Node value)
    {
      Value = value;
    }

    public string GetUrl()
    {
      if (Value is Quoted)
        return ((Quoted) Value).Contents;

      return Value.ToCSS();
    }

    public override string ToCSS()
    {
      return "url(" + Value.ToCSS() + ")";
    }
  }
}