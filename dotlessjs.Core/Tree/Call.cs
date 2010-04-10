using System.Linq;
using dotless.Infrastructure;
using dotless.Utils;

namespace dotless.Tree
{
  public class Call : Node
  {
    public string Name { get; set; }
    public NodeList Arguments { get; set; }

    public Call(string name, NodeList arguments)
    {
      Name = name;
      Arguments = arguments;
    }

    public override string ToCSS(Env env)
    {
      var args = Arguments.Select(a => a.Evaluate(env) );
      var function = env.GetFunction(Name);

      if (function != null)
      {
        function.Name = Name;
        return function.Call(args).ToCSS(env);
      }

      return Name + "(" + args.Select(a => a.ToCSS(env) ).JoinStrings(", ") + ")";
    }
  }
}