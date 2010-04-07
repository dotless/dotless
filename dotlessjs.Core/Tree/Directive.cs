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

    public Directive(string name, NodeList rules)
    {
      Name = name;
      Rules = rules;
    }

    public Directive(string name, Node value)
    {
      Name = name;
      Value = value;
    }

    public override string ToCSS(List<IEnumerable<Selector>> context, Env env)
    {
      if (Rules)
        return Name + " {\n  " + Rules.Select(r => r.ToCSS(env)).JoinStrings("\n  ") + "\n}\n";

      return Name + ' ' + Value.ToCSS(env) + ";\n";
    }
  }
}