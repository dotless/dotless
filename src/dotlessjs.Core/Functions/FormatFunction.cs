using System;
using System.Linq;
using dotless.Infrastructure;
using dotless.Tree;

namespace dotless.Functions
{
  public class FormatStringFunction : Function
  {
    protected override Node Evaluate()
    {
      if (Arguments.Count == 0)
        return new Quoted("");

      Func<Node, string> unescape = n => n is Quoted ? ((Quoted) n).UnescapeContents() : n.ToCSS(null);

      var format = unescape(Arguments[0]);

      var args = Arguments.Skip(1).Select(unescape).ToArray();

      var result = string.Format(format, args);

      return new Quoted(result);
    }
  }

}