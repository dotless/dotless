﻿using System.Text;
namespace dotless.Core.Parser.Infrastructure.Nodes
{
    public class TextNode : Node
    {
        public string Value { get; set; }

        public TextNode(string contents)
        {
            Value = contents;
        }

        public static TextNode operator &(TextNode n1, TextNode n2)
        {
            return n1 != null ? n2 : null;
        }

        public static TextNode operator |(TextNode n1, TextNode n2)
        {
            return n1 ?? n2;
        }

        public override void ToCSS(Env env, StringBuilder output)
        {
            output.Append(env != null && env.Compress ? Value.Trim() : Value);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}