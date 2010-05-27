using System;
using System.Collections.Generic;
using System.Linq;
using dotless.Infrastructure;
using dotless.Utils;

namespace dotless.Tree
{
  public class Directive : Ruleset
  {
    public string Name { get; set; }
    public Node Value { get; set; }

    public Directive(string name, List<Node> rules)
    {
      Name = name;
      Rules = rules;
    }

    public Directive(string name, Node value)
    {
      Name = name;
      Value = value;
    }

    protected Directive()
    {
    }

    public override Node Evaluate(Env env)
    {
      env.Frames.Push(this);

      if(Rules != null)
        Rules = new List<Node>(Rules.Select(r => r.Evaluate(env)));
      else
        Value = Value.Evaluate(env);

      env.Frames.Pop();

      return this;
    }

    protected override string ToCSS(Context context)
    {
      if (Rules != null)
        return Name + " {\n" + Rules.Select(r => r.ToCSS()).JoinStrings("\n").Trim().Indent(2) + "\n}\n";

      return Name + " " + Value.ToCSS() + ";\n";
    }
  }
}