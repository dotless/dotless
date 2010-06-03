namespace dotless.Core.Parser.Functions
{
    using System;
    using System.Linq;
    using Infrastructure.Nodes;
    using Tree;

    public class FormatStringFunction : Function
  {
    protected override Node Evaluate()
    {
      if (Arguments.Count == 0)
        return new Quoted("");

      Func<Node, string> unescape = n => n is Quoted ? ((Quoted) n).UnescapeContents() : n.ToCSS();

      var format = unescape(Arguments[0]);

      var args = Arguments.Skip(1).Select(unescape).ToArray();

      var result = string.Format(format, args);

      return new Quoted(result);
    }
  }

}