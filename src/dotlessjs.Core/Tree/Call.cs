using System;
using System.Linq;
using dotless.Infrastructure;
using dotless.Utils;

namespace dotless.Tree
{
  public class Call : Node, IEvaluatable
  {
    public string Name { get; set; }
    public NodeList<Expression> Arguments { get; set; }

    public Call(string name, NodeList<Expression> arguments)
    {
      Name = name;
      Arguments = arguments;
    }

    protected Call()
    {
    }

    public override Node Evaluate(Env env)
    {
      var args = Arguments.Select(a => a.Evaluate(env));
 
      if (env != null)
      {
        var function = env.GetFunction(Name);

        if (function != null)
        {
          function.Name = Name;
          return function.Call(args);
        }
      }

      return this;
    }

    public override string ToCSS(Env env)
    {
      var evaled = Evaluate(env);
      
      if(evaled != null && evaled != this)
        return evaled.ToCSS(env);

      return Name + "(" + Arguments.Select(a => a.ToCSS(env) ).JoinStrings(", ") + ")";
    }
  }
}