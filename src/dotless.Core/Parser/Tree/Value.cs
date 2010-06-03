namespace dotless.Core.Parser.Tree
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using Infrastructure.Nodes;
    using Utils;

    public class Value : Node
  {
    public NodeList Values { get; set; }
    public Node Important { get; set; }

    public Value(IEnumerable<Node> values, Node important)
    {
      Values = new NodeList(values);
      Important = important;
    }

    public override string ToCSS()
    {
      return Values.Select(v => v.ToCSS()).JoinStrings(", ");
    }

    public override string ToString()
    {
      return ToCSS();
    }

    public override Node Evaluate(Env env)
    {
      if (Values.Count == 1)
        return Values[0].Evaluate(env);

      return new Value(Values.Select(n => n.Evaluate(env)), Important);
    }
  }
}