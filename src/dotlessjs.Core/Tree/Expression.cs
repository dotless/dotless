using System.Linq;
using dotless.Infrastructure;
using dotless.Utils;

namespace dotless.Tree
{
  public class Expression : Node, IEvaluatable
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

    public override string  ToCSS(Env env)
    {
      var evaled = Value
        .Select(e => e is IEvaluatable ? e.Evaluate(env) : e)
        .Select(e => e.ToCSS(env));

      return evaled.JoinStrings(" ");
    }
  }
}