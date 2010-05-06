using System.Collections.Generic;
using System.Linq;
using dotless.Infrastructure;
using dotless.Utils;

namespace dotless.Tree
{
  public class Value : Node, IEvaluatable
  {
    public NodeList Values { get; set; }
    public Node Important { get; set; }

    public Value(IEnumerable<Node> values, Node important)
    {
      Values = new NodeList(values);
      Important = important;
    }

    public override string ToCSS(Env env)
    {
      return Values.Select(v => v.ToCSS(env)).JoinStrings(", ");
    }

    public override string ToString()
    {
      return ToCSS(null);
    }

    public override Node Evaluate(Env env)
    {
      if (Values.Count == 1)
        return Values[0].Evaluate(env);

      return new Value(Values.Select(n => n.Evaluate(env)), Important);
    }
  }
}