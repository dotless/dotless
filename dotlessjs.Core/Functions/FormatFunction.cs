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

      var format = Arguments[0] is Quoted ? ((Quoted) Arguments[0]).UnescapeContents() : Arguments[0].ToCSS(null);

      var args = Arguments.Skip(1).Select(n => n.ToCSS(null)).ToArray();

      var result = string.Format(format, args);

      return new Quoted(result);
    }
  }

}