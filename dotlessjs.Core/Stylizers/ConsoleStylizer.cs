using System;
using System.Collections.Generic;

namespace dotless.Stylizers
{
  public class ConsoleStylizer : IStylizer
  {
    private Dictionary<string, int[]> styles;

    public ConsoleStylizer()
    {
      styles = new Dictionary<string, int[]>
                 {
                   {"bold",       new[] {1, 22}},
                   {"inverse",    new[] {7, 27}},
                   {"underline",  new[] {4, 24}},
                   {"yellow",     new[] {33, 39}},
                   {"green",      new[] {32, 39}},
                   {"red",        new[] {31, 39}},
                   {"reset",      new[] {0, 0}}
                 };
    }

    private string Stylize(string str, string style)
    {
      return "\x1b[" + styles[style][0] + "m" + str +
             "\x1b[" + styles[style][1] + "m";
    }

    public string Stylize(string str, int errorPosition)
    {
      return Stylize(str.Substring(0, errorPosition), "green") +
             Stylize(
               Stylize(str[errorPosition].ToString(), "inverse") + str.Substring(errorPosition + 1), "yellow") +
             Stylize("", "reset");
    }
  }
}