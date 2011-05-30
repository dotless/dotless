namespace dotless.Core.Parser.Tree
{
    using System.Collections.Generic;
    using Infrastructure;
    using Infrastructure.Nodes;
    using System.Text;

    public class Combinator : Node
    {
        public string Value { get; set; }

        public Combinator(string value)
        {
            if (string.IsNullOrEmpty(value))
                Value = "";
            else if (value == " ")
                Value = " ";
            else if (value == "& ")
                Value = "& ";
            else
                Value = value.Trim();
        }

        public override void ToCSS(Env env, StringBuilder output)
        {
            output.Append(
                new Dictionary<string, string> { 
                  { "", "" }, 
                  { " ", " " },
                  { "&", "" },
                  { "& ", " " },
                  { ":", " :" },
                  { "::", "::" },
                  { "+", env.Compress ? "+" : " + " },
                  { "~", env.Compress ? "~" : " ~ " },
                  { ">", env.Compress ? ">" : " > " } 
              }[Value]);
        }
    }
}