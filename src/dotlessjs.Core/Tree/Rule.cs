using dotless.Infrastructure;

namespace dotless.Tree
{
  public class Rule : Node
  {
    public string Name { get; set; }
    public Node Value { get; set; }
    public bool Variable { get; set; }

    public Rule(string name, Node value)
    {
      Name = name;
      Value = value;
      Variable = name != null ? name[0] == '@' : false;
    }

    public override Node Evaluate(Env env)
    {
      return new Rule(Name, Value.Evaluate(env));
    }

    public override string ToCSS(Env env)
    {
      if (Variable)
        return "";
      
      return Name + ": " + Value.ToCSS(env) + ";";
    }
  }
}