using System.Collections.Generic;
using System.Linq;
using dotless.Infrastructure;
using dotless.Utils;

namespace dotless.Tree
{
  public class Expression : Node
  {
    public NodeList Value { get; set; }

    public Expression(NodeList value)
    {
      Value = value;
    }
    
    public override Node Evaluate (Env env)
    {
      if (Value.Count > 1)
        return new Expression(new NodeList(Value.Select(e => e.Evaluate(env))));

      return Value[0].Evaluate(env);
    }

    public override string  ToCSS()
    {
      return Value.Select(e => e.ToCSS()).JoinStrings(" ");
    }
  }
}