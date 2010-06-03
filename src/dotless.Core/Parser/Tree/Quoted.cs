namespace dotless.Core.Parser.Tree
{
    using System.Text.RegularExpressions;
    using Infrastructure.Nodes;

    public class Quoted : Node
  {
    public string Value { get; set; }
    public string Contents { get; set; }

    public Quoted(string value, string contents)
    {
      Value = value;
      Contents = contents;
    }

    public Quoted(string value)
      : this(value, value)
    {
    }

    public override string ToCSS()
    {
      return Value;
    }

    private readonly Regex _unescape = new Regex(@"(^|[^\\])\\(.)");
    public string UnescapeContents()
    {
      return _unescape.Replace(Contents, @"$1$2");
    }
  }
}