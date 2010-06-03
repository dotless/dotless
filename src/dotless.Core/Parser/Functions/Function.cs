using System.Collections.Generic;
using System.Linq;
using dotless.Infrastructure;

namespace dotless.Functions
{
  public abstract class Function
  {
    public string Name { get; set; }
    protected List<Node> Arguments { get; set; }

    public Node Call(IEnumerable<Node> arguments)
    {
      Arguments = arguments.ToList();

      return Evaluate();
    }

    protected abstract Node Evaluate();

    public override string ToString()
    {
      return string.Format("function '{0}'", Name.ToLowerInvariant());
    }
  }
}