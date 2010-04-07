using System.Collections.Generic;

namespace dotless.Infrastructure
{
  public abstract class Function
  {
    public abstract Node Call(IEnumerable<Node> arguments);
  }
}