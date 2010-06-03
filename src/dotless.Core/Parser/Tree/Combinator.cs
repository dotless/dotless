namespace dotless.Core.Parser.Tree
{
    using Infrastructure.Nodes;

    public class Combinator : Node
  {
    public string Value { get; set; }

    public Combinator(string value)
    {
      if (string.IsNullOrEmpty(value))
        Value = "";
      else if (value == " ")
        Value = " ";
      else
        Value = value.Trim();
    }

    public override string ToCSS()
    {
      switch (Value)
      {
        case "":   return "";
        case " ":  return " ";
        case "&":  return "";
        case ":":  return " :";
        case "::": return "::";
        case "+":  return " + ";
        case "~":  return " ~ ";
        case ">":  return " > ";
      }
      return "";
    }
  }
}